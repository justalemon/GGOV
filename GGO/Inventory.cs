using GGO.Properties;
using GGO.UserData;
using GTA;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
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
        /// Positions of the weapons inside of the inventory.
        /// </summary>
        private static Point[] Positions = new Point[5];
        
        public Inventory()
        {
            // Start by parsing the config
            Config = JsonConvert.DeserializeObject<InventoryConfig>(File.ReadAllText("scripts\\GGO\\Inventory.json"));

            // Don't do nothing if the user requested the menu to be disabled
            if (!Config.Enabled)
            {
                return;
            }

            // Iterate between 0-4 (1-5)
            for (int Index = 0; Index < 5; Index++)
            {
                // And add a weapon on that position
                // Formula for Y: VerticalPosition + (SeparationBetweenWeapons * WeaponNumber)
                Positions[Index] = new Point((int)(UI.WIDTH * Config.WeaponX), (int)(UI.HEIGHT * Config.WeaponY) + ((int)(UI.HEIGHT * Config.WeaponSpacing) * Index));
            }

            // Add the events
            Tick += OnTick;
        }

        /// <summary>
        /// Tick that handles the drawing and actions of the inventory.
        /// </summary>
        public void OnTick(object Sender, EventArgs Args)
        {
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
            UIText ArmsText = new UIText("Arms", LiteralPoint(Config.ArmsX, Config.ArmsY), 0.3f, Color.White, GTA.Font.ChaletLondon, false);
            // Then, draw them on screen
            GeneralBackground.Draw();
            PlayerName.Draw();
            InfoBackground.Draw();
            NameBackground.Draw();
            HealthMax.Draw();
            HealthCurrent.Draw();
            LifeText.Draw();
            ArmsText.Draw();

            // Get the image and filename for the player gender
            Bitmap GenderPicture = Config.PlayerGender == Gender.Male ? Resources.GenderMale : Resources.GenderFemale;
            string GenderFilename = Config.PlayerGender == Gender.Male ? nameof(Resources.GenderMale) : nameof(Resources.GenderFemale);
            // Draw the gender image
            DrawImage(GenderPicture, GenderFilename, LiteralPoint(Config.GenderX, Config.GenderY), LiteralSize(Config.GenderWidth, Config.GenderHeight));

            // For each one of the positions, draw a background rectangle
            foreach (Point Position in Positions)
            {
                DrawImage(Resources.InventoryItem, nameof(Resources.InventoryItem), Position + LiteralSize(Config.WeaponRectangleX, Config.WeaponRectangleY), LiteralSize(Config.WeaponRectangleWidth, Config.WeaponRectangleHeight));
            }

            // Iterate over the number of player weapons
            for (int Index = 0; Index < Config.Weapons.Count; Index++)
            {
                // Get the weapon internal name
                string Name = Weapon.GetDisplayNameFromHash(Config.Weapons[Index]).Replace("WTT_", string.Empty);
                // Get the bitmap
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                // If the bitmap is valid, draw it
                if (WeaponBitmap != null)
                {
                    DrawImage(WeaponBitmap, "Weapon" + Name, Positions[Index], LiteralSize(Config.WeaponWidth, Config.WeaponHeight));
                }
            }
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
                if (Positions[Index].IsClicked(LiteralSize(Config.WeaponWidth, Config.WeaponHeight)))
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
