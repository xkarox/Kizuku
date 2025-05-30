namespace Frontend.StateContainers;

public abstract class StateContainer
{
    public event Action OnChange;
    protected void NotifyStateChanged() => OnChange?.Invoke();
}