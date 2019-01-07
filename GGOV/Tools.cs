using GTA;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GGO
{
    /// <summary>
    /// Set of tools used to make the development of GGO easier.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// The indexes of the images.
        /// </summary>
        private static int Index = 0;

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

        /// <summary>
        /// Cleans up the existing index list.
        /// </summary>
        public static void ResetImageIndex()
        {
            Index = 0;
        }

        /// <summary>
        /// Draws an image based on a Bitmap.
        /// </summary>
        /// <param name="FileName">The name of the file to draw.</param>
        /// <param name="Position">Where the image should be drawn.</param>
        /// <param name="Sizes">The size of the image.</param>
        public static void DrawImage(string FileName, Point Position, Size Sizes)
        {
            // Create the path of the image
            string ImagePath = $"scripts\\GGO\\Images\\{FileName}.png";

            // If the image does not exists, notify the user and return
            if (!File.Exists(ImagePath))
            {
                UI.Notify($"Warning: '{ImagePath}' does not exists!");
                return;
            }

            // Finally, if the file exists draw it
            UI.DrawTexture(ImagePath, Index, 0, 150, Position, Sizes);
            Index++;
        }
    }
}
