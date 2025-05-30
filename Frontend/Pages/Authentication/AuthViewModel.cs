using System.Net.Mail;
using Blazing.Mvvm.ComponentModel;
using Core;
using Core.Requests;
using Frontend.Services.Interfaces;
using Frontend.StateContainers;
using Microsoft.AspNetCore.Components;

namespace Frontend.ViewModels;

public class AuthViewModel
    (IAuthenticationServiceUI authServiceUi,
     AuthComponentStateContainer state,
     NavigationManager navigationManager)
    : ViewModelBase
{
    public void HandleBackButtonClicked()
    {
        navigationManager.NavigateTo("/");
    }
    public async Task HandleLogin()
    {
        state.IsProcessing = true;
        state.ErrorMessage = string.Empty;
        
        if (!IsValidEmail(state.Email))
        {
            state.IsProcessing = false;
            state.ErrorMessage = "Please enter a valid email address.";
            return;
        }

        if (string.IsNullOrEmpty(state.Password))
        {
            state.IsProcessing = false;
            state.ErrorMessage = "Please enter a password.";
            return;
        }
        
        var loginRequest = new LoginRequest
        {
            Email = state.Email, Password = state.Password
        };
        var loginResponse = 
            await authServiceUi.Login(loginRequest);
        
        if (loginResponse.IsSuccess)
        {
            navigationManager.NavigateTo("/");
        }
        else
        {
            state.ErrorMessage = 
                "Login failed. Please check your credentials.";
        }
        state.IsProcessing = false;
    }
    
    public async Task HandleRegistration()
    {
        state.IsProcessing = true;
        state.ErrorMessage = string.Empty;
        state.RegistrationErrorMessage = string.Empty;
        
        if (!IsUsernameValid(state.Username))
        {
            state.ErrorMessage = "Username must be at least 3 characters long.";
            state.IsProcessing = false;
            return;
        }
        
        if (!IsValidEmail(state.Email))
        {
            state.ErrorMessage = "Please enter a valid email address.";
            state.IsProcessing = false;
            return;
        }
        
        if (!PasswordsMatch(state.Password, state.RepeatPassword))
        {
            state.RegistrationErrorMessage = "Passwords do not match.";
            state.IsProcessing = false;
            return;
        }
        
        if (!IsPasswordValid(state.Password))
        {
            state.RegistrationErrorMessage = "Password does not meet requirements.";
            state.IsProcessing = false;
            return;
        }

        var registrationRequest = new RegistrationRequest
        {
            Username = state.Username, Email = state.Email, Password = state.Password
        };
        var registrationResponse = 
            await authServiceUi.Register(registrationRequest);

        if (registrationResponse.IsSuccess)
        {
            SetLoginState();
            state.Email = registrationRequest.Email; 
            state.Password = string.Empty;
        }
        else
        {
            state.ErrorMessage = "Registration failed. Please try again.";
        }
        state.IsProcessing = false;
    }
    
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        try
        {
            var addr = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private bool PasswordsMatch(string first, string second)
    {
        if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(second))
            return false;
        return first.Equals(second);
    }

    private bool IsPasswordValid(string password)
    {
        var hasMinLength = password.Length >= 8;
        var hasUpperCase = password.Any(char.IsUpper);
        var hasLowerCase = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

        return hasMinLength && hasUpperCase && hasLowerCase && hasDigit &&
               hasSpecialChar;
    }
    
    private bool IsUsernameValid(string username)
    {
        return !(string.IsNullOrWhiteSpace(username) || username.Length < 3);
    }
    
    public void SetLoginState()
    {
        state.IsLoginState = true;
        state.ErrorMessage = string.Empty;
        state.RegistrationErrorMessage = string.Empty;
    }
    
    public void SetRegistrationState()
    {
        state.IsLoginState = false;
        state.ErrorMessage = string.Empty;
        state.RegistrationErrorMessage = string.Empty;
    }
}