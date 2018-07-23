using GTA;
using System;
using System.Drawing;

namespace GGOHud
{
    class Calculations
    {
        public static int CalculateVertical(float Offset)
        {
            return Convert.ToInt32((Game.ScreenResolution.Height / 100) * Offset);
        }

        public static int CalculateHorizontal(float Offset)
        {
            return Convert.ToInt32((Game.ScreenResolution.Width / 100) * Offset);
        }

        public static Point CalculateComplete(float Width, float Height)
        {
            return new Point(CalculateHorizontal(Height), CalculateVertical(Width));
        }
    }
}
