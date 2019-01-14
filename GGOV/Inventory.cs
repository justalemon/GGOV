using GGO.Properties;
using GGO.UserData;
using GTA;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static GGO.Tools;

namespace GGO
{
    /// <summary>
    /// Script that handles the inventory actions.
    /// </summary>
    public class Inventory : Script
    {
        /// <summary>
        /// Configuration for the inventory script.
        /// </summary>
        private InventoryConfig Config;
        /// <summary>
        /// Positions of the items inside of the inventory.
        /// </summary>
        private static List<Point> ItemsPosition = new List<Point>();
        /// <summary>
        /// Positions of the weapons inside of the inventory.
        /// </summary>
        private static List<Point> WeaponPositions = new List<Point>();
        
        public Inventory()
        {
            // Start by parsing the config
            Config = JsonConvert.DeserializeObject<InventoryConfig>(File.ReadAllText("scripts\\GGO\\Inventory.json"));

            // Don't do nothing if the user requested the menu to be disabled
            if (!Config.Enabled)
            {
                return;
            }

            // Itterate between 0-2 (1-3) and 0-4 (1-5) and create the item positions
            for (int Y = 0; Y < 5; Y++)
            {
                for (int X = 0; X < 3; X++)
                {
                    // And generate the item positions
                    ItemsPosition.Add(new Point((int)(UI.WIDTH * Config.ItemsX) + ((int)(UI.HEIGHT * Config.SpacingX) * X) + ((int)(UI.HEIGHT * Config.ItemsWidth) * X),
                                                (int)(UI.HEIGHT * Config.ItemsY) + ((int)(UI.HEIGHT * Config.SpacingY) * Y)));
                }
            }
            // Iterate between 0-4 (1-5) and create the weapon positions
            for (int Index = 0; Index < 5; Index++)
            {
                // And add a weapon on that position
                // Formula for Y: VerticalPosition + (SeparationBetweenWeapons * WeaponNumber)
                WeaponPositions.Add(new Point((int)(UI.WIDTH * Config.WeaponX), (int)(UI.HEIGHT * Config.WeaponY) + ((int)(UI.HEIGHT * Config.SpacingY) * Index)));
            }

            // Add the events
            Tick += OnTick;
        }

