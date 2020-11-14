using GTA;
using LemonUI;
using LemonUI.Extensions;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// The set of fields used for the player information.
    /// </summary>
    public sealed class PlayerFields : IProcessable, IRecalculable
    {
        #region Properties

        /// <summary>
        /// If the player fields should be visible or not.
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// The health of the player.
        /// </summary>
        public PedHealth Health { get; } = new PedHealth(Game.Player.Character, true, true);
        /// <summary>
        /// ThePprimary Weapon slot of the player.
        /// </summary>
        public Weapon PrimaryWeapon { get; } = new WeaponPrimary();
        /// <summary>
        /// The Secondary Weapon slot of the Player.
        /// </summary>
        public Weapon SecondaryWeapon { get; } = new WeaponSecondary();

        #endregion

        #region Constructor

        internal PlayerFields()
        {
            Recalculate();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the player fields.
        /// </summary>
        public void Recalculate()
        {
            float x = HUD.selectedPreset == null ? 388 : HUD.selectedPreset.PlayerX.SelectedItem;
            float y = HUD.selectedPreset == null ? 848 : HUD.selectedPreset.PlayerY.SelectedItem;

            PointF position = new PointF(1f.ToXAbsolute() - x, y);
            Health.Recalculate(position);
            PrimaryWeapon.Recalculate(new PointF(position.X, position.Y + 50 + 5));
            SecondaryWeapon.Recalculate(new PointF(position.X, position.Y + (50 * 2) + (5 * 2)));
        }
        /// <summary>
        /// Processes the player fields.
        /// </summary>
        public void Process()
        {
            Health.Process();
            PrimaryWeapon.Process();
            SecondaryWeapon.Process();
        }

        #endregion
    }
}
