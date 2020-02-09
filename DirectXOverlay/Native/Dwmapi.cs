using System;
using System.Runtime.InteropServices;

namespace DirectXOverlay.Native
{
    public static class Dwmapi
    {
        public const string LibraryName = "dwmapi";

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);
    }
}