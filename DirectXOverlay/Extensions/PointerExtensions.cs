using System;
using DirectXOverlay.Utilities;

namespace DirectXOverlay.Extensions
{
    public static class PointerExtensions
    {
        public static void Validate(this IntPtr handle, bool win32Error = false)
        {
            if (handle == IntPtr.Zero)
            {
                if (win32Error) ExceptionHelper.ThrowLastWin32Exception();

                throw new InvalidOperationException($"Invalid handle '{nameof(handle)}'");
            }
        }
    }
}