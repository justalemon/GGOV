using GTA;

namespace GGO
{
    /// <summary>
    /// Represents the Secondary Weapon Slot on the Player Fields.
    /// </summary>
    public sealed class WeaponSecondary : Weapon
    {
        /// <summary>
        /// If this weapon is valid for the Secondary slot.
        /// </summary>
        public override bool IsWeaponValid
        {
            get
            {
                // What is considered a Secondary Weapon:
                // - Group: Pistols
                // - Weapon: Micro SMG
                // - Weapon: Machine Pistol
                // - Weapon: Mini SMG
                // - Weapon: Compact Grenade Launcher
                // - Weapon: Sawed-Off Shotgun
                // - Weapon: Double Barrel Shotgun
                // With the following exceptions:
                // - Weapon: Stun Gun

                WeaponGroup group = Game.Player.Character.Weapons.Current.Group;
                WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
                bool isGroupValid = group == WeaponGroup.Pistol;
                bool isWeaponValid = hash == WeaponHash.MicroSMG || hash == WeaponHash.MachinePistol || hash == WeaponHash.MiniSMG ||
                    hash == WeaponHash.CompactGrenadeLauncher || hash == WeaponHash.SawnOffShotgun || hash == WeaponHash.DoubleBarrelShotgun ||
                    hash == WeaponHash.SweeperShotgun;
                bool isWeaponInvalid = hash == WeaponHash.StunGun;
                return (isGroupValid || isWeaponValid) && !isWeaponInvalid;
            }
        }
    }
}
