
using Backend.Infrastructure;
using Core.Entities;
using Core.Requests;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        var passwordHash = BC.HashPassword(request.Password, 12);
        var user = request.ToUser(passwordHash);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}