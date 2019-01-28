using GGO.Extensions;
using GGO.UserData;
using GTA;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static GGO.Tools;

namespace GGO
{
    /// <summary>
    /// Script that handles the inventory actions.
    /// </summary>
    public class ScriptInventory : Script
    {
        /// <summary>
        /// Configuration for the inventory script.
        /// </summary>
        private Inventory InventoryConfig;
        /// <summary>
        /// Positions of the items inside of the inventory.
        /// </summary>
        private static List<Point> ItemsPosition = new List<Point>();
        /// <summary>
        /// Positions of the weapons inside of the inventory.
        /// </summary>
        private static List<Point> WeaponPositions = new List<Point>();
        /// <summary>
        /// Offset for the item click detection.
        /// </summary>
        private int Offset = 0;
        /// <summary>
        /// Next time to check if the user has a weapon that is not on the inventory.
        /// </summary>
        private int NextWeaponCheck = 0;

        /// <summary>
        /// If the ammo count should be available for the current weapon.
        /// </summary>
        private bool IsAmmoAvailable
        {
            get
            {
                switch (Game.Player.Character.Weapons.Current.GetStyle())
                {
                    case Usage.Main:
                    case Usage.Sidearm:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public ScriptInventory()
        {
            // Start by parsing the config
            InventoryConfig = JsonConvert.DeserializeObject<Inventory>(File.ReadAllText("scripts\\GGO\\Inventory.json"));

            // Don't do nothing if the user requested the menu to be disabled
            if (!InventoryConfig.Enabled)
            {
                return;
            }

            // Itterate between 0-2 (1-3) and 0-4 (1-5) and create the item positions
            for (int Y = 0; Y < 5; Y++)
            {
                for (int X = 0; X < 3; X++)
                {
                    // And generate the item positions
                    ItemsPosition.Add(new Point((int)(UI.WIDTH * InventoryConfig.ItemsX) + ((int)(UI.HEIGHT * InventoryConfig.SpacingX) * X) + ((int)(UI.HEIGHT * InventoryConfig.ItemsWidth) * X),
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
            Tick += OnTick;
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
        private void OnTick(object Sender, EventArgs Args)
        {
            // Don't process the inventory when using a controller and in a vehicle
            if (Game.CurrentInputMode == InputMode.GamePad && Game.Player.Character.IsInVehicle())
            {
                return;
            }

            // Update the item offset
            Offset = 0;
            if (IsAmmoAvailable && InventoryConfig.AmmoTotal)
            {
                Offset += 1;
            }
            if (IsAmmoAvailable && InventoryConfig.AmmoMags)
            {
                Offset += 1;
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
                Draw();
                // Show the cursor during this frame
                Function.Call(Hash._SHOW_CURSOR_THIS_FRAME);
                // Disable the fire, aim and camera controls
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
                Game.DisableControlThisFrame(0, Control.Aim);
                Game.DisableControlThisFrame(0, Control.LookUpDown);
                Game.DisableControlThisFrame(0, Control.LookLeftRight);
                // And check the user clicked something
                CheckClick();
            }
        }

        /// <summary>
        /// Draws the Inventory on screen.
        /// </summary>
        private void Draw()
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
            foreach (Point Position in ItemsPosition)
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
            }

            // Start an index to count how many items we have in total
            int ItemIndex = 0;

            // Show the total ammo count if the user wants
            if (InventoryConfig.AmmoTotal && IsAmmoAvailable)
            {
                DrawImage(Game.Player.Character.Weapons.Current.GetAmmoImage(), ItemsPosition[ItemIndex] + LiteralSize(InventoryConfig.ItemsImageX, InventoryConfig.ItemsImageY), LiteralSize(InventoryConfig.ItemsImageWidth, InventoryConfig.ItemsImageHeight));
                new UIText(Game.Player.Character.Weapons.Current.GetCorrectAmmo(), ItemsPosition[ItemIndex] + LiteralSize(InventoryConfig.ItemsQuantityX, InventoryConfig.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();
                ItemIndex++;
            }

            // If the user wants the mags to be shown
            if (InventoryConfig.AmmoMags && IsAmmoAvailable)
            {
                float MagsLeft = 0;
                if (Game.Player.Character.Weapons.Current.Ammo != 0 && Game.Player.Character.Weapons.Current.MaxAmmoInClip != 0)
                {
                    MagsLeft = Game.Player.Character.Weapons.Current.Ammo / Game.Player.Character.Weapons.Current.MaxAmmoInClip;
                }
                DrawImage(Game.Player.Character.Weapons.Current.GetMagazineImage(), ItemsPosition[ItemIndex] + LiteralSize(InventoryConfig.ItemsImageX, InventoryConfig.ItemsImageY), LiteralSize(InventoryConfig.ItemsImageWidth, InventoryConfig.ItemsImageHeight));
                new UIText(MagsLeft.ToString("0"), ItemsPosition[ItemIndex] + LiteralSize(InventoryConfig.ItemsQuantityX, InventoryConfig.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();
                ItemIndex++;
            }

            // Iterate over the maximum count of items
            for (int Index = 0; Index < InventoryConfig.Items.Count; Index++)
            {
                // If the current index + the total index is higher than the max
                if (Index + ItemIndex > 15)
                {
                    // Break the for
                    break;
                }

                // Set a dummy in case of the weapon does not exists
                string Ammo = "0";
                // If the weapon is on the player inventory
                if (Game.Player.Character.Weapons.HasWeapon(InventoryConfig.Items[Index]))
                {
                    // Set the correct ammo count
                    Ammo = Game.Player.Character.Weapons[InventoryConfig.Items[Index]].GetCorrectAmmo();
                }

                // Draw the item
                DrawImage("Item" + Enum.GetName(typeof(WeaponHash), InventoryConfig.Items[Index]), ItemsPosition[Index + ItemIndex] + LiteralSize(InventoryConfig.ItemsImageX, InventoryConfig.ItemsImageY), LiteralSize(InventoryConfig.ItemsImageWidth, InventoryConfig.ItemsImageHeight));
                new UIText(Ammo, ItemsPosition[Index + ItemIndex] + LiteralSize(InventoryConfig.ItemsQuantityX, InventoryConfig.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();
            }
        }

        private void CheckClick()
        {
            // If the player did not pressed the click, return
            if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
            {
                return;
            }

            // Iterate over the item count
            for (int Index = 0; Index < InventoryConfig.Items.Count; Index++)
            {
                // If the player clicked on the weapon position
                if (ItemsPosition[Index + Offset].IsClicked(LiteralSize(InventoryConfig.WeaponWidth, InventoryConfig.WeaponHeight)))
                {
                    SelectOrGive(InventoryConfig.Items[Index]);
                }
            }

            // Iterate over the weapon count
            for (int Index = 0; Index < InventoryConfig.Weapons.Count; Index++)
            {
                // If the player clicked on the weapon position
                if (WeaponPositions[Index].IsClicked(LiteralSize(InventoryConfig.WeaponWidth, InventoryConfig.WeaponHeight)))
                {
                    SelectOrGive(InventoryConfig.Weapons[Index]);
                }
            }
        }

        private void SelectOrGive(WeaponHash SelectedHash)
        {
            // If the current weapon equals the desired one
            if (Game.Player.Character.Weapons.Current.Hash == SelectedHash)
            {
                // Hide the weapon
                Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
            }
            // Check if the player does not has the weapon on the inventory
            else if (!Game.Player.Character.Weapons.HasWeapon(SelectedHash))
            {
                // If not, give them the requested weapon with no ammo
                Game.Player.Character.Weapons.Give(SelectedHash, 0, true, false);
            }
            else
            {
                // If the user has it, change the weapon to it
                Game.Player.Character.Weapons.Select(SelectedHash, true);
            }
        }
    }
}
