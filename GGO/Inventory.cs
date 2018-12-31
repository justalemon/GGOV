using GGO.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// Script that handles the inventory actions.
    /// </summary>
    public class Inventory : Script
    {
        /// <summary>
        /// The weapons that the Player wants on the inventory.
        /// </summary>
        private static List<uint> Weapons => GGO.Config.Inventory["weapons"].ToObject<List<uint>>();
        /// <summary>
        /// Positions of the weapons inside of the inventory.
        /// </summary>
        private static Point[] Positions = new Point[5];
        
        public Inventory()
        {
            // Iterate between 0-4 (1-5)
            for (int Index = 0; Index < 5; Index++)
            {
                // And add a weapon on that position
                // Formula for Y: VerticalPosition + (SeparationBetweenWeapons * WeaponNumber)
                Positions[Index] = new Point(GGO.Config.InventoryWeaponPosition.X, GGO.Config.InventoryWeaponPosition.Y + (GGO.Config.InventoryWeaponSeparation.Height * Index));
            }

            // Add the events
            Tick += OnTick;
        }

        /// <summary>
        /// Tick that handles the drawing and actions of the inventory.
        /// </summary>
        public static void OnTick(object Sender, EventArgs Args)
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
        public static void Draw()
        {
            // Get the current and max health and calculate the size of the health bar
            float HealthMaxN = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
            float HealthCurrentN = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
            float HealthWidth = HealthCurrentN / HealthMaxN * 100 / 100 * GGO.Config.InventoryHealthSize.Width;
            Size HealthSize = new Size((int)HealthWidth, GGO.Config.InventoryHealthSize.Height);

            // Generate the information for the Rectangles and Texts
            UIRectangle GeneralBackground = new UIRectangle(GGO.Config.InventoryBackgroundPosition, GGO.Config.InventoryBackgroundSize, Colors.Inventory);
            UIRectangle InfoBackground = new UIRectangle(GGO.Config.InventoryBackgroundPosition, GGO.Config.InventoryInfoSize, Colors.Backgrounds);
            UIRectangle NameBackground = new UIRectangle(GGO.Config.InventoryColourPosition, GGO.Config.InventoryColourSize, Colors.Details);
            UIRectangle HealthMax = new UIRectangle(GGO.Config.InventoryHealthPosition, GGO.Config.InventoryHealthSize, Color.Gray);
            UIRectangle HealthCurrent = new UIRectangle(GGO.Config.InventoryHealthPosition, HealthSize, Color.White);
            UIText PlayerName = new UIText(Game.Player.Name, GGO.Config.InventoryPlayerName, 0.7f, Color.White, GTA.Font.Monospace, false, false, false);
            UIText LifeText = new UIText("Life", GGO.Config.InventoryLifePosition, 0.3f, Color.White, GTA.Font.ChaletLondon, false, false, false);
            UIText ArmsText = new UIText("Arms", GGO.Config.InventoryArmsPosition, 0.3f, Color.White, GTA.Font.ChaletLondon, false);
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
            Bitmap GenderPicture = (Gender)(int)GGO.Config.Inventory["gender"] == Gender.Male ? Resources.GenderMale : Resources.GenderFemale;
            string GenderFilename = (Gender)(int)GGO.Config.Inventory["gender"] == Gender.Male ? nameof(Resources.GenderMale) : nameof(Resources.GenderFemale);
            // Draw the gender image
            Toolkit.Image(GenderPicture, GenderFilename, GGO.Config.InventoryGender, GGO.Config.IconSize);

            // For each one of the positions, draw a background rectangle
            foreach (Point Position in Positions)
            {
                Toolkit.Image(Resources.InventoryItem, nameof(Resources.InventoryItem), Position + GGO.Config.InventoryRectangleOffset, GGO.Config.InventoryRectangleSize);
            }

            // Iterate over the number of player weapons
            for (int Index = 0; Index < Weapons.Count; Index++)
            {
                // Get the weapon internal name
                string Name = Weapon.GetDisplayNameFromHash((WeaponHash)Weapons[Index]).Replace("WTT_", string.Empty);
                // Get the bitmap
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                // If the bitmap is valid, draw it
                if (WeaponBitmap != null)
                {
                    Toolkit.Image(WeaponBitmap, "Weapon" + Name, Positions[Index], GGO.Config.InventoryWeaponSize);
                }
            }
        }

        private static void CheckClick()
        {
            // If the player did not pressed the click, return
            if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
            {
                return;
            }

            // Iterate over the weapon count
            for (int Index = 0; Index < Weapons.Count; Index++)
            {
                // If the player clicked on the weapon position
                if (Positions[Index].IsClicked(GGO.Config.InventoryWeaponSize))
                {
                    // Check if the player does not has the weapon on the inventory
                    if (!Game.Player.Character.Weapons.HasWeapon((WeaponHash)Weapons[Index]))
                    {
                        // If not, give them the requested weapon with 100 of ammo
                        Game.Player.Character.Weapons.Give((WeaponHash)Weapons[Index], 100, true, false);
                    }
                    else
                    {
                        // If the user has it, change the weapon to it
                        Game.Player.Character.Weapons.Select((WeaponHash)Weapons[Index], true);
                    }
                }
            }
        }
    }
}
