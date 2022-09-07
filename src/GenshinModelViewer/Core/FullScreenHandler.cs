using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace GenshinModelViewer.Core
{
    public static class FullScreenHandler
    {
        private class FullScreenInfo
        {
            public double Left { get; set; }
            public double Top { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public WindowState WindowState { get; set; }
            public WindowStyle WindowStyle { get; set; }
            public ResizeMode ResizeMode { get; set; }

            public FullScreenInfo(Window window)
            {
                Left = window.Left;
                Top = window.Top;
                Width = window.Width;
                Height = window.Height;
                WindowState = window.WindowState;
                WindowStyle = window.WindowStyle;
                ResizeMode = window.ResizeMode;
            }
        }

        private static FullScreenInfo GetFullScreenInfo(DependencyObject obj)
            => (FullScreenInfo)obj.GetValue(FullScreenProperty);
        private static void SetFullScreenInfo(DependencyObject obj, FullScreenInfo value)
            => obj.SetValue(FullScreenProperty, value);
        private static readonly DependencyProperty FullScreenProperty
            = DependencyProperty.RegisterAttached("FullScreenInfo", typeof(FullScreenInfo), typeof(FullScreenHandler), new PropertyMetadata(null));

        public static void SetFullScreen(this Window window, bool? restore = null)
        {
            if (restore != null)
            {
                if (restore == true)
                {
                    window.RestoreFullScreen();
                }
                else
                {
                    window.MakeFullScreen();
                }
            }
            else
            {
                window.ReserveFullScreen();
            }
        }

        private static void ReserveFullScreen(this Window window)
        {
            if (GetFullScreenInfo(window) == null)
            {
                window.MakeFullScreen();
            }
            else
            {
                window.RestoreFullScreen();
            }
        }

        private static void MakeFullScreen(this Window window)
        {
            Screen screen = Screen.FromHandle(new WindowInteropHelper(window).Handle);
            FullScreenInfo info = new(window);

            SetFullScreenInfo(window, info);
            window.Left = screen.Bounds.Left;
            window.Top = screen.Bounds.Top;
            window.Width = screen.Bounds.Width;
            window.Height = screen.Bounds.Height;
            window.WindowState = WindowState.Normal;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
        }

        private static void RestoreFullScreen(this Window window)
        {
            FullScreenInfo info = GetFullScreenInfo(window);

            if (info == null) return;
            SetFullScreenInfo(window, null);
            window.Left = info.Left;
            window.Top = info.Top;
            window.Width = info.Width;
            window.Height = info.Height;
            window.WindowState = info.WindowState;
            window.WindowStyle = info.WindowStyle;
            window.ResizeMode = info.ResizeMode;
        }
    }
}
