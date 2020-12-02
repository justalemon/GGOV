using GTA.UI;
using LemonUI.Menus;
using System;

namespace GGO
{
    /// <summary>
    /// The Main Menu of the Settings.
    /// </summary>
    public class SettingsMenu : NativeMenu
    {
        #region Properties

        /// <summary>
        /// Item used to Disable the current Active Preset.
        /// </summary>
        public NativeItem DisableActivePreset { get; } = new NativeItem("Disable Active Preset", "Disables the Active HUD Preset.");
        /// <summary>
        /// The menu that manages the HUD Presets.
        /// </summary>
        public NativeMenu Presets { get; } = new NativeMenu("", "HUD Presets", "Presets allow you to store and apply different positions for the HUD Elements.", null)
        {
            Alignment = Alignment.Right,
            ResetCursorWhenOpened = false
        };
        /// <summary>
        /// Item to toggle between Equipping and Switching weapons on the inventory.
        /// </summary>
        public NativeCheckboxItem EquipWeapons { get; } = new NativeCheckboxItem("Equip Inventory Weapons", "Enabled: Marks the Weapons as Primary or Secondary, allowing you to switch between your Primary, Secondary and Fists with 1, 2 and 3 respectively (like Apex Legends).~n~~n~Disabled: Equips the Weapons directly. Requires you to open the Inventory every time that you want to change weapons.", true);

        #endregion

        #region Constructor

        public SettingsMenu() : base("", "Gun Gale Online Settings", "", null)
        {
            // Set the options of the menu
            Alignment = Alignment.Right;
            ResetCursorWhenOpened = false;
            // Subscribe the events
            DisableActivePreset.Activated += DisableActivePreset_Activated;
            EquipWeapons.CheckboxChanged += EquipWeapons_CheckboxChanged;
            // And add the items and submenus
            Add(DisableActivePreset);
            AddSubMenu(Presets);
            Add(EquipWeapons);
        }

        #endregion

        #region Events

        private void DisableActivePreset_Activated(object sender, EventArgs e)
        {
            if (GGO.selectedPreset == null)
            {
                Notification.Show("There is no HUD Preset active.");
            }
            else
            {
                GGO.selectedPreset = null;
                GGO.Squad.Recalculate();
                GGO.Player.Recalculate();
                Notification.Show("The Active Preset has been Disabled. The HUD now uses the Default Values.");
            }
        }

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
