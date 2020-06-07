using System;
using SharpDX.DirectWrite;

namespace DirectXOverlay.DirectX
{
    public class Dx2DFont : IDisposable
    {
        private static readonly Factory Factory = new Factory();

        private readonly TextFormat _tf;
        private bool _disposed;

        public Dx2DFont(string name, float size, Dx2DFontWeight weight, Dx2DFontStyle style)
        {
            _tf = new TextFormat(Factory, name, (FontWeight)weight, (FontStyle)style, size);
        }

        ~Dx2DFont() => Dispose();

        public string FamilyName => _tf.FontFamilyName;
        public float Size => _tf.FontSize;
        public Dx2DFontWeight Weight => (Dx2DFontWeight)_tf.FontWeight;
        public Dx2DFontStyle Style => (Dx2DFontStyle)_tf.FontStretch;

        public TextLayout GetLayout(string text) => new TextLayout(Factory, text, _tf, float.PositiveInfinity, float.PositiveInfinity);
        public float GetWidth(string text) => GetLayout(text).Metrics.Width;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _tf.Dispose();
        }

        public static implicit operator TextFormat(Dx2DFont font) => font._tf;
    }
}