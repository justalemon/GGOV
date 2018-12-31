using GTA;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// Set of tools used to make the development of GGO easier.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Creates a Point with absolute values based on relative ones.
        /// </summary>
        /// <param name="X">The relative X.</param>
        /// <param name="Y">The relative Y.</param>
        /// <returns>A Point with absolute values.</returns>
        public static Point LiteralPoint(float X, float Y)
        {
            return new Point((int)(UI.WIDTH * X), (int)(UI.HEIGHT * Y));
        }

        /// <summary>
        /// Creates a Size with absolute values based on relative ones.
        /// </summary>
        /// <param name="Width">The relative X.</param>
        /// <param name="Height">The relative Y.</param>
        /// <returns>A Size with absolute values.</returns>
        public static Size LiteralSize(float Width, float Height)
        {
            return new Size((int)(UI.WIDTH * Width), (int)(UI.HEIGHT * Height));
        }
    }
}
