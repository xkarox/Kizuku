namespace Frontend.StateContainers;

public class AuthComponentStateContainer : AuthStateContainer
{
    public bool IsProcessing { get; private set; } = false;
    public bool IsLoginState { get; private set; } = true;
    public string ErrorMessage { get; private set; } = string.Empty;

    public void SetProcessing(bool isProcessing)
    {
        IsProcessing = isProcessing;
        NotifyStateChanged();
    }

    public void SetLoginState(bool isLoginState)
    {
        IsLoginState = isLoginState;
        NotifyStateChanged();
    }

    public void SetErrorMessage(string message)
    {
        ErrorMessage = message;
        NotifyStateChanged();
    }
}
