using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DirectXOverlay.Utilities
{
    public static class ExceptionHelper
    {
        public static void ThrowLastWin32Exception() => throw GetLastWin32Exception();

        public static Win32Exception GetLastWin32Exception()
        {
            return new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}