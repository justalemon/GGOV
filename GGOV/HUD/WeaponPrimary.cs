using GTA;

namespace GGO.HUD
{
    /// <summary>
    /// Represents the Primary Weapon Slot on the Player Fields.
    /// </summary>
    public sealed class WeaponPrimary : Weapon
    {
        /// <summary>
        /// If this weapon is valid for the primary slot.
        /// </summary>
        public override bool IsWeaponValid
        {
            get
            {
                // What is considered a Primary Weapon:
                // - Group: Assault Rifles
                // - Group: Machine Guns
                // - Group: SMG
                // - Group: Heavy
                // - Group: Shotguns
                // - Group: Sniper Rifles
                // - Weapon: Musket
                // With the following exceptions:
                // - Weapon: Micro SMG
                // - Weapon: Machine Pistol
                // - Weapon: Mini SMG
                // - Weapon: Compact Grenade Launcher
                // - Weapon: Sawed-Off Shotgun
                // - Weapon: Double Barrel Shotgun

                WeaponGroup group = Game.Player.Character.Weapons.Current.Group;
                WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
                bool isGroupValid = group == WeaponGroup.AssaultRifle || group == WeaponGroup.MG || group == WeaponGroup.SMG ||
                    group == WeaponGroup.Heavy || group == WeaponGroup.Shotgun || group == WeaponGroup.Sniper;
                bool isWeaponValid = hash == WeaponHash.Musket;
                bool isWeaponInvalid = hash == WeaponHash.MicroSMG || hash == WeaponHash.MachinePistol || hash == WeaponHash.MiniSMG ||
                    hash == WeaponHash.CompactGrenadeLauncher || hash == WeaponHash.SawnOffShotgun || hash == WeaponHash.DoubleBarrelShotgun ||
                    hash == WeaponHash.SweeperShotgun;
                return (isGroupValid || isWeaponValid) && !isWeaponInvalid;
            }
        }
    }
}
