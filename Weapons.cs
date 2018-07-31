using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace GGOHud
{
    class Weapons
    {
        /// <summary>
        /// The type of the weapon that the player currently has.
        /// </summary>
        public enum Type
        {
            Banned = -1,
            Main = 0,
            Sidearm = 1,
            Melee = 2
        }
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static readonly List<int> Banned = new List<int> { 1595662460, 1548507267, -1609580060, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), Stun Gun (Type), SMG.
        /// </summary>
        public static readonly List<int> Sidearms = new List<int> { 416676503, 690389602, -957766203 };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type), Boxer (Type).
        /// </summary>
        public static readonly List<int> Melee = new List<int> { -1609580060, -728555052 };

        /// <summary>
        /// Checks the weapon that the player currently has equiped.
        /// </summary>
        /// <returns>The Type of weapon.</returns>
        public static Type CurrentType()
        {
            // Store the hash of the current weapon
            int WeaponType = Function.Call<int>(Hash.GET_WEAPONTYPE_GROUP, Game.Player.Character.Weapons.Current.Hash.GetHashCode());

            // If the weapon is Banned
            if (Banned.Contains(WeaponType))
                return Type.Banned;
            // If the weapon is not a firearm
            if (Melee.Contains(WeaponType))
                return Type.Melee;
            // If the weapon is a small firearm
            if (Sidearms.Contains(WeaponType))
                return Type.Sidearm;
            // If the weapon is other than the previous ones
            else
                return Type.Main;
        }
    }
}
