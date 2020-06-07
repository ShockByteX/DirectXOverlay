using System;
using System.Threading;
using DirectXOverlay.Extensions;
using DirectXOverlay.Native;
using DirectXOverlay.Utilities;

namespace DirectXOverlay.Windows
{
    public class Window : IDisposable
    {
        public const WindowStyles WindowStyle = WindowStyles.Popup | WindowStyles.Visible;
        public const WindowStylesEx WindowStyleEx = WindowStylesEx.ToolWindow | WindowStylesEx.TopMost | WindowStylesEx.NoActivate | WindowStylesEx.Transparent | WindowStylesEx.Layered;

        protected IntPtr _windowHandle;
        private IntPtr _parentWindowHandle;
        private ushort _regResult;

        private int _x, _y;
        private int _width, _height;

        private bool _visible;
        private bool _disposed;

        private Thread _windowThread;
        private readonly Thread _stickThread;

        public Window(int x, int y, int width, int height)
        {
            Initialize(x, y, width, height);
        }

        public Window(IntPtr parentWindowHandle, bool stick = false)
        {
            parentWindowHandle.Validate();
            _parentWindowHandle = parentWindowHandle;

            User32.GetWindowRect(_parentWindowHandle, out var rect);

            Initialize(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            if (stick)
            {
                _stickThread = ThreadHelper.RunGuarded(StickThreadFunc, (ex) => throw ex);
            }
        }
        ~Window() => Dispose();

        public IntPtr WindowHandle => _windowHandle;
        public IntPtr ParentWindowHandle => _parentWindowHandle;

        public int X
        {
            get => _x;
            set => SetBounds(value, _y, _width, _height);
        }
        public int Y
        {
            get => _y;
            set => SetBounds(_x, value, _width, _height);
        }
        public int Width
        {
            get => _width;
            set => SetBounds(_x, _y, value, _height);
        }
        public int Height
        {
            get => _height;
            set => SetBounds(_x, _y, _width, value);
        }

        public bool Visible
        {
            get => _visible; set
            {
                if (_visible == value) return;
                User32.ShowWindow(_windowHandle, value ? 5 : 0);
                ExtendFrameIntoClient();
                _visible = value;
            }
        }

        public virtual void SetBounds(int x, int y, int width, int height)
        {
            if (!ValidateBounds(x, y, width, height))
            {
                SetBoundsFields(x, y, width, height);
                User32.MoveWindow(_windowHandle, x, y, width, height, true);
                ExtendFrameIntoClient();
            }
        }
        public void ExtendFrameIntoClient() => WindowHelper.ExtendFrameIntoClient(_windowHandle, _x, _y, _width, _height);

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            var windowHandle = _windowHandle;
            _windowHandle = IntPtr.Zero;

            User32.SendMessage(windowHandle, WindowsMessage.WM_DESTROY, IntPtr.Zero, IntPtr.Zero);
            User32.DestroyWindow(windowHandle);
            User32.UnregisterClass(_regResult, IntPtr.Zero);

            ThreadHelper.SafeJoin(_stickThread, _windowThread);

            GC.SuppressFinalize(this);
        }

        private void SetBoundsFields(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        protected bool ValidateBounds(int x, int y, int width, int height)
        {
            return x == _x && y == _y && width == _width && height == _height;
        }

        protected virtual IntPtr WndProc(IntPtr hWnd, WindowsMessage message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                case WindowsMessage.WM_DWMCOMPOSITIONCHANGED:
                    ExtendFrameIntoClient();
                    return IntPtr.Zero;
                case WindowsMessage.WM_DESTROY:
                case WindowsMessage.WM_KEYDOWN:
                case WindowsMessage.WM_PAINT:
                case WindowsMessage.WM_DPI_CHANGED:
                    return IntPtr.Zero;
                case WindowsMessage.WM_ERASEBKGND:
                    User32.SendMessage(hWnd, WindowsMessage.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                    break;
            }

            return User32.DefWindowProc(hWnd, message, wParam, lParam);
        }

        private void WindowThreadFunc()
        {
            WindowHelper.CreateWindow(_x, _y, _width, _height, WndProc, out var hWnd, out var regResult);

            _windowHandle = hWnd;
            _regResult = regResult;

            while (_windowHandle != IntPtr.Zero)
            {
                User32.WaitMessage();

                if (!User32.PeekMessageW(out var msg, _windowHandle, 0, 0, 1))
                {
                    if (msg.message == WindowsMessage.WM_QUIT) continue;

                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);
                }
            }
        }

        private void StickThreadFunc()
        {
            while (_parentWindowHandle != IntPtr.Zero)
            {
                User32.GetWindowRect(_parentWindowHandle, out var rect);
                SetBounds(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

                Visible = User32.GetForegroundWindow() == _parentWindowHandle;

                Thread.Sleep(33);
            }
        }

        private void Initialize(int x, int y, int width, int height)
        {
            SetBoundsFields(x, y, width, height);
            _windowThread = ThreadHelper.RunGuarded(WindowThreadFunc, (ex) => throw ex);

            while (_windowHandle == IntPtr.Zero) Thread.Sleep(10);
        }
    }
}