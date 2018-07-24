using GTA;

namespace GGOHud
{
    class Checks
    {
        /// <summary>
        /// Checks if the current weapon of the player should be shown.
        /// </summary>
        /// <returns>True if the weapon is banned, False otherwise.</returns>
        public static bool IsCurrentWeaponBanned()
        {
            return Weapons.Hidden.Contains(Game.Player.Character.Weapons.Current.Hash);
        }

        /// <summary>
        /// Checks if the current weapon of the player is a sidearm.
        /// Lemon says: I consider that a sidearm is a small firearm that can fit on your pocket or under your clothes.
        /// </summary>
        /// <returns>True if the weapon is a sidearm, False otherwise.</returns>
        public static bool IsCurrentWeaponSidearm()
        {
            return Weapons.Sidearms.Contains(Game.Player.Character.Weapons.Current.Hash);
        }
    }
}
