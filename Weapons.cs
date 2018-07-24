using GTA.Native;
using System.Collections.Generic;

namespace GGOHud
{
    class Weapons
    {
        // Weapons that can be considered sidearm/secondary
        public static readonly List<WeaponHash> Sidearms = new List<WeaponHash> {
            // Pistols
            WeaponHash.StunGun,
            WeaponHash.FlareGun,
            WeaponHash.SNSPistol,
            WeaponHash.SNSPistolMk2,
            WeaponHash.VintagePistol,
            WeaponHash.CombatPistol,
            WeaponHash.Pistol,
            WeaponHash.PistolMk2,
            WeaponHash.APPistol,
            WeaponHash.HeavyPistol,
            WeaponHash.Pistol50,
            WeaponHash.MarksmanPistol,
            WeaponHash.DoubleActionRevolver,
            WeaponHash.Revolver,
            WeaponHash.RevolverMk2,
            // Machine Guns
            WeaponHash.MicroSMG,
            WeaponHash.MiniSMG,
            WeaponHash.MachinePistol,
            // Shotguns
            WeaponHash.SawnOffShotgun,
            WeaponHash.SweeperShotgun,
            // Assault Rifles
            WeaponHash.CompactRifle,
            // Heavy Weapons
            WeaponHash.CompactGrenadeLauncher
        };
        // Weapons that are not going to be shown on the hud
        public static readonly List<WeaponHash> Hidden = new List<WeaponHash>
        {
            WeaponHash.Unarmed,
            WeaponHash.Flare,
            WeaponHash.Ball,
            WeaponHash.Snowball
        };
    }
}
