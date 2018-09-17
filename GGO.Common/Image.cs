using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GGO.Common
{
    public class Image
    {
        /// <summary>
        /// Our generator of random characters.
        /// </summary>
        private static Random Generator = new Random();

        /// <summary>
        /// Creates a random string of the desired length.
        /// </summary>
        /// <param name="Length">The length of the string.</param>
        /// <returns>A random alphanumeric string of the desired length.</returns>
        public static string RandomString(int Length)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", Length).Select(s => s[Generator.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Creates a PNG file from a resource image.
        /// </summary>
        /// <param name="Origin">The original resource.</param>
        /// <returns>The absolute path of the output file.</returns>
        public static string ResourceToPNG(Bitmap Origin)
        {
            // This is going to be our image location
            string OutputFile = Path.Combine(Path.GetTempPath(), "GGO", RandomString(10) + ".png");

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
