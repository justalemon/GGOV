using GGO.Shared.Properties;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GGO.Shared
{
    public class Images
    {
        /// <summary>
        /// Gets a weapon image or Bitmap from the resources.
        /// </summary>
        /// <param name="Name">The name of the weapon.</param>
        /// <returns>The requested Bitmap.</returns>
        public static Bitmap GetWeaponImages(string Name)
        {
            return (Bitmap)Resources.ResourceManager.GetObject("Gun" + Name);
        }

        /// <summary>
        /// Creates a PNG file from a resource image.
        /// </summary>
        /// <param name="Origin">The original resource.</param>
        /// <param name="Filename">The output filename.</param>
        /// <returns>The absolute path of the output file.</returns>
        public static string ResourceToPNG(Bitmap Origin, string Filename)
        {
            // This is going to be our image location
            string OutputFile = Path.Combine(Path.GetTempPath(), "GGO", Filename + ".png");

            // If the file already exists, return it and don't waste resources
            if (File.Exists(OutputFile))
            {
                return OutputFile;
            }

            // If our %TEMP%\GGO folder does not exist, create it
            if (!Directory.Exists(Path.GetDirectoryName(OutputFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(OutputFile));
            }

            // Create a memory stream
            MemoryStream ImageStream = new MemoryStream();
            // Dump the image into it
            Origin.Save(ImageStream, ImageFormat.Png);
            // And close the stream
            ImageStream.Close();
            // Finally, write the stream into the disc
            File.WriteAllBytes(OutputFile, ImageStream.ToArray());

            // At this point we conclude that it was successfully
            // So we return the image location
            return OutputFile;
        }
    }
}
