using GGO.API;
using GGO.API.Native;
using GGO.Extensions;
using GGO.UserData;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using static GGO.Tools;

namespace GGO
{
    public class GGO : Script
    {
        #region Configurations

        /// <summary>
        /// Configuration for the HUD.
        /// </summary>
        private static Hud HudConfig = JsonConvert.DeserializeObject<Hud>(File.ReadAllText("scripts\\GGO\\Hud.json"));
        /// <summary>
        /// Configuration for the inventory.
        /// </summary>
        private Inventory InventoryConfig = JsonConvert.DeserializeObject<Inventory>(File.ReadAllText("scripts\\GGO\\Inventory.json"));
        /// <summary>
        /// Names for the peds on the squad section.
        /// </summary>
        public static Dictionary<string, string> Names = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("scripts\\GGO\\Names.json"));

        #endregion

        #region Custom Fields
        
        /// <summary>
        /// The list of squad fields.
        /// </summary>
        private static List<IField> SquadFields = new List<IField>();
        /// <summary>
        /// The list of player fields.
        /// </summary>
        private static List<IField> PlayerFields = new List<IField>();
        /// <summary>
        /// The list of inventory items.
        /// </summary>
        private static List<IItem> InventoryItems = new List<IItem>();
        /// <summary>
        /// If the squad side is running on the exclusive mode.
        /// </summary>
        private static bool Exclusive = false;
        /// <summary>
        /// The Script that is using the exclusive mode.
        /// </summary>
        private static string Script = null;

        #endregion

        #region Positions

        /// <summary>
        /// Positions of the items inside of the inventory.
        /// </summary>
        private static List<Point> ItemsPositions = new List<Point>();
        /// <summary>
        /// Positions of the weapons inside of the inventory.
        /// </summary>
        private static List<Point> WeaponPositions = new List<Point>();

        #endregion

        #region Peds and Weapons

        /// <summary>
        /// Next game time that we should update the lists of peds.
        /// </summary>
        private static int NextFetch = 0;
        /// <summary>
        /// List of peds that are near the player.
        /// </summary>
        private static List<Ped> NearbyPeds = new List<Ped>();
        /// <summary>
        /// List of peds that are friendly and part of the squad.
        /// </summary>
        private static List<Ped> FriendlyPeds = new List<Ped>();
        /// <summary>
        /// Next time to check if the user has a weapon that is not on the inventory.
        /// </summary>
        private int NextWeaponCheck = 0;

        #endregion

        #region Constructors

        public GGO()
        {
            // If the user has enabled the HUD
            if (HudConfig.Enabled)
            {
                // Add the player fields
                PlayerFields.Add(new PlayerSecondary());
                PlayerFields.Add(new PlayerPrimary());
                PlayerFields.Add(new PlayerHealth());
                if (HudConfig.VehicleInfo) { PlayerFields.Add(new PlayerVehicle()); }

                // Add our Tick and Aborted events
                Tick += OnTickHud;
                Aborted += OnAbort;
            }

            // Don't do nothing if the user requested the menu to be disabled
            if (InventoryConfig.Enabled)
            {
                // Add the inventory fields
                InventoryItems.Add(new ItemAmmo());
                InventoryItems.Add(new ItemMagazines());

                // Add the weapon fields
                foreach (WeaponHash StoredHash in InventoryConfig.Items)
                {
                    InventoryItems.Add(new ItemWeapon(StoredHash));
                }

                // Itterate between 0-2 (1-3) and 0-4 (1-5) and create the item positions
                for (int Y = 0; Y < 5; Y++)
                {
                    for (int X = 0; X < 3; X++)
                    {
                        // And generate the item positions
                        ItemsPositions.Add(new Point((int)(UI.WIDTH * InventoryConfig.ItemsX) + ((int)(UI.HEIGHT * InventoryConfig.SpacingX) * X) + ((int)(UI.HEIGHT * InventoryConfig.ItemsWidth) * X),
                                                    (int)(UI.HEIGHT * InventoryConfig.ItemsY) + ((int)(UI.HEIGHT * InventoryConfig.SpacingY) * Y)));
                    }
                }
                // Iterate between 0-4 (1-5) and create the weapon positions
                for (int Index = 0; Index < 5; Index++)
                {
                    // And add a weapon on that position
                    // Formula for Y: VerticalPosition + (SeparationBetweenWeapons * WeaponNumber)
                    WeaponPositions.Add(new Point((int)(UI.WIDTH * InventoryConfig.WeaponX), (int)(UI.HEIGHT * InventoryConfig.WeaponY) + ((int)(UI.HEIGHT * InventoryConfig.SpacingY) * Index)));
                }

                // Add the events
                Tick += OnTickGiveWeapons;
                Tick += OnTickRemoveWeapons;
                Tick += OnTickInventory;
            }
        }

