namespace Kirel.Blazor.HttpLogger.Listeners;

public class NotFoundListener
{
    public Action? OnNotFound { get;set; }

    public void NotifyNotFound()
    {
        OnNotFound?.Invoke();
    }
}