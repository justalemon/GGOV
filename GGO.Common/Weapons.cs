using GTA;
using System.Collections.Generic;

namespace GGO.Common
{
    public class Weapons
    {
        /// <summary>
        /// The type of the weapon that the player currently has.
        /// </summary>
        public enum Type
        {
            Banned = -1,
            Main = 0,
            Sidearm = 1,
            Melee = 2,
            Double = 3
        }

        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static readonly List<WeaponGroup> BannedWeapons = new List<WeaponGroup> { WeaponGroup.PetrolCan, WeaponGroup.Thrown, WeaponGroup.Unarmed, 0 };

        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), Stun Gun --seems to be depricated-- (Type), SMG.
        /// </summary>
        public static readonly List<WeaponGroup> SecondaryWeapons = new List<WeaponGroup> { WeaponGroup.Pistol, WeaponGroup.SMG };

        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type), Boxer --seems to be depricated-- (Type).
        /// </summary>
        public static readonly List<WeaponGroup> MeleeWeapons = new List<WeaponGroup> { WeaponGroup.Melee };

        /// <summary>
        /// Checks the weapon that the player currently has equiped.
        /// </summary>
        /// <returns>The Type of weapon.</returns>
        public static Type CurrentWeaponType
        {
            get
            {
                // Store the hash of the current weapon
                WeaponGroup WeaponType = Game.Player.Character.Weapons.Current.Group;

                // Return the first match, in order
                // From dangerous to normal
                if (BannedWeapons.Contains(WeaponType))
                    return Type.Banned;
                else if (MeleeWeapons.Contains(WeaponType))
                    return Type.Melee;
                else if (SecondaryWeapons.Contains(WeaponType))
                    return Type.Sidearm;
                else
                    return Type.Main;
            }
        }
    }
}
