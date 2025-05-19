using System.Text.Json;
using Core;
using Core.Requests;
using Core.Responses;

namespace Frontend.Services.Interfaces;

public interface IAuthenticationServiceUI
{
    public Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
    public Task Logout();
    public Task<Result<RegistrationResponse>> Register(RegistrationRequest registrationRequest);
}