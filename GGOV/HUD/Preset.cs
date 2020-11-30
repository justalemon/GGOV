using GGO.Items;
using GTA;
using LemonUI.Menus;
using LemonUI.Scaleform;
using System;

namespace GGO.HUD
{
    /// <summary>
    /// Represents the Preset used as a start position for the on screen elements.
    /// </summary>
    public sealed class Preset : NativeMenu
    {
        #region Properties

        internal FloatSelectorItem SquadX { get; } = new FloatSelectorItem("Squad Members: X", "The X value of the Squad Health Information.");
        internal FloatSelectorItem SquadY { get; } = new FloatSelectorItem("Squad Members: Y", "The Y value of the Squad Health Information.");
        internal FloatSelectorItem PlayerX { get; } = new FloatSelectorItem("Player Info: X", "The X value of the Player Information.");
        internal FloatSelectorItem PlayerY { get; } = new FloatSelectorItem("Player Info: Y", "The Y value of the Player Information.");
        internal NativeItem MarkAsActive { get; } = new NativeItem("Mark as Active", "Activates this HUD Preset.");

        #endregion

        #region Constructor

        internal Preset(string name) : this(0, 0, 0, 0, name)
        {
        }
        internal Preset(int squadX, int squadY, int playerX, int playerY, string name) : base("", name, "", null)
        {
            // Set the properties of the menu
            Alignment = GTA.UI.Alignment.Right;
            ResetCursorWhenOpened = false;
            // Add some innstructional buttons
            Buttons.Add(new InstructionalButton("Increment 10x", Control.FrontendX));
            Buttons.Add(new InstructionalButton("Increment 0.1x", Control.FrontendY));
            // Set the values of the sliders
            SquadX.SelectedItem = squadX;
            SquadY.SelectedItem = squadY;
            PlayerX.SelectedItem = playerX;
            PlayerY.SelectedItem = playerY;
            // Add the events
            SquadX.ItemChanged += (sender, e) => UpdateSquad();
            SquadY.ItemChanged += (sender, e) => UpdateSquad();
            PlayerX.ItemChanged += (sender, e) => UpdatePlayer();
            PlayerY.ItemChanged += (sender, e) => UpdatePlayer();
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
            GGO.selectedPreset = this;
            GGO.Player.Recalculate();
            GGO.Squad.Recalculate();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Updates the Squad Information.
        /// </summary>
        internal void UpdateSquad()
        {
            if (GGO.selectedPreset == this)
            {
                GGO.Squad.Recalculate();
            }
            UpdateText();
        }
        /// <summary>
        /// Updates the Player Information.
        /// </summary>
        internal void UpdatePlayer()
        {
            if (GGO.selectedPreset == this)
            {
                GGO.Player.Recalculate();
            }
            UpdateText();
        }
        /// <summary>
        /// Updates the Description based on the items.
        /// </summary>
        internal void UpdateText()
        {
            Description = $"Squad X: {SquadX.SelectedItem}~n~Squad Y: {SquadY.SelectedItem}~n~Player X: {PlayerX.SelectedItem}~n~Player Y: {PlayerY.SelectedItem}";
        }

        #endregion
    }
}
