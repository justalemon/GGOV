using GTA;
using System;

namespace GGOHud
{
    class Calculations
    {
        public static int CalculateVertical(float Offset)
        {
            return Convert.ToInt32(Game.ScreenResolution.Height / Offset);
        }

        public static int CalculateHorizontal(float Offset)
        {
            return Convert.ToInt32(Game.ScreenResolution.Width / Offset);
        }
    }
}
