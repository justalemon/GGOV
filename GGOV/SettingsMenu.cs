using GGO.Items;
using GTA.UI;
using LemonUI.Menus;
using System;
using System.IO;

namespace GGO
{
    /// <summary>
    /// The Main Menu of the Settings.
    /// </summary>
    public class SettingsMenu : NativeMenu
    {
        #region Properties

        public NativeCheckboxItem EquipWeapons { get; } = new NativeCheckboxItem("Equip Inventory Weapons", "Enabled: Marks the Weapons as Primary or Secondary, allowing you to switch between your Primary, Secondary and Fists with 1, 2 and 3 respectively (like Apex Legends).~n~~n~Disabled: Equips the Weapons directly. Requires you to open the Inventory every time that you want to change weapons.", true);
        public FloatSelectorItem SquadX { get; } = new FloatSelectorItem("Squad Members: X", "The X value of the Squad Health Information.", 103);
        public FloatSelectorItem SquadY { get; } = new FloatSelectorItem("Squad Members: Y", "The Y value of the Squad Health Information.", 66);
        public FloatSelectorItem PlayerX { get; } = new FloatSelectorItem("Player Info: X", "The X value of the Player Information.", -388);
        public FloatSelectorItem PlayerY { get; } = new FloatSelectorItem("Player Info: Y", "The Y value of the Player Information.", -232);
        public NativeItem MarkAsActive { get; } = new NativeItem("Mark as Active", "Activates this HUD Preset.");

        #endregion

        #region Constructor

        public SettingsMenu() : base("", "Gun Gale Online Settings", "", null)
        {
            // Set the options of the menu
            Alignment = Alignment.Right;
            ResetCursorWhenOpened = false;
            // Add the items and submenus
            Add(EquipWeapons);
            Add(SquadX);
            Add(SquadY);
            Add(PlayerX);
            Add(PlayerY);
            // Subscribe the events
            EquipWeapons.CheckboxChanged += EquipWeapons_CheckboxChanged;
            SquadX.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            SquadY.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            PlayerX.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            PlayerY.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
        }

        #endregion

        #region Events

        private void EquipWeapons_CheckboxChanged(object sender, EventArgs e)
        {
            // If the feature was disabled, clear the current weapons
            if (!EquipWeapons.Checked)
            {
                GGO.weaponPrimary = 0;
                GGO.weaponSecondary = 0;
            }
        }

        #endregion
    }
}
