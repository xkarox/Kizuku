namespace Frontend.StateContainers;

public abstract class AuthStateContainer
{
    public event Action OnChange;
    protected void NotifyStateChanged() => OnChange?.Invoke();
}