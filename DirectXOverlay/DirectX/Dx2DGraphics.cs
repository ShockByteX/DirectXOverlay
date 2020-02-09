using System;
using System.Runtime.CompilerServices;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DirectXOverlay.DirectX
{
    public class Dx2DGraphics : IDisposable
    {
        private readonly WindowRenderTarget _device;
        private readonly SolidColorBrush _brush;
        private readonly Factory _factory;
        private bool _disposed;

        public Dx2DGraphics(Factory factory, WindowRenderTarget device)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _brush = new SolidColorBrush(_device, new RawColor4(0, 0, 0, 255));
        }
        ~Dx2DGraphics() => Dispose();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearScene() => _device.Clear(null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearScene(Dx2DColor color) => _device.Clear(color);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLine(float startX, float startY, float endX, float endY, float stroke, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawLine(new RawVector2(startX, startY), new RawVector2(endX, endY), _brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRectangle(float x, float y, float width, float height, float stroke, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), _brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, float stroke, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawRoundedRectangle(new RoundedRectangle() { Rect = new RawRectangleF(x, y, x + width, y + height), RadiusX = radiusX, RadiusY = radiusY }, _brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawCircle(float x, float y, float radius, float stroke, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawEllipse(new Ellipse(new RawVector2(x, y), radius, radius), _brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawEllipse(float x, float y, float width, float height, float stroke, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawEllipse(new Ellipse(new RawVector2(x, y), width, height), _brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTriangle(float x_x, float x_y, float y_x, float y_y, float z_x, float z_y, float stroke, Dx2DColor color)
        {
            _brush.Color = color;

            using (var path = new PathGeometry(_factory))
            {
                using (var sink = path.Open())
                {
                    sink.BeginFigure(new RawVector2(x_x, x_y), FigureBegin.Hollow);
                    sink.AddLine(new RawVector2(y_x, y_y));
                    sink.AddLine(new RawVector2(z_x, z_y));
                    sink.EndFigure(FigureEnd.Closed);
                    sink.Close();
                    _device.DrawGeometry(path, _brush, stroke);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(string text, float x, float y, Dx2DFont font, Dx2DColor color)
        {
            _brush.Color = color;
            _device.DrawText(text, text.Length, font, new RawRectangleF(x, y, float.PositiveInfinity, float.PositiveInfinity), _brush, DrawTextOptions.NoSnap, MeasuringMode.Natural);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBitmap(Dx2DBitmap bitmap, float x, float y, float opacity)
        {
            _device.DrawBitmap(bitmap, new RawRectangleF(x, y, x + bitmap.Width, y + bitmap.Height), opacity, BitmapInterpolationMode.Linear);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBitmap(Dx2DBitmap bitmap, float x, float y, float width, float height, float opacity)
        {
            _device.DrawBitmap(bitmap, new RawRectangleF(x, y, x + width, y + height), opacity, BitmapInterpolationMode.Linear, new RawRectangleF(0, 0, bitmap.Width, bitmap.Height));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangle(float x, float y, float width, float height, Dx2DColor color)
        {
            _brush.Color = color;
            _device.FillRectangle(new RawRectangleF(x, y, x + width, y + height), _brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, Dx2DColor color)
        {
            _brush.Color = color;
            _device.FillRoundedRectangle(new RoundedRectangle()
            {
                Rect = new RawRectangleF(x, y, x + width, y + height),
                RadiusX = radiusX,
                RadiusY = radiusY
            }, _brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCircle(float x, float y, float radius, Dx2DColor color)
        {
            _brush.Color = color;
            _device.FillEllipse(new Ellipse(new RawVector2(x, y), radius, radius), _brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillEllipse(float x, float y, float width, float height, Dx2DColor color)
        {
            _brush.Color = color;
            _device.FillEllipse(new Ellipse(new RawVector2(x, y), width, height), _brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillTriangle(float x_x, float x_y, float y_x, float y_y, float z_x, float z_y, Dx2DColor color)
        {
            _brush.Color = color;

            using (var path = new PathGeometry(_factory))
            {
                using (var sink = path.Open())
                {
                    sink.BeginFigure(new RawVector2(x_x, x_y), FigureBegin.Filled);
                    sink.AddLine(new RawVector2(y_x, y_y));
                    sink.AddLine(new RawVector2(z_x, z_y));
                    sink.EndFigure(FigureEnd.Closed);
                    sink.Close();
                    _device.FillGeometry(path, _brush);
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (!_brush.IsDisposed) _brush.Dispose();
            if (!_device.IsDisposed) _device.Dispose();
            if (!_factory.IsDisposed) _factory.Dispose();
        }
    }
}