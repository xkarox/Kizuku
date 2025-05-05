using Backend.Infrastructure;
using Backend.Infrastructure.Repositories;
using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Moq.EntityFrameworkCore.Dynamic;

namespace BackendTest.Unit.Infrastructure.Repositories;
[Parallelizable]
[TestFixture]
public class UserRepositoryTests
{
    private Mock<IKizukuContext> _mockContext;
    private Mock<DbSet<User>> _mockDbSet;
    private UserRepository _repository;
    private List<User> _users;

    [SetUp]
    public void Setup()
    {
        _users = new List<User>
        {
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "user1",
                Email = "user1@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password"),
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "user2",
                Email = "user2@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password"),
            }
        };

        _mockContext = new Mock<IKizukuContext>();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(_users);
        
        _mockDbSet = new Mock<DbSet<User>>();
        
        _repository = new UserRepository(_mockContext.Object);
    }
    private void AssertUserMatches(User expected, User actual)
    {
        Assert.That(actual.Username, Is.EqualTo(expected.Username));
        Assert.That(actual.Password, Is.EqualTo(expected.Password));
        Assert.That(actual.Email, Is.EqualTo(expected.Email));
        Assert.That(actual.RegisteredAt, Is.EqualTo(expected.RegisteredAt));
    }
    
     
    [Test]
    public async Task Create_WithValidUser_CreatesUser()
    {
        var user = new User
        {
            Username = "moiself",
            Password = BCrypt.Net.BCrypt.HashPassword("password", 12),
            Email = "moiself@example.com",
            RegisteredAt = DateTime.UtcNow
        };
        
        var result = await _repository.Create(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
        AssertUserMatches(user, result.Value);
    }
     
    [Test]
    public async Task Create_WhenDbSavesChanges_ReturnsDatabaseError()
    {
        var user = new User
        {
            Username = "moiself",
            Password = BCrypt.Net.BCrypt.HashPassword("password", 12),
            Email = "moiself@example.com",
            RegisteredAt = DateTime.UtcNow
        };
        _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Simulated database error", new Exception()));
        _repository = new UserRepository(_mockContext.Object);
        
        var result = await _repository.Create(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error.GetType(), Is.EqualTo(typeof(DatabaseError)));
        });
    }
     
    [Test]
    public async Task Create_WithNullUser_ReturnsEntityNullError()
    {
        User user = null;
        
        var result = await _repository.Create(null);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error.GetType(), 
                Is.EqualTo(typeof(EntityNullError<User>)));
        });
    }
     
    [Test]
    public async Task GetById_WithValidUser_ReturnsUser()
    {
        var user = _users.First();
        var id = user.UserId;
        
        var result = await _repository.GetById(id);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
        Assert.That(result.Value, Is.EqualTo(user));
    }
     
    [Test]
    public async Task GetById_WithInvalidUser_ReturnsEntityNotFoundError()
    {
        var id = Guid.NewGuid();
        
        var result = await _repository.GetById(id);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
        });
        var type = result.Error.GetType();
        Assert.That(type, Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
     
    [Test]
    public async Task GetById_WhenDbAccessThrowsArgumentNull_ReturnsDatabaseError()
    {
        var userIdToQuery = Guid.NewGuid();
        var dbException = new ArgumentNullException("db", "Simulierte Exception beim DB-Zugriff.");
        _mockContext.Setup(m => m.Users)
            .Throws(dbException);
        
        var result = await _repository.GetById(userIdToQuery);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error, Is.TypeOf<DatabaseError>());
        });
    }
     
    [Test]
    public async Task Get_WithValidUser_ReturnsUser()
    {
        var user = _users.First();
        
        var result = await _repository.Get(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
        Assert.That(result.Value, Is.EqualTo(user));
    }
     
    [Test]
    public async Task Get_WithInvalidUser_ReturnsEntityNotFoundError()
    {
        var user = new User()
        {
            Username = "Schizo",
            Password = "someUnhashedPassword",
            Email = "schizo@example.com",
            RegisteredAt = DateTime.UtcNow
        };
        
        var result = await _repository.Get(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
        });
        var type = result.Error.GetType();
        Assert.That(type, Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
     
    [Test]
    public async Task Get_WhenDbAccessThrowsArgumentNull_ReturnsDatabaseError()
    {
        var user = _users.First();
        var dbException = new ArgumentNullException("db", "Simulierte Exception beim DB-Zugriff.");
        _mockContext.Setup(m => m.Users)
            .Throws(dbException);
        
        var result = await _repository.Get(user);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error, Is.TypeOf<DatabaseError>());
        });
    }
     
    [Test]
    public async Task GetAll_ReturnsAllUsers()
    {
        var users = _users.ToList();
        
        var result = await _repository.GetAll();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value!.GetType(), Is.EqualTo(typeof(List<User>)));
        Assert.That(result.Value, Is.EquivalentTo(users));
    }
     
    [Test]
    public async Task GetAll_WhenDbAccessThrowsArgumentNull_ReturnsDatabaseError()
    {
        _mockContext.Setup(m => m.Users)
            .Throws(new ArgumentNullException("Simulated database error", new Exception()));
        _repository = new UserRepository(_mockContext.Object);
        
        var result = await _repository.GetAll();
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error.GetType(), Is.EqualTo(typeof(DatabaseError)));
        });
    }
     
    [Test]
    public async Task GetByEmail_WithValidEmail_ReturnsUser()
    {
        var user = _users.First();
        var email = user.Email;
        
        var result = await _repository.GetByEmail(email);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
        Assert.That(result.Value, Is.EqualTo(user));
    }
     
    [Test]
    public async Task GetByEmail_WithInvalidEmail_ReturnsEntityNotFoundError()
    {
        var email = "loit@gravel.com";
        
        var result = await _repository.GetByEmail(email);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
        });
        var type = result.Error.GetType();
        Assert.That(type, Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
     
    [Test]
    public async Task GetByEmail_WhenDbAccessThrowsArgumentNull_ReturnsDatabaseError()
    {
        var email = "duski@robloks.com";
        var dbException = new ArgumentNullException("db", "Simulierte Exception beim DB-Zugriff.");
        _mockContext.Setup(m => m.Users)
            .Throws(dbException);
        
        var result = await _repository.GetByEmail(email);
    
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error, Is.TypeOf<DatabaseError>());
        });
    }
     
    [Test]
    public async Task GetByUsername_WithValidUsername_ReturnsUser()
    {
        var user = _users.First();
        var username = user.Username;
        
        var result = await _repository.GetByUsername(username);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
        Assert.That(result.Value, Is.EqualTo(user));
    }
     
    [Test]
    public async Task GetByEmail_WithInvalidUsername_ReturnsEntityNotFoundError()
    {
        var username = "EuroPaLette";
        
        var result = await _repository.GetByUsername(username);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
        });
        var type = result.Error.GetType();
        Assert.That(type, Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
     
    [Test]
    public async Task GetByUsername_WhenDbAccessThrowsArgumentNull_ReturnsDatabaseError()
    {
        var username = "morde";
        var dbException = new ArgumentNullException("db", "Simulierte Exception beim DB-Zugriff.");
        _mockContext.Setup(m => m.Users)
            .Throws(dbException);
        
        var result = await _repository.GetByUsername(username);
    
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error, Is.TypeOf<DatabaseError>());
        });
    }
     
    [Test]
    public async Task Update_WithValidUser_ReturnsSuccess()
    {
        var user = _users.First();
        var newEmail = "jddk@rowdyandwho.com";
        user.Email = newEmail;
        _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var result = await _repository.Update(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.True);
    }
     
    [Test]
    public async Task Update_WithInvalidUser_ReturnsDatabaseError()
    {
        var user = new User()
        {
            Username = "annapohh",
            Password = "mexicanPower",
            Email = "annapohh@pauline.com",
            RegisteredAt = DateTime.UtcNow
        };
        
        var result = await _repository.Update(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Error.GetType(), Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
     
    [Test]
    public async Task Delete_WithValidUser_ReturnsSuccess()
    {
        var userResult = await _repository.GetAll();
        var user = userResult.Value!.First();
        _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var result = await _repository.Delete(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.True);
    }
     
    [Test]
    public async Task Delete_WithInvalidUser_ReturnsDatabaseError()
    {
        var user = new User()
        {
            Username = "Jagoebb",
            Password = "derHuebsche",
            Email = "czerwi@bioeng.com",
            RegisteredAt = DateTime.UtcNow
        };
        
        var result = await _repository.Delete(user);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Error.GetType(), Is.EqualTo(typeof(EntityNotFoundError<User>)));
    }
}