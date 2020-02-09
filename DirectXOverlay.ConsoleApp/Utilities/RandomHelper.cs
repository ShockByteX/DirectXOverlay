using System;

namespace DirectXOverlay.ConsoleApp.Utilities
{
    public static class RandomHelper
    {
        public static readonly Random Rand = new Random();

        public static byte GetByte(byte min, byte max) => (byte)GetInt32(min, max);
        public static int GetInt32(int min, int max) => Rand.Next(min, max + 1);
    }
}