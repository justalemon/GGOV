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

        /// <summary>
        /// Gets the image for the weapon caliber and cartridge.
        /// </summary>
        public static string GetAmmoImage(this Weapon PlayerWeapon)
        {
            switch (PlayerWeapon.Hash)
            {
                case WeaponHash.MarksmanPistol:
                    return "Ammo22Long";
                case WeaponHash.APPistol:
                    return "Ammo22Scamp";
                case WeaponHash.VintagePistol:
                    return "Ammo32ACP";
                case WeaponHash.DoubleActionRevolver:
                    return "Ammo38LongColt";
                case WeaponHash.Revolver:
                    return "Ammo44Magnum";
                case WeaponHash.HeavyPistol:
                case WeaponHash.SNSPistol:
                case WeaponHash.MicroSMG: // Model and Texture says ".45 ACB"
                case WeaponHash.Gusenberg:
                    return "Ammo45ACP";
                case WeaponHash.Pistol50:
                    return "Ammo50AE";
                case WeaponHash.HeavySniper:
                case WeaponHash.HeavySniperMk2:
                    return "Ammo50BMG";
                case WeaponHash.CarbineRifle:
                case WeaponHash.CarbineRifleMk2:
                case WeaponHash.SpecialCarbine:
                case WeaponHash.SpecialCarbineMk2:
                case WeaponHash.AdvancedRifle:
                    return "Ammo556NATO";
                case WeaponHash.SniperRifle:
                case WeaponHash.Minigun:
                case WeaponHash.CombatMG:
                case WeaponHash.CombatMGMk2:
                    return "Ammo762NATO";
                case WeaponHash.BullpupRifle:
                case WeaponHash.BullpupRifleMk2:
                case WeaponHash.CompactRifle:
                case WeaponHash.AssaultRifle:
                case WeaponHash.AssaultrifleMk2:
                case WeaponHash.MarksmanRifle:
                case WeaponHash.MarksmanRifleMk2:
                case WeaponHash.MG: // PKM is actually 7.62x54mmR instead of 7.62x39mm
                    return "Ammo762Soviet";
                case WeaponHash.CombatPistol:
                case WeaponHash.Pistol: // "9mm semiautomatic" in Complications
                case WeaponHash.PistolMk2:
                case WeaponHash.SMG: // MP5N
                case WeaponHash.SMGMk2: // MP5K
                case WeaponHash.MachinePistol:
                case WeaponHash.MiniSMG:
                case WeaponHash.CombatPDW:
                    return "Ammo9mm";
                case WeaponHash.SweeperShotgun:
                case WeaponHash.AssaultShotgun:
                case WeaponHash.BullpupShotgun:
                case WeaponHash.SawnOffShotgun:
                case WeaponHash.PumpShotgun:
                case WeaponHash.PumpShotgunMk2:
                case WeaponHash.HeavyShotgun:
                case WeaponHash.DoubleBarrelShotgun:
                    return "Ammo12Gauge";
                case WeaponHash.CompactGrenadeLauncher:
                case WeaponHash.GrenadeLauncher:
                    return "Ammo40mm";
                case WeaponHash.AssaultSMG:
                    return "AmmoFN57";
                case WeaponHash.RPG:
                    return "AmmoPG7VM";
                default:
                    return "AmmoUnknown";
            }
        }

        /// <summary>
        /// Gets the image for the weapon magazine.
        /// </summary>
        public static string GetMagazineImage(this Weapon PlayerWeapon)
        {
            switch (PlayerWeapon.Hash)
            {
                case WeaponHash.APPistol:
                case WeaponHash.CombatPistol:
                    return "MagBerettaPx4Storm";
                case WeaponHash.Pistol50:
                    return "MagDesertEagle50AE";
                case WeaponHash.HeavyPistol:
                    return "MagEAWB1911";
                case WeaponHash.SNSPistol:
                case WeaponHash.SNSPistolMk2:
                    return "MagHeckler&KochP7M10";
                case WeaponHash.Pistol:
                case WeaponHash.PistolMk2:
                    return "MagTaurusPT92AF";
                case WeaponHash.MarksmanPistol:
                    return "Ammo22Long"; // Is a single shot weapon
                case WeaponHash.VintagePistol:
                    return "MagFNModel1922";
                case WeaponHash.DoubleActionRevolver:
                case WeaponHash.Revolver:
                case WeaponHash.RevolverMk2:
                    return "MagCylinder";
                case WeaponHash.SweeperShotgun:
                    return "MagProMagSAI05Saiga";
                case WeaponHash.AssaultShotgun:
                case WeaponHash.HeavyShotgun:
                    return "MagATIOmniHybrid";
                case WeaponHash.BullpupShotgun:
                case WeaponHash.SawnOffShotgun:
                case WeaponHash.PumpShotgun:
                case WeaponHash.PumpShotgunMk2:
                case WeaponHash.DoubleBarrelShotgun:
                    return "Ammo12Gauge"; // They don't have magazines
                case WeaponHash.SMG:
                case WeaponHash.SMGMk2:
                    return "MagMP5";
                case WeaponHash.MicroSMG:
                    return "MagUzi";
                case WeaponHash.MachinePistol:
                    return "MagTec9";
                case WeaponHash.MiniSMG:
                    return "MagSkorpion";
                case WeaponHash.Gusenberg:
                    return "MagThompson";
                case WeaponHash.AssaultSMG:
                    return "MagFNP90"; // Is really from a Magpul PDR-C, but for GGO consistency...
                case WeaponHash.BullpupRifle:
                case WeaponHash.BullpupRifleMk2:
                case WeaponHash.CompactRifle:
                case WeaponHash.AssaultRifle:
                case WeaponHash.AssaultrifleMk2:
                    return "MagAK47";
                case WeaponHash.CarbineRifle:
                case WeaponHash.CarbineRifleMk2:
                case WeaponHash.AdvancedRifle:
                    return "MagAR15"; // They are similar enough
                case WeaponHash.MarksmanRifle:
                case WeaponHash.MarksmanRifleMk2:
                    return "MagAccuracyInternational"; // Same here
                case WeaponHash.HeavySniper:
                case WeaponHash.HeavySniperMk2:
                    return "MagBarrettM107";
                case WeaponHash.CompactGrenadeLauncher:
                    return "Ammo40mm";
                case WeaponHash.RPG:
                    return "AmmoPG7VM";
                default:
                    return "AmmoUnknown";
            }
        }
    }
}
