using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectXOverlay.ConsoleApp.RandomTypes;
using DirectXOverlay.ConsoleApp.Shapes;
using DirectXOverlay.DirectX;
using DirectXOverlay.Utilities;
using DirectXOverlay.Windows;

namespace DirectXOverlay.ConsoleApp
{
    public class Program
    {
        private static readonly RandInt Radius = new RandInt(1, 30);
        private static readonly RandInt Speed = new RandInt(-4, 4);
        private static readonly RandColor Color = new RandColor(new RandByte(127, byte.MaxValue), new RandByte(10, 210));

        private static readonly Circle[] Circles = new Circle[1850];
        private static readonly Dx2DFont Font = new Dx2DFont("Arial", 16, Dx2DFontWeight.Black, Dx2DFontStyle.Normal);
        private static readonly Dx2DColor BackgroundColor = new Dx2DColor(10, 20, 30, 0);
        private static readonly Dx2DColor FontColor = new Dx2DColor(220, 220, 220);

        private static void Main()
        {
            //var process = Process.GetProcessesByName("telegram").Single(x => x.MainWindowHandle != IntPtr.Zero);
            var window = new OverlayWindow(0, 0, 1920, 1080, true)
            {
                FramesPerSecond = 5000
            };

            window.OnDraw += Overlay_OnDraw;

            GenerateCircles(window);

            void UpdateCircles()
            {
                foreach (var circle in Circles)
                {
                    circle.Update(window.Width, window.Height);
                }
            }

            for (int i = 0; i < 180; i++)
            {
                RunHelper.ConsistentRun(UpdateCircles, 16);
            }

            window.Dispose();

            Console.ReadKey(true);
        }

        private static void Overlay_OnDraw(OverlayWindow window, Dx2DGraphics graphics)
        {
            graphics.ClearScene(BackgroundColor);

            foreach (var circle in Circles)
            {
                graphics.FillCircle(circle.X, circle.Y, circle.Radius, circle.Color);
            }

            graphics.DrawText(" Welcome to the DXOverlay example!", 0, 0, Font, FontColor);
            graphics.DrawText(window.CountedFramesPerSecond.ToString(), window.Width - 40, 0, Font, FontColor);
        }

        private static void GenerateCircles(Window window)
        {
            var radius = Radius.Max;

            for (var i = 0; i < Circles.Length; i++)
            {
                var x = new RandInt(radius, window.Width - radius).Value;
                var y = new RandInt(radius, window.Height - radius).Value;
                var speedX = GetCircleSpeed();
                var speedY = GetCircleSpeed();
                Circles[i] = new Circle(x, y, Radius.Value, speedX, speedY, Color.Value);
            }
        }

        private static int GetCircleSpeed()
        {
            var speed = 0;

            while (speed == 0)
            {
                speed = Speed.Value;
            }

            return speed;
        }
    }
}