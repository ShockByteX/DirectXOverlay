using System;
using SharpDX.Mathematics.Interop;

namespace DirectXOverlay.DirectX
{
    public class Dx2DColor : IEquatable<Dx2DColor>
    {
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
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals(obj as Dx2DColor);
        }

        public bool Equals(Dx2DColor other) => R == other.R && G == other.G && B == other.B && A == other.A;

        public static bool operator ==(Dx2DColor left, Dx2DColor right) => left.Equals(right);
        public static bool operator !=(Dx2DColor left, Dx2DColor right) => !left.Equals(right);

        public static implicit operator Dx2DColor(RawColor4 color) => new Dx2DColor((byte)(color.R * 255f), (byte)(color.G * 255f), (byte)(color.B * 255f), (byte)(color.A * 255f));
        public static implicit operator RawColor4(Dx2DColor color) => new RawColor4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }
}