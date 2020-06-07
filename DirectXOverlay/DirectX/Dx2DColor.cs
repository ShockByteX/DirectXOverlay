using System;
using SharpDX.Mathematics.Interop;

namespace DirectXOverlay.DirectX
{
    public class Dx2DColor : IEquatable<Dx2DColor>
    {
        private const float MaxValue = 255f;

        public byte R, G, B, A;

        public static Dx2DColor Black => new Dx2DColor(0, 0, 0);
        public static Dx2DColor Gray => new Dx2DColor(127, 127, 127);
        public static Dx2DColor White => new Dx2DColor(255, 255, 255);
        public static Dx2DColor Red => new Dx2DColor(255, 0, 0);
        public static Dx2DColor Green => new Dx2DColor(0, 255, 0);
        public static Dx2DColor Blue => new Dx2DColor(0, 0, 255);
        public static Dx2DColor Yellow => new Dx2DColor(255, 255, 0);
        public static Dx2DColor Cyan => new Dx2DColor(0, 255, 255);
        public static Dx2DColor Purple => new Dx2DColor(255, 0, 255);

        public Dx2DColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public override int GetHashCode() => R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode() ^ A.GetHashCode();
        public override bool Equals(object obj) => (obj is Dx2DColor color) && Equals(color);

        public bool Equals(Dx2DColor other) => R == other.R && G == other.G && B == other.B && A == other.A;

        public static bool operator ==(Dx2DColor left, Dx2DColor right) => left.Equals(right);
        public static bool operator !=(Dx2DColor left, Dx2DColor right) => !left.Equals(right);

        public static implicit operator Dx2DColor(RawColor4 color)
        {
            return new Dx2DColor((byte)(color.R * MaxValue), (byte)(color.G * MaxValue), (byte)(color.B * MaxValue), (byte)(color.A * MaxValue));
        }

        public static implicit operator RawColor4(Dx2DColor color)
        {
            return new RawColor4(color.R / MaxValue, color.G / MaxValue, color.B / MaxValue, color.A / MaxValue);
        }
    }
}