﻿using System.Drawing;

namespace GGOHud
{
    class Position
    {
        // Icons on the left
        // Images
        public static readonly Point PlayerIcon = Coords.CalculatePoint(79.1f, 73.85f);
        public static readonly Point PrimaryIcon = Coords.CalculatePoint(84.6f, 73.85f);
        public static readonly Point SecondaryIcon = Coords.CalculatePoint(90.1f, 73.85f);
        // Dummies
        public static readonly Point PrimaryIconDummy = Coords.CalculatePoint(85f, 74.9f);
        public static readonly Point SecondaryIconDummy = Coords.CalculatePoint(90.5f, 74.9f);
        // Backgrounds
        public static readonly Point PlayerIconBG = Coords.CalculatePoint(79f, 73.75f);
        public static readonly Point PrimaryIconBG = Coords.CalculatePoint(84.6f, 73.75f);
        public static readonly Point SecondaryIconBG = Coords.CalculatePoint(90.2f, 73.75f);

        // Player Information
        // Name
        public static readonly Point PlayerName = Coords.CalculatePoint(79.1f, 77.5f);
        // Backgrounds
        public static readonly Point PlayerInfoBG = Coords.CalculatePoint(79f, 77f);

        // Weapon Information
        // Ammo
        public static readonly Point PrimaryAmmo = Coords.CalculatePoint(85f, 78.5f);
        public static readonly Point SecondaryAmmo = Coords.CalculatePoint(90.75f, 78.5f);
        // Dummies
        public static readonly Point PrimaryAmmoDummy = Coords.CalculatePoint(85f, 78.1f);
        public static readonly Point SecondaryAmmoDummy = Coords.CalculatePoint(90.75f, 78.1f);
        // Images
        public static readonly Point PrimaryImage = Coords.CalculatePoint(85.25f, 81.25f);
        public static readonly Point SecondaryImage = Coords.CalculatePoint(90.75f, 81.25f);
        // Backgrounds
        public static readonly Point PrimaryAmmoBG = Coords.CalculatePoint(84.6f, 77f);
        public static readonly Point SecondaryAmmoBG = Coords.CalculatePoint(90.2f, 77f);
        public static readonly Point PrimaryBG = Coords.CalculatePoint(84.6f, 80.25f);
        public static readonly Point SecondaryBG = Coords.CalculatePoint(90.2f, 80.25f);

        // Health Bar
        public static readonly Point HealthBar = Coords.CalculatePoint(82.507f, 77.625f);
        public static readonly Point HealthDividerOne = Coords.CalculatePoint(82.25f, 77.625f);
        public static readonly Point HealthDividerTwo = Coords.CalculatePoint(82.25f, 82.175f);
        public static readonly Point HealthDividerThree = Coords.CalculatePoint(82.25f, 86.725f);
        public static readonly Point HealthDividerFour = Coords.CalculatePoint(82.25f, 91.3f);

        // Sizes
        // Icon Size
        public static readonly Size IconSize = Coords.CalculateSize(4.75f, 2.8f);
        // Icon Background
        public static readonly Size SquaredBG = Coords.CalculateSize(5f, 3f);
        // Player Information
        public static readonly Size PlayerBGSize = Coords.CalculateSize(5f, 15f);
        // Weapon Picture
        public static readonly Size WeaponSize = Coords.CalculateSize(3.25f, 9.5f);
        // Weapon Background
        public static readonly Size WeaponBG = Coords.CalculateSize(5f, 11.75f);
        // Health Bar
        public static readonly Size HealthBarS = Coords.CalculateSize(0.4f, 13.8f);
        public static readonly Size HealthBarDivider = Coords.CalculateSize(1.1f, 0.1f);
    }
}
