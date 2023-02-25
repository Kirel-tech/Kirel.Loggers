namespace Kirel.Logger.HTTP.Blazor.Pages.Listeners;

public class NotFoundListener
{
    public Action? OnNotFound { get;set; }

    public void NotifyNotFound()
    {
        OnNotFound?.Invoke();
    }
}