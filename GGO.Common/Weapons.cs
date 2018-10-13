using GTA;
using System.Drawing;
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

        /// <summary>
        /// Gets the display name from the hash of the weapon with added prefix for resources.
        /// </summary>
        /// <returns>The name of the weapon.</returns>
        public static string CurrentWeaponName
        {
            get
            {
                return "GUN_" + GTA.Weapon.GetDisplayNameFromHash(Game.Player.Character.Weapons.Current.Hash);
            }
        }

        /// <summary>
        /// Gets the Bitmap resource of the current weapon.
        /// </summary>
        /// <returns>The bitmap of the weapon.</returns>
        public static Bitmap CurrentWeaponResource
        {
            get
            {
                if((Bitmap)Properties.Resources.ResourceManager.GetObject(CurrentWeaponName) != null)
                    return (Bitmap)Properties.Resources.ResourceManager.GetObject(CurrentWeaponName);
                else
                    return (Bitmap)Properties.Resources.ResourceManager.GetObject("GUN_WTT_PIST");
            }
        }
    }
}