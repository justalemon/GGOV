﻿using GTA;
using LemonUI;
using LemonUI.Extensions;
using System.Drawing;
using GTA.UI;

namespace GGO.HUD
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
            float x = GGO.menu.PlayerX.SelectedItem;
            float y = GGO.menu.PlayerY.SelectedItem;

            PointF position = new PointF(1f.ToXAbsolute() + x, 1080 + y);
            Health.Recalculate(position);
            PrimaryWeapon.Recalculate(new PointF(position.X, position.Y + 50 + 5));
            SecondaryWeapon.Recalculate(new PointF(position.X, position.Y + (50 * 2) + (5 * 2)));
        }
        /// <summary>
        /// Processes the player fields.
        /// </summary>
        public void Process()
        {
            Hud.HideComponentThisFrame(HudComponent.AreaName);
            Hud.HideComponentThisFrame(HudComponent.StreetName);
            Hud.HideComponentThisFrame(HudComponent.VehicleName);

            Health.Process();
            PrimaryWeapon.Process();
            SecondaryWeapon.Process();
        }

        #endregion
    }
}
