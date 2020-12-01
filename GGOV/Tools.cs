using GTA;
using GTA.Native;

namespace GGO
{
    /// <summary>
    /// The Type of a specific Weapon.
    /// </summary>
    /// <seealso cref="WeaponGroup"/>
    public enum WeaponType
    {
        Invalid = -1,
        Unknown = 0,
        Primary = 1,
        Secondary = 2,
    }

    /// <summary>
    /// Tools to make the job easier.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Gets the type of weapon based on the Hash.
        /// </summary>
        /// <param name="weapon">The <see cref="WeaponHash"/> to check.</param>
        /// <returns>The <see cref="WeaponType"/> of the weapon.</returns>
        public static WeaponType GetWeaponType(WeaponHash weapon)
        {
            // First, check the hash of the weapon itself
            switch (weapon)
            {
                // Invalid Weapons (aka no use for the GGO Mod)
                case WeaponHash.Unarmed:
                case WeaponHash.StunGun:
                    return WeaponType.Invalid;
                // Primary Weapons (here because they have the wrong group)
                case WeaponHash.Musket:
                    return WeaponType.Primary;
                // Secondary Weapons (also with invalid group)
                case WeaponHash.MicroSMG:
                case WeaponHash.MachinePistol:
                case WeaponHash.MiniSMG:
                case WeaponHash.CompactGrenadeLauncher:
                case WeaponHash.SawnOffShotgun:
                case WeaponHash.DoubleBarrelShotgun:
                case WeaponHash.SweeperShotgun:
                    return WeaponType.Secondary;
            }

            // Then, check the group
            switch (Function.Call<WeaponGroup>(Hash.GET_WEAPONTYPE_GROUP, weapon))
            {
                case WeaponGroup.AssaultRifle:
                case WeaponGroup.MG:
                case WeaponGroup.SMG:
                case WeaponGroup.Heavy:
                case WeaponGroup.Shotgun:
                case WeaponGroup.Sniper:
                    return WeaponType.Primary;
                case WeaponGroup.Pistol:
                    return WeaponType.Secondary;
            }

            // If we got here, we don't know what type of weapon it is
            return WeaponType.Unknown;
        }
    }
}
