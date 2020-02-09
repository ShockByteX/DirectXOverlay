using System;

namespace DirectXOverlay.Utilities
{
    internal static class RandomHelper
    {
        public const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public static int GetInt32(int min, int max) => Rand.Next(min, max + 1);

        public static string GetString(int min, int max, bool isFirstLetter = false)
        {
            var chars = new char[GetInt32(min, max)];

            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = CharacterSet[Rand.Next(0, CharacterSet.Length)];
            }

            if (isFirstLetter)
            {
                chars[0] = CharacterSet[Rand.Next(0, 52)];
            }

            return new string(chars);
        }
    }
}