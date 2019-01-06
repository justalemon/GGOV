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
        /// <param name="Resource">The Bitmap image to draw.</param>
        /// <param name="Position">Where the image should be drawn.</param>
        /// <param name="Sizes">The size of the image.</param>
        public static void DrawImage(Bitmap Resource, string Filename, Point Position, Size Sizes)
        {
            // This is going to be our image location
            string OutputFile = Path.Combine(Path.GetTempPath(), "GGO", Filename + ".png");

            // If the file already exists, return it and don't waste resources
            if (!File.Exists(OutputFile))
            {
                // If our %TEMP%\GGO folder does not exist, create it
                if (!Directory.Exists(Path.GetDirectoryName(OutputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(OutputFile));
                }

                // Create a memory stream
                MemoryStream ImageStream = new MemoryStream();
                // Dump the image into it
                Resource.Save(ImageStream, ImageFormat.Png);
                // And close the stream
                ImageStream.Close();
                // Finally, write the stream into the disc
                File.WriteAllBytes(OutputFile, ImageStream.ToArray());
            }

            UI.DrawTexture(OutputFile, Index, 0, 150, Position, Sizes);
            Index++;
        }
    }
}
