using System.Net.Mail;
using Blazing.Mvvm.ComponentModel;
using Core;
using Core.Requests;
using Frontend.Services.Interfaces;
using Frontend.StateContainers;
using Microsoft.AspNetCore.Components;

namespace Frontend.ViewModels;

public class LoginViewModel
    (IFrontendAuthenticationService authService,
        NavigationManager navigationManager)
    : ViewModelBase
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    
    private async Task HandleLogin(AuthComponentStateContainer state)
    {
        state.SetProcessing(true);
        state.SetErrorMessage(string.Empty);
        
        if (!IsValidEmail(_email))
        {
            state.SetProcessing(false);
            state.SetErrorMessage("Please enter a valid email address.");
            return;
        }

        if (string.IsNullOrEmpty(_password))
        {
            state.SetProcessing(false);
            state.SetErrorMessage("Please enter a password.");
            return;
        }
        
        var loginRequest = new LoginRequest { Email = _email, Password = _password };
        var loginResponse = await authService.Login(loginRequest);
        
        if (loginResponse.IsSuccess)
        {
            navigationManager.NavigateTo("/");
        }
        else
        {
            state.SetErrorMessage("Login failed. Please check your credentials.");
        }
        state.SetProcessing(false);
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
}