        /// <summary>
        /// Tick that handles the drawing and actions of the inventory.
        /// </summary>
        public void OnTick(object Sender, EventArgs Args)
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
        public void Draw()
        {
            // Get the current and max health and calculate the size of the health bar
            float HealthMaxN = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
            float HealthCurrentN = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
            float HealthWidth = HealthCurrentN / HealthMaxN * 100 / 100 * (UI.WIDTH * Config.HealthWidth);
            Size HealthSize = new Size((int)HealthWidth, (int)(UI.HEIGHT * Config.HealthHeight));

            // Generate the information for the Rectangles and Texts
            UIRectangle GeneralBackground = new UIRectangle(LiteralPoint(Config.BackgroundX, Config.BackgroundY), LiteralSize(Config.BackgroundWidth, Config.BackgroundHeight), Colors.Inventory);
            UIRectangle InfoBackground = new UIRectangle(LiteralPoint(Config.BackgroundX, Config.BackgroundY), LiteralSize(Config.InfoWidth, Config.InfoHeight), Colors.Backgrounds);
            UIRectangle NameBackground = new UIRectangle(LiteralPoint(Config.PlayerX, Config.PlayerY), LiteralSize(Config.PlayerWidth, Config.PlayerHeight), Colors.Details);
            UIRectangle HealthMax = new UIRectangle(LiteralPoint(Config.HealthX, Config.HealthY), LiteralSize(Config.HealthWidth, Config.HealthHeight), Color.Gray);
            UIRectangle HealthCurrent = new UIRectangle(LiteralPoint(Config.HealthX, Config.HealthY), HealthSize, Color.White);
            UIText PlayerName = new UIText(Game.Player.Name, LiteralPoint(Config.NameX, Config.NameY), 0.7f, Color.White, GTA.Font.Monospace, false, false, false);
            UIText LifeText = new UIText("Life", LiteralPoint(Config.LifeX, Config.LifeY), 0.3f, Color.White, GTA.Font.ChaletLondon, false, false, false);
            UIText ItemsText = new UIText("Items", LiteralPoint(Config.ItemsTextX, Config.ItemsTextY), 0.3f, Color.White, GTA.Font.ChaletLondon, false);
            UIText ArmsText = new UIText("Arms", LiteralPoint(Config.WeaponTextX, Config.WeaponTextY), 0.3f, Color.White, GTA.Font.ChaletLondon, false);
            // Then, draw them on screen
            GeneralBackground.Draw();
            PlayerName.Draw();
            InfoBackground.Draw();
            NameBackground.Draw();
            HealthMax.Draw();
            HealthCurrent.Draw();
            LifeText.Draw();
            ItemsText.Draw();
            ArmsText.Draw();
            
            // Draw the gender image
            DrawImage(Config.PlayerGender == Gender.Male ? "GenderMale" : "GenderFemale", LiteralPoint(Config.GenderX, Config.GenderY), LiteralSize(Config.GenderWidth, Config.GenderHeight));

            // Draw the item backgrounds
            foreach (Point Position in ItemsPosition)
            {
                DrawImage("InventoryItem", Position, LiteralSize(Config.ItemsWidth, Config.ItemsHeight));
                new UIRectangle(Position + LiteralSize(Config.ItemsSeparatorX, Config.ItemsSeparatorY), LiteralSize(Config.ItemsSeparatorWidth, Config.ItemsSeparatorHeight), Colors.Dividers).Draw();
            }

            // For each one of the positions, draw a background rectangle
            foreach (Point Position in WeaponPositions)
            {
                DrawImage("InventoryItem", Position, LiteralSize(Config.WeaponWidth, Config.WeaponHeight));
            }

            // Iterate over the number of player weapons
            for (int Index = 0; Index < Config.Weapons.Count; Index++)
            {
                // Get the weapon internal name
                string Name = Weapon.GetDisplayNameFromHash(Config.Weapons[Index]).Replace("WTT_", string.Empty);
                // Draw the weapon image
                DrawImage($"Weapon{Name}", WeaponPositions[Index] + LiteralSize(Config.WeaponImageX, Config.WeaponImageY), LiteralSize(Config.WeaponImageWidth, Config.WeaponImageHeight));
            }

            // Draw the ammo icon and count on the first slot
            DrawImage(Game.Player.Character.Weapons.Current.GetAmmoImage(), ItemsPosition[0] + LiteralSize(Config.ItemsImageX, Config.ItemsImageY), LiteralSize(Config.ItemsImageWidth, Config.ItemsImageHeight));
            new UIText(Game.Player.Character.Weapons.Current.Ammo.ToString(), ItemsPosition[0] + LiteralSize(Config.ItemsQuantityX, Config.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();

            // Draw the ammo icon and count on the first slot
            float MagsLeft = Game.Player.Character.Weapons.Current.Ammo / Game.Player.Character.Weapons.Current.MaxAmmoInClip;
            DrawImage(Game.Player.Character.Weapons.Current.GetAmmoImage(), ItemsPosition[1] + LiteralSize(Config.ItemsImageX, Config.ItemsImageY), LiteralSize(Config.ItemsImageWidth, Config.ItemsImageHeight));
            new UIText(MagsLeft.ToString("0"), ItemsPosition[1] + LiteralSize(Config.ItemsQuantityX, Config.ItemsQuantityY), 0.475f, Color.White, GTA.Font.ChaletLondon, true).Draw();
        }

        private void CheckClick()
        {
            // If the player did not pressed the click, return
            if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
            {
                return;
            }

            // Iterate over the weapon count
            for (int Index = 0; Index < Config.Weapons.Count; Index++)
            {
                // If the player clicked on the weapon position
                if (WeaponPositions[Index].IsClicked(LiteralSize(Config.WeaponWidth, Config.WeaponHeight)))
                {
                    // Check if the player does not has the weapon on the inventory
                    if (!Game.Player.Character.Weapons.HasWeapon(Config.Weapons[Index]))
                    {
                        // If not, give them the requested weapon with 100 of ammo
                        Game.Player.Character.Weapons.Give(Config.Weapons[Index], 100, true, false);
                    }
                    else
                    {
                        // If the user has it, change the weapon to it
                        Game.Player.Character.Weapons.Select(Config.Weapons[Index], true);
                    }
                }
            }
        }
    }
}
