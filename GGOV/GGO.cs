using GGO.HUD;
using GGO.Inventory;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using Newtonsoft.Json;
using PlayerCompanion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace GGO
{
    /// <summary>
    /// Main script for the entire GGO HUD System.
    /// </summary>
    public class GGO : Script
    {
        #region Fields

        /// <summary>
        /// The location of this script.
        /// </summary>
        internal static readonly string location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        /// <summary>
        /// The Pool that holds all of the Processable items.
        /// </summary>
        internal static readonly ObjectPool pool = new ObjectPool();

        /// <summary>
        /// The current Primary Weapon.
        /// </summary>
        internal static WeaponHash weaponPrimary = 0;
        /// <summary>
        /// The current Secondary Weapon.
        /// </summary>
        internal static WeaponHash weaponSecondary = 0;

        /// <summary>
        /// The main configuration menu.
        /// </summary>
        internal static readonly SettingsMenu menu = new SettingsMenu();

        /// <summary>
        /// The inventory of the user.
        /// </summary>
        internal static readonly PlayerInventory inventory = new PlayerInventory();

        /// <summary>
        /// The currently known peds.
        /// </summary>
        private Ped[] peds = new Ped[0];
        /// <summary>
        /// The currently active Death Markers.
        /// </summary>
        private readonly Dictionary<Ped, ScaledTexture> markers = new Dictionary<Ped, ScaledTexture>();
        /// <summary>
        /// The next time for the Marker update.
        /// </summary>
        private int nextMarkerUpdate = 0;

        /// <summary>
        /// The next time for updating the Squad Members.
        /// </summary>
        private int nextSquadUpdate = 0;

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

        public GGO()
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

            // Add the UI elements into the pool
            pool.Add(menu);
            pool.Add(inventory);
            pool.Add(Squad);
            pool.Add(Player);
            // And add the tick event
            Tick += HUD_Tick;

            // And create the events for item updates
            Companion.Inventories.ItemAdded += (sender, e) => inventory.UpdateItems();
            Companion.Inventories.ItemRemoved += (sender, e) => inventory.UpdateItems();
        }

        #endregion

        #region Events

        private void HUD_Tick(object sender, EventArgs e)
        {
            // Make the inventory visible based on the button pressed
            Game.DisableControlThisFrame(Control.SelectWeapon);
            if (Game.IsControlJustPressed(Control.SelectWeapon))
            {
                inventory.Visible = !inventory.Visible;
            }

            // If a Ped update is required and we are not in a cutscene
            if ((nextSquadUpdate <= Game.GameTime || nextSquadUpdate == 0) && !Game.IsCutsceneActive)
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
                nextSquadUpdate = Game.GameTime + 1000;
            }

            // If the user entered ggohudconfig in the cheat input, open the menu
            if (Game.WasCheatStringJustEntered("ggo"))
            {
                menu.Visible = true;
            }

            // If the current weapon used by the player is not the primary or secondary and is not unarmed
            WeaponHash current = Game.Player.Character.Weapons.Current.Hash;
            if ((current != weaponPrimary || current != weaponSecondary) && current != WeaponHash.Unarmed)
            {
                // Check for the group and switch it if required
                switch (Tools.GetWeaponType(current))
                {
                    case WeaponType.Primary:
                        weaponPrimary = current;
                        inventory.UpdateWeapons();
                        Player.PrimaryWeapon.ShiftImage(false, true);
                        break;
                    case WeaponType.Secondary:
                        weaponSecondary = current;
                        inventory.UpdateWeapons();
                        Player.SecondaryWeapon.ShiftImage(false, true);
                        break;
                    case WeaponType.Melee:
                        weaponSecondary = current;
                        inventory.UpdateWeapons();
                        Player.SecondaryWeapon.ShiftImage(true, true);
                        break;
                }
            }

            // If the player does not has either the primary or secondary weapons, clear them out
            if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, weaponPrimary, false))
            {
                weaponPrimary = 0;
            }
            if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, weaponSecondary, false))
            {
                weaponSecondary = 0;
            }

            // Just process the HUD Elements
            pool.Process();

            // If the user pressed 1, 2 or 3 with the equip mode enabled, change the weapon
            if (menu.EquipWeapons.Checked)
            {
                if (Game.IsControlJustPressed(Control.SelectWeaponUnarmed))
                {
                    Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weaponPrimary, false);
                }
                if (Game.IsControlJustPressed(Control.SelectWeaponMelee))
                {
                    Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weaponSecondary, false);
                }
                if (Game.IsControlJustPressed(Control.SelectWeaponShotgun))
                {
                    Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, WeaponHash.Unarmed, false);
                }
            }

            // Finally, update the markers
            UpdateMarkers();
        }

        private void UpdateMarkers()
        {
            // Update the peds if is time to do so
            if (nextMarkerUpdate <= Game.GameTime)
            {
                peds = World.GetAllPeds();
                nextMarkerUpdate = Game.GameTime + 1000;
            }

            // Iterate over the peds in the list
            foreach (Ped ped in peds)
            {
                // If the ped is alive or no longer exists and is on the list, remove it
                if ((ped.IsAlive || !ped.Exists()) && markers.ContainsKey(ped))
                {
                    markers.Remove(ped);
                }
                // If the ped is dead and is not part of the markers, add it
                else if (ped.IsDead && !markers.ContainsKey(ped))
                {
                    markers.Add(ped, new ScaledTexture(PointF.Empty, new SizeF(220 * 0.75f, 124 * 0.75f), "ggo", "marker_dead"));
                }
            }

            // Iterate over the existing items
            foreach (KeyValuePair<Ped, ScaledTexture> marker in markers)
            {
                Ped ped = marker.Key;

                // If the ped is not on the screen, skip it
                if (!ped.IsOnScreen)
                {
                    continue;
                }

                // Get the position of the ped head
                Vector3 headPos = ped.Bones[Bone.SkelHead].Position;

                // And then conver it to screen coordinates
                OutputArgument originalX = new OutputArgument();
                OutputArgument originalY = new OutputArgument();
                bool ok = Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, headPos.X, headPos.Y, headPos.Z, originalX, originalY);

                // If it was unable to get the position, continue
                if (!ok)
                {
                    continue;
                }

                // Otherwise, convert the position from relative to absolute
                PointF screenPos = new PointF(originalX.GetResult<float>(), originalY.GetResult<float>()).ToAbsolute();
                // And set it for the correct
                marker.Value.Position = new PointF(screenPos.X - (marker.Value.Size.Width * 0.5f), screenPos.Y - marker.Value.Size.Height);

                // Finally, draw it
                marker.Value.Draw();
            }
        }

        #endregion
    }
}
