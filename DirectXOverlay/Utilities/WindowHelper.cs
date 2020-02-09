using System;
using System.Runtime.InteropServices;
using DirectXOverlay.Extensions;
using DirectXOverlay.Native;

namespace DirectXOverlay.Utilities
{
    public static class WindowHelper
    {
        private const WindowStyles WindowStyle = WindowStyles.Popup | WindowStyles.Visible;
        private const WindowStylesEx WindowStyleEx = WindowStylesEx.ToolWindow | WindowStylesEx.TopMost | WindowStylesEx.NoActivate | WindowStylesEx.Transparent | WindowStylesEx.Layered;

        public static void CreateWindow(int x, int y, int width, int height, WndProc wndProc, out IntPtr hWnd, out ushort regResult)
        {
            regResult = CreateWindowClass(wndProc);
            hWnd = CreateWindow(regResult, x, y, width, height, WindowStyle, WindowStyleEx);

            User32.SetLayeredWindowAttributes(hWnd, 0, 255, LayeredWindowAttributes.Alpha);
            User32.UpdateWindow(hWnd);

            ExtendFrameIntoClient(hWnd, x, y, width, height);
        }

        public static void ExtendFrameIntoClient(IntPtr hWnd, int x, int y, int width, int height)
        {
            var margins = new Margins(x, y, width, height);
            Dwmapi.DwmExtendFrameIntoClientArea(hWnd, ref margins);
        }

        private static IntPtr CreateWindow(ushort regResult, int x, int y, int width, int height, WindowStyles ws, WindowStylesEx wsEx)
        {
            var hWnd = User32.CreateWindowEx(wsEx, regResult, string.Empty, ws, x, y, width, height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            hWnd.Validate(true);

            return hWnd;
        }
        private static ushort CreateWindowClass(WndProc wndProc, string menuName, string className)
        {
            var wnd = new WindowClassEx()
            {
                cbSize = WindowClassEx.GetSize(),
                style = 0,
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProc),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = IntPtr.Zero,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = IntPtr.Zero,
                lpszMenuName = menuName,
                lpszClassName = className,
                hIconSm = IntPtr.Zero
            };

            var regResult = User32.RegisterClassEx(ref wnd);

            if (regResult == 0) ExceptionHelper.ThrowLastWin32Exception();

            return regResult;
        }

        private static ushort CreateWindowClass(WndProc wndProc)
        {
            var menuName = RandomHelper.GetString(5, 10, true);
            var className = RandomHelper.GetString(5, 10, true);

            return CreateWindowClass(wndProc, menuName, className);
        }
    }
}