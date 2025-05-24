using SharpGDX.Desktop;

namespace Dungeon.Game.Core;

public class CloseRequestArgs
{
    public bool Cancel { get; set; } = false;
}


public class WindowEventManager
{


    public static event EventHandler<DesktopWindow>? WindowCreatedListener;
    
    public static event EventHandler<bool>? WindowIconifiedListener;

    public static event EventHandler<bool>? WindowMaximizedListener;

    public static event EventHandler<bool>? WindowFocusListener;

    public static event EventHandler<CloseRequestArgs>? CloseRequestedListener;

    public static event EventHandler<string[]>? FilesDroppedListener;

    public static event EventHandler? WindowRefreshListener;

    public static IDesktopWindowListener WindowListener { get; set; } = 
        CreateWindowListener();
    
    
    private static IDesktopWindowListener CreateWindowListener() =>
        new LocalWindowListener();


    private class LocalWindowListener : IDesktopWindowListener
    {
        
        public void Created(DesktopWindow window)
        {
            WindowCreatedListener?.Invoke(this, window);
        }

        public void FilesDropped(string[] files)
        {
            FilesDroppedListener?.Invoke(this, files);
        }

        public void FocusGained()
        {
            WindowFocusListener?.Invoke(this, false);
        }

        public void FocusLost()
        {
            WindowFocusListener?.Invoke(this, false);
        }

        public void Iconified(bool isIconified)
        {
            WindowIconifiedListener?.Invoke(this, isIconified);
        }

        public void Maximized(bool isMaximized)
        {
            WindowMaximizedListener?.Invoke(this, isMaximized);
        }

        public void RefreshRequested()
        {
            WindowRefreshListener?.Invoke(this, EventArgs.Empty);
        }
        
        /// <inheritdoc />
        public bool CloseRequested()
        {
            var args = new CloseRequestArgs();
            CloseRequestedListener?.Invoke(this, args);
            return args.Cancel;
        }

    }
}