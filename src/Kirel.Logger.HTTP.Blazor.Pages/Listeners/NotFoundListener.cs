namespace Kirel.Logger.HTTP.Blazor.Pages.Listeners;

/// <summary>
/// Listener class which allows notify when page was not found
/// </summary>
public class NotFoundListener
{
    /// <summary>
    /// Not found action
    /// </summary>
    public Action? OnNotFound { get;set; }

    /// <summary>
    /// Notify that page was not found
    /// </summary>
    public void NotifyNotFound()
    {
        OnNotFound?.Invoke();
    }
}