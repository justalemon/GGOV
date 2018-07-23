using GTA;
using System;
using System.Drawing;

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

        public static Point CalculateComplete(float Width, float Height)
        {
            return new Point(CalculateVertical(Width), CalculateHorizontal(Height));
        }
    }
}
