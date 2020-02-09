using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DirectXOverlay.DirectX;
using DirectXOverlay.Utilities;

namespace DirectXOverlay.Windows
{
    public class OverlayWindow : Window, IDisposable
    {
        public delegate void Draw(OverlayWindow window, Dx2DGraphics graphics);

        private readonly Dx2DRenderer _renderer;
        private readonly Thread _drawThread;

        private int _sleep = 16;
        private int _fps;
        private bool _active;
        private bool _disposed;

        public OverlayWindow(int x, int y, int width, int height, bool vSync = false) : base(x, y, width, height)
        {
            _renderer = new Dx2DRenderer(_windowHandle, vSync);
            _drawThread = ThreadHelper.RunGuarded(DrawThreadFunc, (ex) => throw ex);
            while (!_active) Thread.Sleep(10);
        }
        public OverlayWindow(IntPtr parentWindowHandle, bool vSync = false) : base(parentWindowHandle, true)
        {
            _renderer = new Dx2DRenderer(_windowHandle, vSync);
            _drawThread = ThreadHelper.RunGuarded(DrawThreadFunc, (ex) => throw ex, false, ThreadPriority.Highest);
            while (!_active) Thread.Sleep(10);
        }

        public ushort FramesPerSecond
        {
            get => (ushort)(1000 / _sleep);
            set => _sleep = 1000 / value;
        }

        public int CountedFramesPerSecond => _fps;

        public Dx2DBitmap CreateBitmap(Stream stream) => new Dx2DBitmap(_renderer.Device, stream);

        public override void SetBounds(int x, int y, int width, int height)
        {
            if (!ValidateBounds(x, y, width, height))
            {
                _renderer?.Resize(width, height);
                base.SetBounds(x, y, width, height);
            }
        }

        public new void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_active)
            {
                _active = false;
                ThreadHelper.SafeJoin(_drawThread);
            }

            _renderer.Dispose();

            base.Dispose();
        }

        private void DrawThreadFunc()
        {
            _active = true;

            var frames = 0;
            var fpsWatch = Stopwatch.StartNew();

            void Draw()
            {
                _renderer.BeginScene();
                OnDraw(this, _renderer.Graphics);
                _renderer.EndScene();

                if (fpsWatch.ElapsedMilliseconds >= 1000)
                {
                    _fps = frames;
                    frames = 0;
                    fpsWatch.Restart();
                }

                frames++;
            }

            while (_active)
            {
                if (OnDraw != null)
                {
                    RunHelper.ConsistentRun(Draw, _sleep);
                }
                else Thread.Sleep(10);
            }
        }

        public event Draw OnDraw;
    }
}