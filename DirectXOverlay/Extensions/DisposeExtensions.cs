using System;

namespace DirectXOverlay.Extensions
{
    public static class DisposeExtensions
    {
        public static void SafeDispose<T>(this T obj) where T : IDisposable
        {
            var tempObj = obj;
            obj = default;
            tempObj.Dispose();
        }
    }
}