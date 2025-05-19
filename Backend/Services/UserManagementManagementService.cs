using Backend.Infrastructure;
using Backend.Validators;
using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Core.Requests;
using Core.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserManagementManagementService(
    IKizukuContext db,
    IAuthenticationService authenticationService,
    IPasswordValidator passwordValidator
    ) : IUserManagementService
{
    public async Task<Result<User>> RegisterUser(RegistrationRequest request)
    {
        var registrationValidation = await ValidateRegistrationRequest(request);
        if (registrationValidation.IsError)
        {
            return Result<User>.Failure(registrationValidation.Error);
        }

        var user = request.ToUser(authenticationService.HashPassword);
        var registration = await Create(user);
        if (registration.IsError)
        {
            return Result<User>.Failure(registration.Error);
        }
        
        return Result<User>.Success(registration.Value!);
    }
    
    private async Task<Result> ValidateRegistrationRequest(RegistrationRequest request)
    {
        var findUserWithEmailResult = await GetByEmail(request.Email);
        if (findUserWithEmailResult.IsSuccess)
        {
            return Result.Failure(
                new EntityAlreadyExistsError<User>(nameof(request.Email), request.Email));
        }
        
        var findUserWithUsernameResult = await GetByUsername(request.Username);
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
    
    public async Task<Result<User>> Create(User entity)
    {
        if (entity == null)
            return Result<User>
                .Failure(new EntityNullError<User>());
        db.Users.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<User>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }

    private async Task<Result<User>> GetByEmail(string email)
    {
        try
        {
            var query = db.Users.Where(u => u.Email == email);
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Email), email));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }
    
    private async Task<Result<User>> GetByUsername(string username)
    {
        try
        {
            var query = db.Users
                .Where(u => u.Username == username);
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Username), username));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }
}