        #endregion

        #region API

        public static bool AddField(IField CustomField, FieldSection Destination)
        {
            // If the exclusive mode is enabled and the script does not matches
            if (Exclusive && Destination == FieldSection.Squad && Script != Assembly.GetCallingAssembly().ManifestModule.ScopeName)
            {
                return false;
            }

            // Notify the user about what we are going to do
            UI.Notify("The script " + Assembly.GetCallingAssembly().ManifestModule.ScopeName + " has added a new Field.");

            // Then, add the field to their respective list
            switch (Destination)
            {
                case FieldSection.Player:
                    PlayerFields.Add(CustomField);
                    break;
                case FieldSection.Squad:
                    SquadFields.Add(CustomField);
                    break;
                default:
                    throw new InvalidOperationException("This field type is not supported.");
            }

            // Finally, return true
            return true;
        }

        public static bool AddItem(IItem CustomItem)
        {
            // If the inventory count is over the limit and the exclusive script does not matches
            if (InventoryItems.Count > 15)
            {
                return false;
            }

            // Add the item and return
            InventoryItems.Add(CustomItem);
            return true;
        }

        public static bool RequestExclusiveMode(bool Force = false)
        {
            // If the exclusive mode is enabled and the user does not wants to force it
            if (Exclusive && !Force)
            {
                return false;
            }

            // Clear the existing squad fields
            SquadFields.Clear();
            // Get the name of the next assembly and store it
            Script = Assembly.GetCallingAssembly().ManifestModule.ScopeName;
            // Set the squad section as exclusive for that script
            Exclusive = true;
            // Return a success
            return true;
        }

        public static bool DisableExclusiveMode(bool Force = true)
        {
            // If the disable should not be forced and the script does not matches
            if (!Force && Script != Assembly.GetCallingAssembly().ManifestModule.ScopeName)
            {
                return false;
            }

            // Remove thev custom fields
            SquadFields.Clear();
            // Set the exclusive mode to false and remove the stored script
            Exclusive = false;
            Script = null;
            // Finally, return
            return true;
        }

        #endregion

        #region Events

        private void OnTickHud(object Sender, EventArgs Args)
        {
            // Don't draw the UI if the game is loading, paused, player is dead or it cannot be controlled
            if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive || !Game.Player.CanControlCharacter)
            {
                return;
            }

            // Reset the index of the images
            ResetImageIndex();

            // Disable the colliding HUD elements by default
            if (!HudConfig.Collisions)
            {
                UI.HideHudComponentThisFrame(HudComponent.WeaponIcon);
                UI.HideHudComponentThisFrame(HudComponent.AreaName);
                UI.HideHudComponentThisFrame(HudComponent.StreetName);
                UI.HideHudComponentThisFrame(HudComponent.VehicleName);
                UI.HideHudComponentThisFrame(HudComponent.HelpText);
            }

            // If the user wants to disable the Radar and is not hidden, do it now
            if (!HudConfig.Radar && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
            {
                Function.Call(Hash.DISPLAY_RADAR, false);
            }

            // Get the interior where the player is
            int Interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character);

