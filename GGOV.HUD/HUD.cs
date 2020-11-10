using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;
using System;

namespace GGO
{
    /// <summary>
    /// Script that handles the HUD Elements.
    /// </summary>
    public class HUD : Script
    {
        #region Private Fields

        internal static PresetMenu selectedPreset = null;

        private readonly ObjectPool pool = new ObjectPool();
        private readonly NativeMenu menu = new NativeMenu("", "Gun Gale Online HUD Settings", "", null)
        {
            Alignment = Alignment.Right,
            ResetCursorWhenOpened = false
        };
        private readonly NativeMenu presets = new NativeMenu("", "Presets", "Presets allow you to store and apply different positions for the HUD Elements.", null)
        {
            Alignment = Alignment.Right,
            ResetCursorWhenOpened = false
        };

        #endregion

        #region Properties

        /// <summary>
        /// The Squad members panel.
        /// </summary>
        public static SquadMembers Squad { get; } = new SquadMembers();
        /// <summary>
        /// The fields with the information of the player.
        /// </summary>
        public static PlayerFields Player { get; } = new PlayerFields();

        #endregion

        #region Constructor

        public HUD()
        {
            // Build the menus
            presets.Buttons.Add(new InstructionalButton("Create New", Control.FrontendX));
            presets.Buttons.Add(new InstructionalButton("Mark as Active", Control.FrontendY));
            menu.AddSubMenu(presets);
            // Add the UI elements into the pool
            pool.Add(menu);
            pool.Add(presets);
            pool.Add(Squad);
            pool.Add(Player);
            // And add the tick event
            Tick += HUD_Tick;
        }

        #endregion

        #region Events

        private void HUD_Tick(object sender, EventArgs e)
        {
            // If the user entered ggohudconfig in the cheat input, open the menu
            if (Game.WasCheatStringJustEntered("ggohudconfig"))
            {
                menu.Visible = true;
            }

            // If the presets menu is visible, disable the controls that collide with X/Space and Y/Square
            if (presets.Visible)
            {
                DisableControlCollisions();
            }

            // Just process the HUD Elements
            pool.Process();

            // If the presets menu is still visible
            if (presets.Visible)
            {
                // Disable the colliding controls again
                DisableControlCollisions();
                // If the user pressed X/Square/Space
                if (Game.IsControlJustPressed(Control.FrontendX))
                {
                    // Ask the user for the name
                    presets.Visible = false;
                    string input = Game.GetUserInput(WindowTitle.EnterMessage60, "", 60);
                    presets.Visible = true;
                    // If the user didn't entered anything, return
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Notification.Show("~r~Error~s~: The Preset name is empty, is only whitespaces or it was cancelled.");
                        return;
                    }
                    // Otherwise, create a new preset
                    PresetMenu menu = new PresetMenu(input);
                    presets.AddSubMenu(menu);
                    pool.Add(menu);
                }
                // If the user pressed Y/Triangle/Tab
                else if (Game.IsControlJustPressed(Control.FrontendY))
                {
                    // Get the currently selected submenu
                    NativeSubmenuItem item = (NativeSubmenuItem)presets.SelectedItem;
                    // If is null, return
                    if (item == null)
                    {
                        return;
                    }
                    // Otherwise, set it as the active preset and recalculate the on screen elements
                    selectedPreset = (PresetMenu)item.Menu;
                    Player.Recalculate();
                    Squad.Recalculate();
                }
            }
        }

        private void DisableControlCollisions()
        {
            Game.DisableControlThisFrame(Control.VehicleExit);
            Game.DisableControlThisFrame(Control.Jump);
        }

        #endregion
    }
}
