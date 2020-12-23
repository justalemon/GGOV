using GTA;

namespace GGO.HUD
{
    /// <summary>
    /// Represents the Secondary Weapon Slot on the Player Fields.
    /// </summary>
    public sealed class WeaponSecondary : Weapon
    {
        /// <summary>
        /// The hash of the Secondary weapon.
        /// </summary>
        public override WeaponHash Hash
        {
            get
            {
                if (GGO.menu.EquipWeapons.Checked)
                {
                    return GGO.weaponSecondary;
                }
                else
                {
                    return Game.Player.Character.Weapons.Current.Hash;
                }
            }
        }
        /// <summary>
        /// If this weapon is valid for the Secondary slot.
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
                    WeaponType type = Tools.GetWeaponType(Game.Player.Character.Weapons.Current.Hash);
                    return type == WeaponType.Secondary || type == WeaponType.Melee;
                }
            }
        }
    }
}
