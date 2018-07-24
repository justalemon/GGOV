using GTA;
using System;

namespace GGOHud
{
    class Image
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
    }
}
