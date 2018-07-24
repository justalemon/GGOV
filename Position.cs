using System.Drawing;

namespace GGOHud
{
    class Position
    {
        // Icons on the left
        public static readonly Point PlayerIcon = Coords.CalculatePoint(79f, 73.5f);
        public static readonly Point PrimaryIcon = Coords.CalculatePoint(84.5f, 73.5f);
        public static readonly Point SecondaryIcon = Coords.CalculatePoint(90f, 73.5f);

        // Dummies for the Icons (not equiped, invalid or banned)
        public static readonly Point PrimaryDummy = Coords.CalculatePoint(85f, 74.5f);
        public static readonly Point SecondaryDummy = Coords.CalculatePoint(90.5f, 74.5f);

        // Images for the primary and secondary guns
        public static readonly Point PrimaryImage = Coords.CalculatePoint(83.55f, 81.5f);
        public static readonly Point SecondaryImage = Coords.CalculatePoint(89.25f, 81.5f);

        // Fields with information
        public static readonly Point PlayerName = Coords.CalculatePoint(79.1f, 77.5f);
        public static readonly Point PrimaryAmmo = Coords.CalculatePoint(85f, 77.6f);
        public static readonly Point SecondaryAmmo = Coords.CalculatePoint(90.75f, 77.6f);

        // Backgrounds
        // Icons
        public static readonly Point PlayerIconBG = Coords.CalculatePoint(79f, 73.2f);
        public static readonly Point PrimaryIconBG = Coords.CalculatePoint(84.6f, 73.2f);
        public static readonly Point SecondaryIconBG = Coords.CalculatePoint(90.2f, 73.2f);
        // Player Information
        public static readonly Point PlayerInfoBG = Coords.CalculatePoint(79f, 77f);
        // Ammo
        public static readonly Point PrimaryAmmoBG = Coords.CalculatePoint(84.6f, 77f);
        public static readonly Point SecondaryAmmoBG = Coords.CalculatePoint(90.2f, 77f);
        // Weapons
        public static readonly Point PrimaryBG = Coords.CalculatePoint(84.6f, 80.8f);
        public static readonly Point SecondaryBG = Coords.CalculatePoint(90.2f, 80.8f);

        // Size of the Backgrounds
        // Icon Size
        public static readonly Size IconSize = Coords.CalculateSize(4.75f, 2.8f);
        // Icon Background
        public static readonly Size IconBGSize = Coords.CalculateSize(5f, 3.5f);
        // Player Information
        public static readonly Size PlayerBGSize = Coords.CalculateSize(5f, 15f);
        // Weapon Icon
        public static readonly Size WeaponSize = Coords.CalculateSize(6.75f, 9.50f);
        // Weapon Background
        public static readonly Size WeaponBG = Coords.CalculateSize(5f, 11.1f);
    }
}
