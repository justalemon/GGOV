using GTA;
using System;
using System.Drawing;

namespace GGO.Common
{
    public static class Math
    {
        /// <summary>
        /// Calculates the percentage of a number.
        /// </summary>
        /// <returns>The value that corresponds to that percentage.</returns>
        public static int Percentage(float Percentage, int Of)
        {
            return Convert.ToInt32((Percentage / 100) * Of);
        }

        public static Point PointFromConfig(ScriptSettings Settings, string Value)
        {
            return PointFromConfig(Settings, Value + "X", Value + "Y");
        }
        public static Point PointFromConfig(ScriptSettings Settings, string X, string Y)
        {
            int AbsoluteX = Percentage(Settings.GetValue("GGO", X, 0f), Game.ScreenResolution.Width);
            int AbsoluteY = Percentage(Settings.GetValue("GGO", Y, 0f), Game.ScreenResolution.Height);

            return new Point(AbsoluteX, AbsoluteY);
        }

        public static Size SizeFromConfig(ScriptSettings Settings, string Value)
        {
            return SizeFromConfig(Settings, Value + "W", Value + "H");
        }
        public static Size SizeFromConfig(ScriptSettings Settings, string Width, string Height)
        {
            int AbsoluteWidth = Percentage(Settings.GetValue("GGO", Width, 0f), Game.ScreenResolution.Width);
            int AbsoluteHeight = Percentage(Settings.GetValue("GGO", Height, 0f), Game.ScreenResolution.Height);

            return new Size(AbsoluteWidth, AbsoluteHeight);
        }
    }
}
