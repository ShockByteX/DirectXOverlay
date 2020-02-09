using DirectXOverlay.DirectX;

namespace DirectXOverlay.ConsoleApp.Shapes
{
    public class Circle
    {
        public int X;
        public int Y;
        public int Diameter;
        public int Radius;
        public int SpeedX;
        public int SpeedY;
        public Dx2DColor Color;

        public Circle(int x, int y, int radius, int speedX, int speedY, Dx2DColor color)
        {
            Diameter = radius << 1;
            Radius = radius;
            X = x;
            Y = y;
            SpeedX = speedX;
            SpeedY = speedY;
            Color = color;
        }

        public void Update(int width, int height)
        {
            UpdatePosition(ref X, ref SpeedX, width);
            UpdatePosition(ref Y, ref SpeedY, height);
        }

        private void UpdatePosition(ref int position, ref int speed, int boundMax)
        {
            var newPosition = position + speed;

            var positiveBound = newPosition + Radius;
            var negativeBound = newPosition - Radius;

            if (positiveBound >= boundMax)
            {
                newPosition = boundMax - Radius;
                speed *= -1;
            }
            else if (negativeBound <= 0)
            {
                newPosition = Radius;
                speed *= -1;
            }

            position = newPosition;
        }
    }
}