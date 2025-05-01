using Backend.Validators;
using Core;
using Core.Entities;
using Core.Errors.Entities;
using Core.Requests;

namespace Backend.Services;

public class UserService(
    IUserRepository userRepository,
    IAuthenticationService authenticationService
    ) : IUserService
{
    public async Task<Result<User>> RegisterUser(RegistrationRequest request)
    {
        var registrationValidation = await ValidateRegistrationRequest(request);
        if (registrationValidation.IsError)
        {
            return Result<User>.Failure(registrationValidation.Error);
        }

        var user = request.ToUser(authenticationService.HashPassword);
        var registration = await userRepository.Create(user);
        if (registration.IsError)
        {
            return Result<User>.Failure(registration.Error);
        }
        
        return Result<User>.Success(registration.Value!);
    }
    
    private async Task<Result> ValidateRegistrationRequest(RegistrationRequest request)
    {
        var findUserWithEmailResult = await userRepository.GetUserByEmail(request.Email);
        if (findUserWithEmailResult.IsSuccess)
        {
            return Result.Failure(
                new EntityAlreadyExistsError<User>(nameof(request.Email), request.Email));
        }
        
        var findUserWithUsernameResult = await userRepository.GetUserByUsername(request.Username);
        if (findUserWithUsernameResult.IsSuccess)
        {
            return Result.Failure(
                new EntityAlreadyExistsError<User>(nameof(request.Username), request.Username));
        }
        
        var passwordValidationResult = PasswordValidator.Validate(request.Password);
        if (passwordValidationResult.IsError)
        {
            return Result.Failure(passwordValidationResult.Error);
        }
        
        return Result.Success();
    }
}