using Backend.Validators;
using Core;
using Core.Entities;
using Core.Errors.Entities;
using Core.Requests;
using Core.Validators;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services;

public class UserService(
    IUserRepository userRepository,
    IAuthenticationService authenticationService,
    IPasswordValidator passwordValidator
    ) : IUserService
{
    /// <summary>
    /// Registers a new user based on the provided registration request.
    /// </summary>
    /// <param name="request">The registration details for the new user.</param>
    /// <returns>A result containing the created user on success, or an error if registration fails.</returns>
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
    
    /// <summary>
    /// Validates a user registration request by checking for existing email or username and ensuring the password meets validation criteria.
    /// </summary>
    /// <param name="request">The registration request to validate.</param>
    /// <returns>A result indicating success if the request is valid, or failure with the corresponding error.</returns>
    private async Task<Result> ValidateRegistrationRequest(RegistrationRequest request)
    {
        var findUserWithEmailResult = await userRepository.GetByEmail(request.Email);
        if (findUserWithEmailResult.IsSuccess)
        {
            return Result.Failure(
                new EntityAlreadyExistsError<User>(nameof(request.Email), request.Email));
        }
        
        var findUserWithUsernameResult = await userRepository.GetByUsername(request.Username);
        if (findUserWithUsernameResult.IsSuccess)
        {
            return Result.Failure(
                new EntityAlreadyExistsError<User>(nameof(request.Username), request.Username));
        }
        
        var passwordValidationResult = passwordValidator.Validate(request.Password);
        if (passwordValidationResult.IsError)
        {
            return Result.Failure(passwordValidationResult.Error);
        }
        
        return Result.Success();
    }
}