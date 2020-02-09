using DirectXOverlay.ConsoleApp.Utilities;

namespace DirectXOverlay.ConsoleApp.RandomTypes
{
    public class RandInt
    {
        public RandInt(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }
        public int Value => RandomHelper.GetInt32(Min, Max);
    }
}