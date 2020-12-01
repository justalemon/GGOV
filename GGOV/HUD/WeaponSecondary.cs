using GTA;

namespace GGO.HUD
{
    /// <summary>
    /// Represents the Secondary Weapon Slot on the Player Fields.
    /// </summary>
    public sealed class WeaponSecondary : Weapon
    {
        /// <summary>
        /// If this weapon is valid for the Secondary slot.
        /// </summary>
        public override bool IsWeaponValid => Tools.GetWeaponType(Game.Player.Character.Weapons.Current.Hash) == WeaponType.Secondary;
    }
}
