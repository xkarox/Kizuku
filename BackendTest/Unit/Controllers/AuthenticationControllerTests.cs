using Backend.Controllers;
using Core;
using Core.Entities;
using Core.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using IAuthenticationService = Core.IAuthenticationService;

namespace BackendTest.Unit.Controllers;

[Parallelizable]
[TestFixture]
public class AuthenticationControllerTests
{
    Mock<IUserService> _mockUserService;
    Mock<IAuthenticationService> _mockAuthenticationService;
    Mock<HttpContext> _mockHttpContext;
    AuthController _sut;
    
    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        _mockHttpContext = new Mock<HttpContext>();
        
        var controllerContext = new ControllerContext
        {
            HttpContext = _mockHttpContext.Object,
            
        };
        _sut = new AuthController(_mockAuthenticationService.Object, _mockUserService.Object)
        {
            ControllerContext = controllerContext
        };
    }
    
    [Test]
    public async Task Register_ShouldCallUserServiceRegisterUser()
    {
        var password = "password";
        var registrationRequest = new RegistrationRequest
        {
            Username = "username",
            Email = "test@test.com",
            Password = password
        };
        var successfulRegistrationResult = 
            Result<User>.Success(registrationRequest.ToUser(BCrypt.Net.BCrypt.HashPassword(password, 12)));
        _mockUserService.Setup(m => m.RegisterUser(registrationRequest))
            .ReturnsAsync(successfulRegistrationResult);
        var controllerContext = new ControllerContext
        {
            HttpContext = _mockHttpContext.Object,
            
        };
        _sut = new AuthController(_mockAuthenticationService.Object, _mockUserService.Object)
        {
            ControllerContext = controllerContext
        };
        
        await _sut.Register(registrationRequest);
        
        _mockUserService.Verify(m => m.RegisterUser(registrationRequest), Times.Once);
    }
}