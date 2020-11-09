using GTA;

namespace GGO
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
                WeaponGroup group = Game.Player.Character.Weapons.Current.Group;
                WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
                return hash != WeaponHash.Unarmed && group != WeaponGroup.Unarmed && group != WeaponGroup.Pistol;
            }
        }
    }
}
