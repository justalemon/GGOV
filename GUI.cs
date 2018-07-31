using GTA;
using System;
using System.Drawing;

namespace GGOHud
{
    class GUI
    {
        /// <summary>
        /// The type of Icon to get.
        /// </summary>
        public enum Icon
        {
            Generic = 0,
            Player = 1,
            Primary = 2,
            Secondary = 3
        }
        /// <summary>
        /// The directory that contains our Images.
        /// </summary>
        public static readonly string Folder = AppDomain.CurrentDomain.BaseDirectory + "\\GGOHud\\";

        /// <summary>
        /// Gets an icon.
        /// </summary>
        /// <returns>The absolute location of the icon.</returns>
        public static string GetIcon(Icon Type)
        {
            string IconName = GGOHudScript.Config.GetValue("GGOHud", Type.ToString() + "Icon", "HUD_Generic");
            return Folder + IconName + ".png";
        }

        /// <summary>
        /// Gets a gun picture.
        /// </summary>
        /// <returns>The absolute location of the weapon.</returns>
        public static string GetWeapon()
        {
            string CurrentWeapon = Game.Player.Character.Weapons.Current.Hash.ToString();
            return Folder + "GUN_" + CurrentWeapon + ".png";
        }

        public static Size SizeFromConfig(string ConfigName)
        {
            float WidthOffset = GGOHudScript.Config.GetValue("Sizes", ConfigName + "W", 0f);
            float HeigthOffset = GGOHudScript.Config.GetValue("Sizes", ConfigName + "H", 0f);

            int Width = Convert.ToInt32((Game.ScreenResolution.Width / 100) * WidthOffset);
            int Heigth = Convert.ToInt32((Game.ScreenResolution.Height / 100) * HeigthOffset);

            return new Size(Width, Heigth);
        }

        public static Point PointFromConfig(string ConfigName)
        {
            return PointFromConfig(ConfigName + "X", ConfigName + "Y");
        }

        public static Point PointFromConfig(string XConfig, string YConfig)
        {
            float XOffset = GGOHudScript.Config.GetValue("Positions", XConfig, 0f);
            float YOffset = GGOHudScript.Config.GetValue("Positions", YConfig, 0f);

            int X = Convert.ToInt32((Game.ScreenResolution.Width / 100) * XOffset);
            int Y = Convert.ToInt32((Game.ScreenResolution.Height / 100) * YOffset);

            return new Point(X, Y);
        }
    }
}
