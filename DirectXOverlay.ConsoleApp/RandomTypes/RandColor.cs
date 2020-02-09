using DirectXOverlay.DirectX;

namespace DirectXOverlay.ConsoleApp.RandomTypes
{
    public class RandColor
    {
        public RandColor(RandByte red, RandByte green, RandByte blue, RandByte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public RandColor(RandByte colorChannel, RandByte alpha = null) : this(colorChannel, colorChannel, colorChannel, alpha) { }

        public RandByte R { get; }
        public RandByte G { get; }
        public RandByte B { get; }
        public RandByte A { get; }
        public bool EnableAlpha => A != null;

        public Dx2DColor Value => new Dx2DColor(R.Value, G.Value, B.Value, EnableAlpha ? A.Value : byte.MaxValue);
    }
}