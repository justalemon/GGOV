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
        public override bool IsWeaponValid => Tools.GetWeaponType(Game.Player.Character.Weapons.Current.Hash) == WeaponType.Primary;
    }
}
