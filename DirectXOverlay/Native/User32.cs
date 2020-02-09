using System;
using System.Runtime.InteropServices;

namespace DirectXOverlay.Native
{
    public delegate IntPtr WndProc(IntPtr hWnd, WindowsMessage msg, IntPtr wParam, IntPtr lParam);
    public static class User32
    {
        public const string LibraryName = "user32.dll";

        [DllImport(LibraryName)]
        public static extern bool WaitMessage();

        [DllImport(LibraryName)]
        public static extern bool PeekMessageW(out Message lpMessage, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport(LibraryName)]
        public static extern bool TranslateMessage([In] ref Message lpMessage);

        [DllImport(LibraryName)]
        public static extern IntPtr DispatchMessage([In] ref Message lpMessage);

        [DllImport(LibraryName)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessage message, IntPtr wParam, IntPtr lParam);

        [DllImport(LibraryName)]
        public static extern IntPtr SetThreadDpiAwarenessContext(ref int dpiContext);

        [DllImport(LibraryName)]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern ushort RegisterClassEx([In] ref WindowClassEx lpWindowClassEx);

        [DllImport(LibraryName)]
        public static extern bool UnregisterClass(uint regResult, IntPtr hInstance);

        [DllImport(LibraryName)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [DllImport(LibraryName)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport(LibraryName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport(LibraryName, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx(WindowStylesEx dwExStyle, uint regResult, [MarshalAs(UnmanagedType.LPStr)]  string lpWindowName, WindowStyles dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport(LibraryName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport(LibraryName)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, WindowsMessage uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(LibraryName)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRectangle);

        [DllImport(LibraryName)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, LayeredWindowAttributes dwFlags);

        [DllImport(LibraryName)]
        public static extern IntPtr GetForegroundWindow();
    }
}