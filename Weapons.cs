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
        /// Weapons that are not going to be shown on the HUD.
        /// See the coments on the code to know why they are considered banned.
        /// </summary>
        public static readonly List<WeaponHash> Banned = new List<WeaponHash>
        {
            // Your hands are not firearms
            WeaponHash.Unarmed,
            // Flares are used to mark positions
            WeaponHash.Flare,
            // Recreational does not count
            WeaponHash.Ball,
            // If you enable the snow in-game they can be found everywhere
            WeaponHash.Snowball,
            // Improvised weapons should not be included
            WeaponHash.Bottle,
            // Is a standard flashlight, nothing that can kill
            WeaponHash.Flashlight
        };

        /// <summary>
        /// Weapons that can be considered sidearm either by the size or firing mechanism.
        /// </summary>
        public static readonly List<WeaponHash> Sidearms = new List<WeaponHash> {
            // Melee
            WeaponHash.Knife,
            WeaponHash.Nightstick,
            WeaponHash.Hammer,
            WeaponHash.Bat,
            WeaponHash.Crowbar,
            WeaponHash.GolfClub,
            WeaponHash.Dagger,
            WeaponHash.BattleAxe,
            WeaponHash.KnuckleDuster,
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

        /// <summary>
        /// Weapons that can be considered melee.
        /// </summary>
        public static readonly List<WeaponHash> Melee = new List<WeaponHash> {
            WeaponHash.Knife,
            WeaponHash.Nightstick,
            WeaponHash.Hammer,
            WeaponHash.Bat,
            WeaponHash.Crowbar,
            WeaponHash.GolfClub,
            WeaponHash.Dagger,
            WeaponHash.BattleAxe,
            WeaponHash.KnuckleDuster,
        };

        /// <summary>
        /// Checks the weapon that the player currently has equiped.
        /// </summary>
        /// <returns>The Type of weapon.</returns>
        public static Type CurrentType()
        {
            // Store the hash of the current weapon
            WeaponHash CurrentWeapon = Game.Player.Character.Weapons.Current.Hash;

            // If the weapon is Banned
            if (Banned.Contains(CurrentWeapon))
                return Type.Banned;
            // If the weapon is not a firearm
            if (Melee.Contains(CurrentWeapon))
                return Type.Melee;
            // If the weapon is a small firearm
            if (Sidearms.Contains(CurrentWeapon))
                return Type.Sidearm;
            // If the weapon is other than the previous ones
            else
                return Type.Main;
        }
    }
}
