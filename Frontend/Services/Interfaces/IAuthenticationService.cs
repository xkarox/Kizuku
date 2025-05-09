using Core.Requests;
using Core.Responses;

namespace Frontend.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse?> Login(LoginRequest loginRequest);
    public Task Logout();
    public Task<RegistrationResponse?> Register(RegistrationRequest registrationRequest);
}