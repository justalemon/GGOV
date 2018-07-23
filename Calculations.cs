using GTA;

namespace GGOHud
{
    class Calculations
    {
        public static float CalculateVertical(float Offset)
        {
            return Game.ScreenResolution.Height / Offset;
        }

        public static float CalculateHorizontal(float Offset)
        {
            return Game.ScreenResolution.Width / Offset;
        }
    }
}
