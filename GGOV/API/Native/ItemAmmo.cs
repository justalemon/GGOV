using GGO.Extensions;
using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class ItemAmmo : Item
    {
        public override bool Visible => Game.Player.Character.Weapons.Current.IsAmmoAvailable();

        public override string Icon
        {
            get
                {
                switch (Game.Player.Character.Weapons.Current.Hash)
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
        }

        public override string Quantity => Game.Player.Character.Weapons.Current.Ammo.ToString();
    }
}
