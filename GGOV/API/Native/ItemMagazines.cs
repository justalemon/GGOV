using GGO.Extensions;
using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class ItemMagazines : Item
    {
        public override bool IsAvailable()
        {
            return Game.Player.Character.Weapons.Current.IsAmmoAvailable();
        }

        public override string GetQuantity()
        {
            if (Game.Player.Character.Weapons.Current.Ammo != 0 && Game.Player.Character.Weapons.Current.MaxAmmoInClip != 0)
            {
                return (Game.Player.Character.Weapons.Current.Ammo / Game.Player.Character.Weapons.Current.MaxAmmoInClip).ToString("0");
            }
            else
            {
                return "0";
            }
        }

        public override string GetIcon()
        {
            switch (Game.Player.Character.Weapons.Current.Hash)
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
                    return "MagHecklerKochP7M10";
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