            // Draw get the ped information only if the squad members or dead markers have been enabled
            if (HudConfig.Squad || HudConfig.DeadMarkers)
            {
                // If the current time is higher or equal than the next fetch time
                if (Game.GameTime >= NextFetch)
                {
                    // Get all of the peds and order them by hash
                    NearbyPeds = World.GetAllPeds().OrderBy(P => P.GetHashCode()).ToList();
                    // THen, filter the friendly squad peds
                    FriendlyPeds = NearbyPeds.Where(P => P.IsFriendly() && P.IsMissionEntity()).ToList();

                    // Finally, set the next fetch time to one second in the future
                    NextFetch = Game.GameTime + 1000;
                }

                // If the user wants to, draw the dead markers
                if (HudConfig.DeadMarkers)
                {
                    // Iterate over all the peds
                    foreach (Ped DeadPed in NearbyPeds)
                    {
                        // If the ped is null, does not exists, is alive or is not on screen, skip it
                        if (DeadPed == null || !DeadPed.Exists() || DeadPed.IsAlive || !DeadPed.IsOnScreen)
                        {
                            continue;
                        }

                        // And draw the dead marker
                        DeadMarker(DeadPed);
                    }
                }
            }
            
            // If the squad members are enabled
            if (HudConfig.Squad)
            {
                // Store a number of the skipped peds
                int SquadSkipped = 0;

                // Iterate over the squad members
                for (int i = 0; i < SquadFields.Count + FriendlyPeds.Count; i++)
                {
                    // Set a place for the member
                    IBase Member;

                    // Get the ped on that position
                    if (SquadFields.Count != 0 && i <= SquadFields.Count - 1)
                    {
                        // See if we should show the specified field
                        if (!SquadFields[i].Visible)
                        {
                            SquadSkipped += 1;
                            continue;
                        }

                        // Add the field
                        Member = SquadFields[i];
                    }
                    else
                    {
                        // Store the selected ped
                        Ped SelectedPed = FriendlyPeds[i - SquadFields.Count];

                        // Skip non-existant peds and those inside of the night club (if enabled) and increase the count of invalid
                        if (Exclusive || SelectedPed == null || !SelectedPed.Exists() || (HudConfig.ClubFix && Interior == 271617 && !SelectedPed.IsPlayer))
                        {
                            SquadSkipped += 1;
                            continue;
                        }

                        Member = new SquadMember(SelectedPed);
                    }

                    // Draw the icon and the ped info for it
                    PlayerField(Member, i - SquadSkipped, FieldSection.Squad);
                }
            }

