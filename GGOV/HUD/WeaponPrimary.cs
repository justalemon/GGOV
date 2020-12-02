using GTA;

namespace GGO.HUD
{
    /// <summary>
    /// Represents the Primary Weapon Slot on the Player Fields.
    /// </summary>
    public sealed class WeaponPrimary : Weapon
    {
        /// <summary>
        /// The hash of the Primary weapon.
        /// </summary>
        public override WeaponHash Hash
        {
            get
            {
                if (GGO.menu.EquipWeapons.Checked)
                {
                    return GGO.weaponPrimary;
                }
                else
                {
                    return Game.Player.Character.Weapons.Current.Hash;
                }
            }
        }
        /// <summary>
        /// If this weapon is valid for the Primary slot.
        /// </summary>
        public override bool IsWeaponValid
        {
            get
            {
                if (GGO.menu.EquipWeapons.Checked)
                {
                    return base.IsWeaponValid;
                }
                else
                {
                    return Tools.GetWeaponType(Game.Player.Character.Weapons.Current.Hash) == WeaponType.Primary;
                }
            }
        }
    }
}
