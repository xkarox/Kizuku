using System.Security.Claims;
using Backend.Services;
using Core;
using Core.Entities;
using Core.Errors.Authentication;
using Core.Errors.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Moq;

namespace BackendTest.Unit.Services;
[Parallelizable]
[TestFixture]
public class AuthenticationServiceTests
{
    private AuthenticationService _sut;
    private Mock<IUserRepository> _userService;
    
    [SetUp]
    public void Setup()
    {
        _userService = new Mock<IUserRepository>();
    }
    
    [Test]
    public void HashPassword_ShouldGenerateValidBCryptHash_WithWorkFactor12()
    {
        var password = "SuperSecureP@ssw0rd111!";
        _sut = new AuthenticationService(_userService.Object);
    
        var hash = _sut.HashPassword(password);
        var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
    
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void
        GetClaimsPrincipal_ShouldReturnClaimsPrincipal_WithNameEmailRoleClaim_AndAuthenticationScheme()
    {
        const string expectedUsername = "Username_239843";
        const string expectedEmail = "email@mail.com";
        const string expectedRole = "User";

        var user = new User()
        {
            Username = expectedUsername,
            Email = expectedEmail,
            Password = "IrrelevantHashedPassword",
            RegisteredAt = DateTimeOffset.UtcNow,
            UserId = Guid.NewGuid()
        };
        
        _sut = new AuthenticationService(_userService.Object);
        
        var returnedClaimsPrincipal = _sut.GetClaimsPrincipal(user);
        
        Assert.That(returnedClaimsPrincipal, Is.Not.Null);
        Assert.That(returnedClaimsPrincipal.Identity, Is.Not.Null);
        Assert.That(returnedClaimsPrincipal.Identity.IsAuthenticated, Is.True);
        
        Assert.That(returnedClaimsPrincipal.Identity.AuthenticationType, 
            Is.EqualTo(CookieAuthenticationDefaults.AuthenticationScheme));
        
        Assert.That(returnedClaimsPrincipal.Claims.Count(), 
            Is.EqualTo(3));
        var nameClaim = returnedClaimsPrincipal.FindFirst(ClaimTypes.Name);
        Assert.That(nameClaim, Is.Not.Null);
        Assert.That(nameClaim.Value, Is.EqualTo(expectedUsername));

        var emailClaim = returnedClaimsPrincipal.FindFirst(ClaimTypes.Email);
        Assert.That(emailClaim, Is.Not.Null);
        Assert.That(emailClaim.Value, Is.EqualTo(expectedEmail));

        var roleClaim = returnedClaimsPrincipal.FindFirst(ClaimTypes.Role);
        Assert.That(roleClaim, Is.Not.Null);
        Assert.That(roleClaim.Value, Is.EqualTo(expectedRole));

        var unexpectedClaims = returnedClaimsPrincipal.Claims
            .Where(c => c.Type 
                != ClaimTypes.Name && c.Type 
                != ClaimTypes.Email && c.Type 
                != ClaimTypes.Role)
            .ToList();
        Assert.That(unexpectedClaims, Is.Empty);
    }

    [Test]
    public async Task ValidateCredentials_WithValidCredentials_ShouldReturnSuccessfulResultWithUser()
    {
        const string email = "email@mail.com";
        const string password = "SuperSecureP@ssw0rd111!";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);
        var user = new User()
        {
            Username = "ivoxygen",
            Email = email,
            Password = hashedPassword,
            RegisteredAt = DateTimeOffset.UtcNow,
            UserId = Guid.NewGuid()
        };
        var successfulResult = Result<User>.Success(user);
        _userService.Setup(repository => repository.GetByEmail(email))
            .ReturnsAsync(successfulResult);
        
        
        _sut = new AuthenticationService(_userService.Object);
        
        var validationResult = await _sut.ValidateCredentials(email, password);
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsSuccess, Is.True);
            Assert.That(validationResult.Value, Is.Not.Null);
            var returnedUser = validationResult.Value;
            Assert.That(returnedUser!.Email, Is.EqualTo(email));
            Assert.That(returnedUser.Password, Is.EqualTo(hashedPassword));
        });
        _userService.Verify(repository => repository.GetByEmail(email), Times.Once);
    }
    
    [Test]
    public async Task ValidateCredentials_WithKnownEmailButInvalidPassword_ShouldReturnFailedResultWithPasswordValidationError()
    {
        const string email = "test@example.com";
        const string correctPassword = "CorrectPassword123!";
        const string incorrectPassword = "WrongPassword123!";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword, 12);

        var user = new User()
        {
            Username = "testuser",
            Email = email,
            Password = hashedPassword,
            RegisteredAt = DateTimeOffset.UtcNow,
            UserId = Guid.NewGuid()
        };
        var successfulResult = Result<User>.Success(user);
        _userService.Setup(repository => repository.GetByEmail(email))
            .ReturnsAsync(successfulResult);
        
        _sut = new AuthenticationService(_userService.Object);
        
        var validationResult = await _sut.ValidateCredentials(email, incorrectPassword);
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsError, Is.True);
            Assert.That(validationResult.Error, Is.Not.Null);
            var returnedError = validationResult.Error;
            Assert.That(returnedError, Is.TypeOf<PasswordValidationError>());
        });
        _userService.Verify(repository => repository.GetByEmail(email), Times.Once);
    }
    
    [Test]
    public async Task ValidateCredentials_WithUnknownEmail_ShouldReturnFailedResultWithEntityNotFoundError()
    {
        const string unknownEmail = "test@example.com";
        const string correctPassword = "CorrectPassword123!";
        const string incorrectPassword = "WrongPassword123!";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword, 12);

        var failedResult = Result<User>
            .Failure(new EntityNotFoundError<User>("email", unknownEmail));
        _userService.Setup(repository => repository.GetByEmail(unknownEmail))
            .ReturnsAsync(failedResult);
        
        _sut = new AuthenticationService(_userService.Object);
        
        var validationResult = await _sut.ValidateCredentials(unknownEmail, incorrectPassword);
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsError, Is.True);
            Assert.That(validationResult.Error, Is.Not.Null);
            var returnedError = validationResult.Error;
            Assert.That(returnedError, Is.TypeOf<EntityNotFoundError<User>>());
        });
        _userService.Verify(repository => repository.GetByEmail(unknownEmail), Times.Once);
    }
}