using System;

namespace DirectXOverlay.Utilities
{
    public static class ValidateHelper
    {
        public static void ValidateNull<T>(T obj, string paramName) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}