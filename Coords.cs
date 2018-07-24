using GTA;
using System;
using System.Drawing;

namespace GGOHud
{
    class Coords
    {
        /// <summary>
        /// Calculates the Vertical Offset from 0 to 100.
        /// </summary>
        /// <param name="Offset">The Offset from 0 to 100, where 0 is the base and 100 is the maximum.</param>
        /// <returns>The requested offset.</returns>
        public static int CalculateVertical(float Offset)
        {
            return Convert.ToInt32((Game.ScreenResolution.Height / 100) * Offset);
        }

        /// <summary>
        /// Calculates the Horizontal Offset from 0 to 100.
        /// </summary>
        /// <param name="Offset">The Offset from 0 to 100, where 0 is the base and 100 is the maximum.</param>
        /// <returns>The requested offset.</returns>
        public static int CalculateHorizontal(float Offset)
        {
            return Convert.ToInt32((Game.ScreenResolution.Width / 100) * Offset);
        }

        /// <summary>
        /// Calculates the X and Y parameters on the screen based on a 0-100 scale/offset.
        /// </summary>
        /// <param name="Width">X</param>
        /// <param name="Height">Y</param>
        /// <returns>A Point with the aproximated offset.</returns>
        public static Point CalculatePoint(float Width, float Height)
        {
            return new Point(CalculateHorizontal(Height), CalculateVertical(Width));
        }

        /// <summary>
        /// Calculates the X and Y parameters to create an object based on a 0-100 scale/offset.
        /// </summary>
        /// <param name="Width">X</param>
        /// <param name="Height">Y</param>
        /// <returns>The Size based on the offset.</returns>
        public static Size CalculateSize(float Width, float Height)
        {
            return new Size(CalculateHorizontal(Height), CalculateVertical(Width));
        }
    }
}
