using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace GGO.Extensions
{
    public static class WeaponExtensions
    {
        /// <summary>
        /// Specific weapons that are going to be counted as items.
        /// </summary>
        public static List<WeaponHash> ItemWeapons = new List<WeaponHash>
        {
            WeaponHash.StunGun,
            WeaponHash.FlareGun
        };
        /// <summary>
        /// Types of weapons that are going to be counted as items.
        /// </summary>
        public static List<WeaponGroup> ItemGroups = new List<WeaponGroup>
        {
            WeaponGroup.PetrolCan,
            WeaponGroup.Thrown,
            WeaponGroup.Unarmed,
            WeaponGroup.Melee
        };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// </summary>
        public static List<WeaponGroup> SecondaryGroups = new List<WeaponGroup>
        {
            WeaponGroup.Pistol,
            WeaponGroup.SMG
        };

        /// <summary>
        /// Gets the correct ammo as an string representation.
        /// </summary>
        /// <returns>The correct ammo as a string.</returns>
        public static string GetCorrectAmmo(this Weapon PlayerWeapon)
        {
            if (PlayerWeapon.Hash == WeaponHash.StunGun || PlayerWeapon.Group == WeaponGroup.Melee)
            {
                return "∞";
            }
            else
            {
                return PlayerWeapon.Ammo.ToString();
            }
        }

        /// <summary>
        /// Gets the type for the current player weapon.
        /// </summary>
        /// <returns>The usage of the weapon.</returns>
        public static Usage GetStyle(this Weapon PlayerWeapon)
        {
            if (ItemGroups.Contains(PlayerWeapon.Group) || ItemWeapons.Contains(PlayerWeapon.Hash))
                return Usage.Item;
            else if (SecondaryGroups.Contains(PlayerWeapon.Group))
                return Usage.Sidearm;
            else
                return Usage.Main;
        }

        public static bool IsAmmoAvailable(this Weapon PlayerWeapon)
        {
            switch (PlayerWeapon.GetStyle())
            {
                case Usage.Main:
                case Usage.Sidearm:
                    return true;
                default:
                    return false;
            }
        }
    }
}
