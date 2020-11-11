using LemonUI.Menus;
using System;

namespace GGO
{
    /// <summary>
    /// Represents the Preset used as a start position for the on screen elements.
    /// </summary>
    public sealed class PresetMenu : NativeMenu
    {
        #region Properties

        internal NativeSliderItem SquadX { get; } = new NativeSliderItem("Squad Members: X", "The X value of the Squad Health Information.", 5000, 0);
        internal NativeSliderItem SquadY { get; } = new NativeSliderItem("Squad Members: Y", "The Y value of the Squad Health Information.", 1080, 0);
        internal NativeSliderItem PlayerX { get; } = new NativeSliderItem("Player Info: X", "The X value of the Player Information.", 5000, 0);
        internal NativeSliderItem PlayerY { get; } = new NativeSliderItem("Player Info: Y", "The Y value of the Player Information.", 1080, 0);
        internal NativeItem MarkAsActive { get; } = new NativeItem("Mark as Active", "Activates this HUD Preset.");

        #endregion

        #region Constructor

        internal PresetMenu(string name) : this(0, 0, 0, 0, name)
        {
        }
        internal PresetMenu(int squadX, int squadY, int playerX, int playerY, string name) : base("", name, "", null)
        {
            // Set the properties of the menu
            Alignment = GTA.UI.Alignment.Right;
            ResetCursorWhenOpened = false;
            // Set the values of the sliders
            SquadX.Value = squadX;
            SquadY.Value = squadY;
            PlayerX.Value = playerX;
            PlayerY.Value = playerY;
            // Add the events
            SquadX.ValueChanged += (sender, e) => UpdateSquad();
            SquadY.ValueChanged += (sender, e) => UpdateSquad();
            PlayerX.ValueChanged += (sender, e) => UpdatePlayer();
            PlayerY.ValueChanged += (sender, e) => UpdatePlayer();
            MarkAsActive.Activated += MarkAsActive_Activated;
            // And finally add the UI Elements
            Add(SquadX);
            Add(SquadY);
            Add(PlayerX);
            Add(PlayerY);
            Add(MarkAsActive);
        }

        #endregion

        #region Events

        private void MarkAsActive_Activated(object sender, EventArgs e)
        {
            HUD.selectedPreset = this;
            HUD.Player.Recalculate();
            HUD.Squad.Recalculate();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Updates the Squad Information.
        /// </summary>
        internal void UpdateSquad()
        {
            if (HUD.selectedPreset == this)
            {
                HUD.Squad.Recalculate();
            }
            UpdateText();
        }
        /// <summary>
        /// Updates the Player Information.
        /// </summary>
        internal void UpdatePlayer()
        {
            if (HUD.selectedPreset == this)
            {
                HUD.Player.Recalculate();
            }
            UpdateText();
        }
        /// <summary>
        /// Updates the Description based on the items.
        /// </summary>
        internal void UpdateText()
        {
            Description = $"Squad X: {SquadX.Value}~n~Squad Y: {SquadY.Value}~n~Player X: {PlayerX.Value}~n~Player Y: {PlayerY.Value}";
        }

        #endregion
    }
}
