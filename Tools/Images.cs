using GTA;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GGOHud.Tools
{
    /// <summary>
    /// Tools to create, edit and manipulate the UI images.
    /// </summary>
    class Images
    {
        /// <summary>
        /// A set of characters to create random strings.
        /// </summary>
        public static string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        /// <summary>
        /// Our generator of random characters.
        /// </summary>
        public static Random Generator = new Random();

        /// <summary>
        /// Gets a gun image based on the player current weapon.
        /// </summary>
        /// <returns>The absolute location of the weapon.</returns>
        public static string WeaponImage
        {
            get
            {
                string CurrentWeapon = Game.Player.Character.Weapons.Current.Hash.ToString();
                return AppDomain.CurrentDomain.BaseDirectory + "\\GGOHud\\" + "GUN_" + CurrentWeapon + ".png";
            }
        }

        /// <summary>
        /// A function that saves a resource to a temporary file.
        /// </summary>
        /// <param name="Resource">The name of the resource.</param>
        /// <returns>The path of the new file.</returns>
        public static string ResourceToFile(Bitmap Resource)
        {
            string OutputLocation = Path.Combine(Path.GetTempPath(), "GGOHud", RandomString(10) + ".png");

            if (!Directory.Exists(Path.GetDirectoryName(OutputLocation)))
                Directory.CreateDirectory(Path.GetDirectoryName(OutputLocation));

            MemoryStream ImageStream = new MemoryStream();
            Resource.Save(ImageStream, ImageFormat.Png);
            ImageStream.Close();
            File.WriteAllBytes(OutputLocation, ImageStream.ToArray());

            return OutputLocation;
        }

        /// <summary>
        /// A simple function that returns an alphanumeric string.
        /// </summary>
        /// <param name="Length">The length of the string.</param>
        /// <returns>A random alphanumeric string.</returns>
        public static string RandomString(int Length)
        {
            return new string(Enumerable.Repeat(Chars, Length).Select(s => s[Generator.Next(s.Length)]).ToArray());
        }
    }
}