            // Count the items that should not be drawn
            int PlayerSkipped = 0;
            // Iterate over the count of fields
            for (int i = 0; i < PlayerFields.Count; i++)
            {
                // If the field should not be shown
                if (!PlayerFields[i].Visible)
                {
                    // Add one more and skip the iteration
                    PlayerSkipped += 1;
                    continue;
                }
                // Then, draw the specified field
                PlayerField(PlayerFields[i], i - PlayerSkipped, FieldSection.Player);
            }
        }

        /// <summary>
        /// Gives the startup weapons to the player, and gets unloaded once we are done.
        /// </summary>
        private void OnTickGiveWeapons(object Sender, EventArgs Args)
        {
            // If the player disabled the option
            if (!InventoryConfig.AutoAdd)
            {
                // Unsubscribe the event
                Tick -= OnTickGiveWeapons;
            }

            // If the game is loading
            if (Game.IsLoading)
            {
                return;
            }

            // Remove all of the weapons
            Game.Player.Character.Weapons.RemoveAll();

            // And add the items and weapons, one by one with the max ammo
            foreach (WeaponHash Item in InventoryConfig.Items)
            {
                Game.Player.Character.Weapons.Give(Item, 9999, true, false);
            }
            foreach (WeaponHash Weapon in InventoryConfig.Weapons)
            {
                Game.Player.Character.Weapons.Give(Weapon, 9999, true, false);
            }

            // Finally, unsubscribe the event
            Tick -= OnTickGiveWeapons;
        }

        /// <summary>
        /// Removes the player weapons that are not on the items or weapons.
        /// </summary>
        private void OnTickRemoveWeapons(object Sender, EventArgs Args)
        {
            // If the user has this option disabled, remove the event.
            if (!InventoryConfig.RemvoveNonListed)
            {
                Tick -= OnTickRemoveWeapons;
            }

            // If the time is higher or equal than the next check
            if (Game.GameTime >= NextWeaponCheck)
            {
                // If the current player weapon is not on the inventory
                if (!InventoryConfig.Items.Contains(Game.Player.Character.Weapons.Current.Hash) && !InventoryConfig.Weapons.Contains(Game.Player.Character.Weapons.Current.Hash))
                {
                    // Remove the weapon
                    Game.Player.Character.Weapons.Remove(Game.Player.Character.Weapons.Current.Hash);
                }

                // And set the next check to one second in the future
                NextWeaponCheck = Game.GameTime + 1000;
            }
        }

        /// <summary>
        /// Tick that handles the drawing and actions of the inventory.
        /// </summary>
        private void OnTickInventory(object Sender, EventArgs Args)
        {
            // Don't process the inventory when using a controller and in a vehicle
            if (Game.CurrentInputMode == InputMode.GamePad && Game.Player.Character.IsInVehicle())
            {
                return;
            }

            // Disable the weapon wheel
            Game.DisableControlThisFrame(0, Control.SelectWeapon);
            // If the user just pressed TAB/L1/LB
            if (Game.IsDisabledControlJustPressed(0, Control.SelectWeapon))
            {
                // Center the cursor on the screen
                bool OK = Function.Call<bool>(Hash._0xFC695459D4D0E219, 0.5f, 0.5f); // _SET_CURSOR_POSTION
                // If it was not possible, log it
                if (!OK)
                {
                    Logging.Error("Unable to set the cursor on the center of the screen.");
                }
            }

            // Draw the inventory if the player is keeping the finger on TAB/L1/LB
            if (Game.IsDisabledControlPressed(0, Control.SelectWeapon))
            {
                // Draw the inventory
                DrawInventory();
                // Show the cursor during this frame
                Function.Call(Hash._SHOW_CURSOR_THIS_FRAME);
                // Disable the fire, aim and camera controls
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
                Game.DisableControlThisFrame(0, Control.Aim);
                Game.DisableControlThisFrame(0, Control.LookUpDown);
                Game.DisableControlThisFrame(0, Control.LookLeftRight);
            }
        }

        private void OnAbort(object Sender, EventArgs Args)
        {
            // Reset the Radar state to enabled (just if the script is aborted but not started again)
            Function.Call(Hash.DISPLAY_RADAR, true);
        }

        #endregion

        #region Drawing Tools

        /// <summary>
        /// Draws the Inventory on screen.
        /// </summary>
        private void DrawInventory()
        {
            // Get the current and max health and calculate the size of the health bar
            float HealthMaxN = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
            float HealthCurrentN = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
            float HealthWidth = HealthCurrentN / HealthMaxN * 100 / 100 * (UI.WIDTH * InventoryConfig.HealthWidth);
            Size HealthSize = new Size((int)HealthWidth, (int)(UI.HEIGHT * InventoryConfig.HealthHeight));

            // Generate the information for the Rectangles and Texts and draw them on the screen
            new UIRectangle(LiteralPoint(InventoryConfig.BackgroundX, InventoryConfig.BackgroundY), LiteralSize(InventoryConfig.BackgroundWidth, InventoryConfig.BackgroundHeight), Colors.Inventory).Draw();
            new UIRectangle(LiteralPoint(InventoryConfig.BackgroundX, InventoryConfig.BackgroundY), LiteralSize(InventoryConfig.InfoWidth, InventoryConfig.InfoHeight), Colors.Backgrounds).Draw();
            new UIRectangle(LiteralPoint(InventoryConfig.PlayerX, InventoryConfig.PlayerY), LiteralSize(InventoryConfig.PlayerWidth, InventoryConfig.PlayerHeight), Colors.Details).Draw();
            new UIRectangle(LiteralPoint(InventoryConfig.HealthX, InventoryConfig.HealthY), LiteralSize(InventoryConfig.HealthWidth, InventoryConfig.HealthHeight), Color.Gray).Draw();
            new UIRectangle(LiteralPoint(InventoryConfig.HealthX, InventoryConfig.HealthY), HealthSize, Color.White).Draw();
            new UIText(Game.Player.Name, LiteralPoint(InventoryConfig.NameX, InventoryConfig.NameY), 0.7f, Color.White, GTA.Font.Monospace, false, false, false).Draw();
            new UIText("Life", LiteralPoint(InventoryConfig.LifeX, InventoryConfig.LifeY), 0.3f, Color.White, GTA.Font.ChaletLondon, false, false, false).Draw();
            new UIText("Items", LiteralPoint(InventoryConfig.ItemsX, InventoryConfig.ItemsY) + LiteralSize(InventoryConfig.TextX, InventoryConfig.TextY), 0.3f, Color.White, GTA.Font.ChaletLondon, false).Draw();
            new UIText("Arms", LiteralPoint(InventoryConfig.WeaponX, InventoryConfig.WeaponY) + LiteralSize(InventoryConfig.TextX, InventoryConfig.TextY), 0.3f, Color.White, GTA.Font.ChaletLondon, false).Draw();

            // Draw the gender image
            DrawImage(InventoryConfig.PlayerGender == Gender.Male ? "GenderMale" : "GenderFemale", LiteralPoint(InventoryConfig.GenderX, InventoryConfig.GenderY), LiteralSize(InventoryConfig.GenderWidth, InventoryConfig.GenderHeight));

            // Draw the player status
            new UIText("Status", LiteralPoint(InventoryConfig.StatusBaseX, InventoryConfig.StatusY), 0.38f, Color.White, GTA.Font.ChaletLondon, false).Draw();
            new UIText(Game.Player.GetState(), LiteralPoint(InventoryConfig.StatusCurrentX, InventoryConfig.StatusY), 0.38f, Color.White, GTA.Font.ChaletLondon, true).Draw();

            // Draw the item backgrounds
            foreach (Point Position in ItemsPositions)
            {
                DrawImage("InventoryItem", Position, LiteralSize(InventoryConfig.ItemsWidth, InventoryConfig.ItemsHeight));
                new UIRectangle(Position + LiteralSize(InventoryConfig.ItemsSeparatorX, InventoryConfig.ItemsSeparatorY), LiteralSize(InventoryConfig.ItemsSeparatorWidth, InventoryConfig.ItemsSeparatorHeight), Colors.Dividers).Draw();
            }

            // For each one of the positions, draw a background rectangle
            foreach (Point Position in WeaponPositions)
            {
                DrawImage("InventoryItem", Position, LiteralSize(InventoryConfig.WeaponWidth, InventoryConfig.WeaponHeight));
            }

            // Iterate over the number of player weapons
            for (int Index = 0; Index < InventoryConfig.Weapons.Count; Index++)
            {
                // Get the weapon internal name
                string Name = Enum.GetName(typeof(WeaponHash), InventoryConfig.Weapons[Index]);
                // Draw the weapon image
                DrawImage($"Weapon{Name}", WeaponPositions[Index] + LiteralSize(InventoryConfig.WeaponImageX, InventoryConfig.WeaponImageY), LiteralSize(InventoryConfig.WeaponImageWidth, InventoryConfig.WeaponImageHeight));

                // If the player pressed the click, do the specific action
                if (Game.IsControlJustPressed(0, Control.PhoneSelect))
                {
                    // If the player clicked on the weapon position
                    if (WeaponPositions[Index].IsClicked(LiteralSize(InventoryConfig.WeaponWidth, InventoryConfig.WeaponHeight)))
                    {
                        SelectOrGive(InventoryConfig.Weapons[Index]);
                    }
                }
            }

            // Store the offset for invalid items
            int Offset = 0;

            // Iterate over the maximum count of items
            for (int Index = 0; Index < InventoryItems.Count; Index++)
            {
                // If the current index + the total index is higher than the max
                if (Index > 15)
                {
                    // Break the for
                    break;
                }

                // If the item is not available
                if (!InventoryItems[Index].Visible)
                {
                    // Increase the offset and skip the iteration
                    Offset += 1;
                    continue;
                }

                // Draw the item
                DrawImage("Item" + InventoryItems[Index].Icon, ItemsPositions[Index - Offset] + LiteralSize(InventoryConfig.ItemsImageX, InventoryConfig.ItemsImageY), LiteralSize(InventoryConfig.ItemsImageWidth, InventoryConfig.ItemsImageHeight));
                new UIText(InventoryItems[Index].Quantity, ItemsPositions[Index - Offset] + LiteralSize(InventoryConfig.ItemsQuantityX, InventoryConfig.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();

                // If the player pressed the click, do the specific action
                if (Game.IsControlJustPressed(0, Control.PhoneSelect))
                {
                    // If the player clicked on the weapon position
                    if (ItemsPositions[Index - Offset].IsClicked(LiteralSize(InventoryConfig.ItemsWidth, InventoryConfig.ItemsHeight)))
                    {
                        InventoryItems[Index].PerformClick();
                    }
                }
            }
        }

        private void PlayerField(IBase Field, int Index, FieldSection Section)
        {
            // Store the positions for the UI elements
            bool IsPlayer = Section == FieldSection.Player;
            Position IconPosition = IsPlayer ? Position.PlayerIcon : Position.SquadIcon;
            Position InfoPosition = IsPlayer ? Position.PlayerInfo : Position.SquadInfo;

            // We are always going to need an icon
            Icon("Icon" + Field.Icon, HudConfig.GetSpecificPosition(IconPosition, Index, IsPlayer));

            // Store the base position
            Point BasePosition = HudConfig.GetSpecificPosition(InfoPosition, Index, IsPlayer);

            // If the field is either a health or text type
            if (Field is IHealth || Field is IText)
            {
                // Draw the background for normal fields
                new UIRectangle(BasePosition, IsPlayer ? LiteralSize(HudConfig.PlayerWidth, HudConfig.PlayerHeight) : LiteralSize(HudConfig.SquadWidth, HudConfig.SquadHeight), Colors.Backgrounds).Draw();
            }

            // If the type of field is health
            if (Field is IHealth Health)
            {
                // Draw the first field name
                new UIText(Health.Title, BasePosition + LiteralSize(HudConfig.SquadNameX, HudConfig.SquadNameY), IsPlayer ? .325f : .3f).Draw();

                // Calculate the percentage of total health
                float Percentage = Health.Current / Health.Maximum * 100;

                // With the percentage, calculate the right width for the health bar
                float Width = Percentage / 100 * LiteralSize(IsPlayer ? HudConfig.PlayerHealthWidth : HudConfig.SquadHealthWidth, 0).Width;
                // And create the size with the real health size
                Size HealthSize = new Size((int)Width, LiteralSize(0, IsPlayer ? HudConfig.PlayerHealthHeight : HudConfig.SquadHealthHeight).Height);

                // Calculate the color of the health bar
                Color HealthColor = Color.White;
                // If the health is on normal levels
                // Return White
                if (Percentage >= 50 && Percentage <= 100)
                {
                    HealthColor = Colors.HealthNormal;
                }
                // If the health is on risky levels
                // Return Yellow
                else if (Percentage <= 50 && Percentage >= 25)
                {
                    HealthColor = Colors.HealthDanger;
                }
                // If is about to die
                // Return Red
                else if (Percentage <= 25)
                {
                    HealthColor = Colors.HealthDying;
                }
                // If the health is under 0 or over 100
                // Return blue
                else
                {
                    HealthColor = Colors.HealthOverflow;
                }

                // Draw the entity health
                Size HealthOffset = IsPlayer ? LiteralSize(HudConfig.PlayerHealthX, HudConfig.PlayerHealthY) : LiteralSize(HudConfig.SquadHealthX, HudConfig.SquadHealthY);
                new UIRectangle(BasePosition + HealthOffset, HealthSize, HealthColor).Draw();

                // Draw the health dividers
                foreach (Point Position in HudConfig.GetDividerPositions(BasePosition, IsPlayer))
                {
                    new UIRectangle(Position, LiteralSize(HudConfig.DividerWidth, HudConfig.DividerHeight), Colors.Dividers).Draw();
                }
            }
            else if (Field is IText Text)
            {
                // Draw the first field name
                new UIText(Text.Title, BasePosition + LiteralSize(HudConfig.SquadNameX, HudConfig.SquadNameY), IsPlayer ? .325f : .3f).Draw();
                // Draw the second text field
                new UIText(Text.Text, BasePosition + LiteralSize(HudConfig.SquadName2X, HudConfig.SquadName2Y) + LiteralSize(0, HudConfig.SquadNameY), IsPlayer ? .325f : .3f).Draw();
            }
            // Else if the field type is weapon
            else if (Field is IWeapon Weapon)
            {
                // If we should draw the ammo count and weapon image
                if (Weapon.Available)
                {
                    // Store the position of the weapon space
                    Point WeaponLocation = HudConfig.GetSpecificPosition(Position.PlayerWeapon, Index, IsPlayer);

                    // Draw the ammo quantity
                    new UIRectangle(BasePosition, LiteralSize(HudConfig.SquareWidth, HudConfig.SquareHeight), Colors.Backgrounds).Draw();
                    new UIText(Weapon.Ammo.ToString("0"), BasePosition + LiteralSize(HudConfig.AmmoX, HudConfig.AmmoY), .6f, Color.White, GTA.Font.Monospace, true).Draw();

                    // And weapon image
                    new UIRectangle(WeaponLocation, LiteralSize(HudConfig.PlayerWidth, HudConfig.PlayerHeight) - LiteralSize(HudConfig.SquareWidth, 0) - LiteralSize(HudConfig.CommonX, 0), Colors.Backgrounds).Draw();
                    DrawImage("Weapon" + Weapon.Image, WeaponLocation + LiteralSize(HudConfig.WeaponX, HudConfig.WeaponY), LiteralSize(HudConfig.WeaponWidth, HudConfig.WeaponHeight));
                }
                // Otherwise, draw a simple placeholder
                else
                {
                    Icon("Placeholder", BasePosition);
                }
            }
            // Otherwise
            else
            {
                // Throw an exception because fuck it
                throw new InvalidOperationException("That Field type is not supported or valid.");
            }
        }

        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="Filename">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        private void Icon(string Filename, Point Position)
        {
            // Draw the background
            new UIRectangle(Position, LiteralSize(HudConfig.SquareWidth, HudConfig.SquareHeight), Colors.Backgrounds).Draw();
            // And the image over it
            DrawImage(Filename, Position + LiteralSize(HudConfig.IconX, HudConfig.IconY), LiteralSize(HudConfig.IconWidth, HudConfig.IconHeight));
        }

        /// <summary>
        /// Draws a dead marker over the ped head.
        /// </summary>
        /// <param name="GamePed">The ped where the dead marker should be drawn.</param>
        private void DeadMarker(Ped GamePed)
        {
            // Get the coordinates for the head
            Vector3 HeadCoord = GamePed.GetBoneCoord(Bone.SKEL_Head);
            // Get the On Screen position for the head
            Point ScreenPos = UI.WorldToScreen(HeadCoord);

            // Get distance ratio by Ln(Distance + Sqrt(e)), then calculate size of marker using intercept thereom.
            double Ratio = Math.Log(Vector3.Distance(Game.Player.Character.Position, HeadCoord) + 1.65);
            // Calculate the marker size based on the distance between player and dead ped
            Size LiteralMarker = LiteralSize(HudConfig.DeadMarkerWidth, HudConfig.DeadMarkerHeight);
            Size MarkerSize = new Size((int)(LiteralMarker.Width / Ratio), (int)(LiteralMarker.Height / Ratio));
            // Offset the marker by half width to center, and full height to put on top.
            ScreenPos.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            DrawImage("DeadMarker", ScreenPos, MarkerSize);
        }

        #endregion
    }
}
