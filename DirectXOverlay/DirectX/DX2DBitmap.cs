using System.IO;
using SharpDX.Direct2D1;
using SharpDX.WIC;

namespace DirectXOverlay.DirectX
{
    public class Dx2DBitmap
    {
        private static readonly ImagingFactory ImagingFactory = new ImagingFactory();
        private static readonly FormatConverter FormatConverter = new FormatConverter(ImagingFactory);

        private readonly SharpDX.Direct2D1.Bitmap _bmp;
        private bool _disposed;

        internal Dx2DBitmap(RenderTarget device, Stream stream)
        {
            using (stream)
            {
                using (var decoder = new BitmapDecoder(ImagingFactory, stream, DecodeOptions.CacheOnDemand))
                {
                    using (var frameDecode = decoder.GetFrame(0))
                    {
                        try
                        {
                            FormatConverter.Initialize(frameDecode, SharpDX.WIC.PixelFormat.Format32bppRGBA1010102);

                        }
                        catch
                        {
                            FormatConverter.Initialize(frameDecode, SharpDX.WIC.PixelFormat.Format32bppRGB);
                        }

                        _bmp = SharpDX.Direct2D1.Bitmap.FromWicBitmap(device, FormatConverter);
                    }
                }
            }
        }
        ~Dx2DBitmap() => Dispose();

        public int Width => _bmp.PixelSize.Width;
        public int Height => _bmp.PixelSize.Height;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _bmp.Dispose();
        }

        public static implicit operator SharpDX.Direct2D1.Bitmap(Dx2DBitmap bmp) => bmp._bmp;
    }
}