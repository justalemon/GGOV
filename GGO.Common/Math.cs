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

        /// <summary>
        /// Creates a Point from a set of configuration parameters.
        /// </summary>
        /// <param name="Settings">The Raw settings.</param>
        /// <param name="Value">The value to get from the configuration.</param>
        /// <returns>The Point created from the configuration values.</returns>
        public static Point PointFromConfig(ScriptSettings Settings, string Value)
        {
            return PointFromConfig(Settings, Value + "X", Value + "Y");
        }
        
        /// <summary>
        /// Creates a Point from a set of configuration parameters.
        /// </summary>
        /// <param name="Settings">The Raw settings.</param>
        /// <param name="X">The value of X from the configuration.</param>
        /// <param name="Y">The value of Y from the configuration.</param>
        /// <returns>The Point created from the configuration values.</returns>
        public static Point PointFromConfig(ScriptSettings Settings, string X, string Y)
        {
            int AbsoluteX = Percentage(Settings.GetValue("GGO", X, 0f), Game.ScreenResolution.Width);
            int AbsoluteY = Percentage(Settings.GetValue("GGO", Y, 0f), Game.ScreenResolution.Height);

            return new Point(AbsoluteX, AbsoluteY);
        }

        /// <summary>
        /// Creates a Size from a set of configuration parameters.
        /// </summary>
        /// <param name="Settings">The Raw settings.</param>
        /// <param name="Value">The value to get from the configuration.</param>
        /// <returns>The Size created from the configuration values.</returns>
        public static Size SizeFromConfig(ScriptSettings Settings, string Value)
        {
            return SizeFromConfig(Settings, Value + "W", Value + "H");
        }

        /// <summary>
        /// Creates a Size from a set of configuration parameters.
        /// </summary>
        /// <param name="Settings">The Raw settings.</param>
        /// <param name="Width">The Width from the configuration.</param>
        /// <param name="Height">The Height from the configuration.</param>
        /// <returns>The Size created from the configuration values.</returns>
        public static Size SizeFromConfig(ScriptSettings Settings, string Width, string Height)
        {
            int AbsoluteWidth = Percentage(Settings.GetValue("GGO", Width, 0f), Game.ScreenResolution.Width);
            int AbsoluteHeight = Percentage(Settings.GetValue("GGO", Height, 0f), Game.ScreenResolution.Height);

            return new Size(AbsoluteWidth, AbsoluteHeight);
        }
    }
}
