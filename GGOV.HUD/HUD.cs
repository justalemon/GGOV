using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
            presets.Buttons.Add(new InstructionalButton("Save Presets", Control.FrontendY));
            menu.AddSubMenu(presets);
            // Add the UI elements into the pool
            pool.Add(menu);
            pool.Add(presets);
            pool.Add(Squad);
            pool.Add(Player);
            // And add the tick event
            Tick += HUD_Tick;

            // Once everything is loaded, load the presets if they are present
            if (File.Exists("scripts\\GGOV\\HUDPresets.json"))
            {
                string contents = File.ReadAllText("scripts\\GGOV\\HUDPresets.json");
                List<PresetMenu> foundPresets = JsonConvert.DeserializeObject<List<PresetMenu>>(contents, new PresetConverter());
                foreach (PresetMenu preset in foundPresets)
                {
                    pool.Add(preset);
                    presets.AddSubMenu(preset);
                }
            }
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
                    // Get all of the presets in a list
                    List<PresetMenu> fields = new List<PresetMenu>();
                    pool.ForEach<PresetMenu>(x => fields.Add(x));
                    // Convert them to JSON
                    string json = JsonConvert.SerializeObject(fields, new PresetConverter());
                    // And write them to a file
                    Directory.CreateDirectory("scripts\\GGOV");
                    File.WriteAllText("scripts\\GGOV\\HUDPresets.json", json);

                    Notification.Show("The Presets have been ~g~Saved~s~!");
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
