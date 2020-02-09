using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using DirectXOverlay.Native;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace DirectXOverlay.DirectX
{
    public class Dx2DRenderer : IDisposable
    {
        public readonly Dx2DGraphics Graphics;
        public readonly Factory Factory;
        public readonly WindowRenderTarget Device;

        private Size2 _size, _resize;
        private bool _rendering;
        private bool _disposed;

        public Dx2DRenderer(IntPtr hWnd, bool vSync)
        {
            User32.GetWindowRect(hWnd, out Rectangle bounds);
            _size = new Size2(bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
            var rtProps = new RenderTargetProperties(RenderTargetType.Hardware, new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied), 96.0f, 96.0f, RenderTargetUsage.None, FeatureLevel.Level_DEFAULT);
            var hrtProps = new HwndRenderTargetProperties
            {
                Hwnd = hWnd,
                PixelSize = _size,
                PresentOptions = vSync ? PresentOptions.None : PresentOptions.Immediately
            };

            Factory = new Factory();
            Device = new WindowRenderTarget(Factory, rtProps, hrtProps)
            {
                TextAntialiasMode = TextAntialiasMode.Aliased,
                AntialiasMode = AntialiasMode.Aliased
            };

            Graphics = new Dx2DGraphics(Factory, Device);
        }
        ~Dx2DRenderer() => Dispose();

        public int Width => _size.Width;
        public int Height => _size.Height;
        public bool Rendering => _rendering;

        public void Resize(int width, int height)
        {
            var size = new Size2(width, height);

            if (_size == size) return;

            if (_rendering)
            {
                _resize = size;
                return;
            }

            Device.Resize(size);
        }

        public void BeginScene()
        {
            if (_rendering) return;

            _rendering = true;

            if (_resize != Size2.Zero)
            {
                _size = _resize;
                _resize = Size2.Zero;
                Device.Resize(_size);
            }

            Device.BeginDraw();
        }

        public void EndScene()
        {
            try
            {
                Device.EndDraw();
            }
            catch (Exception ex) when ((uint)ex.HResult == 0x8899000C) { } // D2DERR_RECREATE_TARGET

            _rendering = false;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Graphics.Dispose();

            if (!Device.IsDisposed) Device.Dispose();
            if (!Factory.IsDisposed) Factory.Dispose();
        }
    }
}