using Backend.Infrastructure;
using Backend.Services;
using Backend.Validators;
using Core;
using Core.Entities;
using Core.Errors.Entities;
using Core.Requests;
using Core.Validators;
using Moq;

namespace BackendTest.Unit.Services;

[Parallelizable]
[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IAuthenticationService> _mockAuthenticationService;
    private Mock<IPasswordValidator> _mockPasswordValidator;
    private UserService _sut;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        _mockPasswordValidator = new Mock<IPasswordValidator>();
    }

    [Test]
    public async Task RegisterUser_WithValidRegistrationRequest_ShouldSucceed()
    {
        var registrationRequest = new RegistrationRequest
        {
            Email = "test@test.com",
            Password = "password_lol123",
            Username = "pikachu22",
        };
        var hashedPassword =
            "67d6f975e64c0085fd8a8dda6921f0613b7a80d1959d0986fd9432f5dabd78be";
        var failedResult = Result<User>
            .Failure(new EntityNotFoundError<User>(nameof(User.Email), nameof(User.Email)));
        
        _mockUserRepository.Setup(repository =>
            repository.GetByEmail(registrationRequest.Email)).ReturnsAsync(failedResult);
        _mockUserRepository.Setup(repository => 
            repository.GetByUsername(registrationRequest.Username)).ReturnsAsync(failedResult);
        _mockUserRepository.Setup(repository =>
                repository.Create(It.IsAny<User>()))
            .ReturnsAsync((User userBeingCreated) => Result<User>.Success(userBeingCreated));
        
        _mockAuthenticationService.Setup(service => service.HashPassword(It.IsAny<string>()))
            .Returns(hashedPassword);
        
        _mockPasswordValidator.Setup(validator => 
            validator.Validate(It.IsAny<string>())).Returns(Result.Success);
        _sut = new UserService(_mockUserRepository.Object, 
            _mockAuthenticationService.Object,
            _mockPasswordValidator.Object);
        
        var registrationResult = await _sut.RegisterUser(registrationRequest);
        
        
        Assert.Multiple(() =>
        {
            Assert.That(registrationResult.IsSuccess, Is.True);
            Assert.That(registrationResult.Value, Is.Not.Null);
            Assert.That(registrationResult.Value!.Email, Is.EqualTo(registrationRequest.Email));
            Assert.That(registrationResult.Value!.Username, Is.EqualTo(registrationRequest.Username));
            Assert.That(registrationResult.Value!.Password, Is.EqualTo(hashedPassword));
        });
        _mockUserRepository.Verify(repo => repo.Create(It.Is<User>(u =>
            u.Email == registrationRequest.Email &&
            u.Username == registrationRequest.Username &&
            u.Password == hashedPassword
        )), Times.Once);
        _mockAuthenticationService.Verify(s => s.HashPassword(registrationRequest.Password), Times.Once);
        _mockPasswordValidator.Verify(v => v.Validate(registrationRequest.Password), Times.Once);
        _mockUserRepository.Verify(r => r.GetByEmail(registrationRequest.Email), Times.Once);
        _mockUserRepository.Verify(r => r.GetByUsername(registrationRequest.Username), Times.Once);
    }
    
    [Test]
    public void RegisterUser_WithEmailAlreadyExists_ShouldFail()
    {
    }
    
    [Test]
    public void RegisterUser_WithUsernameAlreadyExists_ShouldFail()
    {
    }
    
    [Test]
    public void RegisterUser_WithPasswordInvalid_ShouldFail()
    {
    }
}