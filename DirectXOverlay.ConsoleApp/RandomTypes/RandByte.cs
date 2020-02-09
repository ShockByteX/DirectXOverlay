using DirectXOverlay.ConsoleApp.Utilities;

namespace DirectXOverlay.ConsoleApp.RandomTypes
{
    public class RandByte
    {
        public RandByte(byte min, byte max)
        {
            Min = min;
            Max = max;
        }

        public byte Min { get; }
        public byte Max { get; }
        public byte Value => RandomHelper.GetByte(Min, Max);
    }
}