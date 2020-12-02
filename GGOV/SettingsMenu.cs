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

        #endregion

        #region Constructor

        public SettingsMenu() : base("", "Gun Gale Online Settings", "", null)
        {
            // Set the options of the menu
            Alignment = Alignment.Right;
            ResetCursorWhenOpened = false;
            // Subscribe the events
            DisableActivePreset.Activated += DisableActivePreset_Activated;
            // And add the items and submenus
            Add(DisableActivePreset);
            AddSubMenu(Presets);
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

        #endregion
    }
}
