using Backend.Infrastructure;
using Backend.Infrastructure.Repositories;
using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BackendTest.Unit.Infrastructure.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    private Mock<IKizukuContext> _mockContext;
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
    public async Task Create_WithValidUserButInvalidDbContext_ReturnsDatabaseError()
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
}