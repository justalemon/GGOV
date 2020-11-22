using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GGO
{
    /// <summary>
    /// Script that handles the HUD Elements.
    /// </summary>
    public class HUD : Script
    {
        #region Private Fields

        internal static Preset selectedPreset = null;
        private readonly string location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
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
        private int nextPedUpdate = 0;

        #endregion

        #region Properties

        /// <summary>
        /// The names for the Ped Models.
        /// </summary>
        public static Dictionary<Model, string> Names { get; private set; } = new Dictionary<Model, string>();
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
            // Look for the names in GGOV/Names and load them
            foreach (string file in Directory.EnumerateFiles(Path.Combine(location, "GGOV", "Names")))
            {
                // If the file is not JSON, warn about it and skip it
                if (Path.GetExtension(file) != ".json")
                {
                    Notification.Show($"~o~Warning~s~: Non JSON file found in Names Directory! ({Path.GetFileName(file)})");
                    continue;
                }

                // Otherwise, try to load it
                string contents = File.ReadAllText(file);
                // And then to parse it
                Dictionary<string, string> loaded;
                // If we failed, notify the user and continue
                try
                {
                    loaded = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
                }
                catch (JsonSerializationException e)
                {
                    Notification.Show($"~r~Error~s~:Unable to load {Path.GetFileName(file)}: {e.Message}");
                    continue;
                }

                // Otherwise, add the names onto the list
                foreach (KeyValuePair<string, string> pair in loaded)
                {
                    // Convert the model from a number or a string
                    Model model;
                    if (int.TryParse(pair.Key, out int number))
                    {
                        model = new Model(number);
                    }
                    else
                    {
                        model = new Model(pair.Key);
                    }

                    // If the key is already on the dictionary, warn the user
                    if (Names.ContainsKey(model))
                    {
                        Notification.Show($"~o~Warning~s~: Model {pair.Key} ({model.Hash}) has more than one name!");
                    }
                    // Then, just add it to the list
                    Names[model] = pair.Value;
                }
            }

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
                List<Preset> foundPresets = JsonConvert.DeserializeObject<List<Preset>>(contents, new PresetConverter());
                foreach (Preset preset in foundPresets)
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
            // If a Ped update is required and we are not in a cutscene
            if ((nextPedUpdate <= Game.GameTime || nextPedUpdate == 0) && !Game.IsCutsceneActive)
            {
                // Iterate over the peds in the whole game world
                foreach (Ped ped in World.GetAllPeds())
                {
                    bool isFriend = Game.Player.Character.GetRelationshipWithPed(ped) <= Relationship.Like && ped.GetRelationshipWithPed(Game.Player.Character) <= Relationship.Like;
                    bool sameGroup = Game.Player.Character.PedGroup == ped.PedGroup;
                    bool groupLeader = ped.PedGroup?.Leader == Game.Player.Character;

                    // If the ped is a friend or is part of the player's group, is not the player, and is not part of the squad, add it
                    if ((isFriend || sameGroup || groupLeader) && ped != Game.Player.Character && !Squad.Contains(ped))
                    {
                        Squad.Add(new PedHealth(ped));
                    }
                }

                // Finally, set the new update time
                nextPedUpdate = Game.GameTime + 1000;
            }

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
                    Preset menu = new Preset(input);
                    presets.AddSubMenu(menu);
                    pool.Add(menu);
                }
                // If the user pressed Y/Triangle/Tab
                else if (Game.IsControlJustPressed(Control.FrontendY))
                {
                    // Get all of the presets in a list
                    List<Preset> fields = new List<Preset>();
                    pool.ForEach<Preset>(x => fields.Add(x));
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
