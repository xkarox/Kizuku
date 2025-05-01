
using System.Security.Claims;
using Backend.Infrastructure;
using Core;
using Core.Entities;
using Core.Errors;
using Core.Requests;
using Core.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;
using IAuthenticationService = Core.IAuthenticationService;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IAuthenticationService authenticationService,
    IUserService userService
    ) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IError))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var credentialsValidation = await authenticationService
            .ValidateCredentials(request.Email, request.Password);
        if (credentialsValidation.IsError)
        {
            return Unauthorized(credentialsValidation.Error);
        }
        
        var user = credentialsValidation.Value;
        var claimsPrincipal = authenticationService.GetClaimsPrincipal(user!);
        
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IsPersistent = true,
        };
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);
        
        return Ok(user!.ToLoginResponse());
    }
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        var registration = await userService.RegisterUser(request);
        if (registration.IsError)
        {
            return BadRequest(registration.Error);
        }
        
        return Ok(registration.Value!.ToRegistrationResponse());
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}