namespace Frontend.StateContainers;

public class AuthComponentStateContainer : StateContainer
{
    private bool _isProcessing = false;
    public bool IsProcessing
    {
        get => _isProcessing;
        set
        {
            _isProcessing = value;
            NotifyStateChanged();
        }
    }

    private bool _isLoginState = true;
    public bool IsLoginState
    {
        get => _isLoginState;
        set
        {
            _isLoginState = value;
            NotifyStateChanged();
        }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            NotifyStateChanged();
        }
    }
    
    private string _registrationErrorMessage = string.Empty;
    public string RegistrationErrorMessage
    {
        get => _registrationErrorMessage;
        set
        {
            _registrationErrorMessage = value;
            NotifyStateChanged();
        }
    }
    
    private string _username = string.Empty;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            NotifyStateChanged();
        }
    }
    
    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyStateChanged();
        }
    }
    
    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            NotifyStateChanged();
        }
    }
    
    private string _repeatPassword = string.Empty;
    public string RepeatPassword
    {
        get => _repeatPassword;
        set
        {
            _repeatPassword = value;
            NotifyStateChanged();
        }
    }
}
