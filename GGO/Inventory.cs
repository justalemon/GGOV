using GGO.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GGO
{
    public static class Inventory
    {
        private static List<uint> Weapons => GGO.Config.Inventory["weapons"].ToObject<List<uint>>();
        private static Point[] Positions = new Point[5];

        /// <summary>
        /// Stores the positions of the weapons once the configuration has been loaded.
        /// </summary>
        public static void StorePositions()
        {
            for (int Index = 0; Index < 5; Index++)
            {
                Positions[Index] = new Point(GGO.Config.InventoryWeaponPosition.X, GGO.Config.InventoryWeaponPosition.Y + (GGO.Config.InventoryWeaponSeparation.Height * Index));
            }
        }

        /// <summary>
        /// Tick that handles the 
        /// </summary>
        public static void Tick(object Sender, EventArgs Args)
        {
            // Disable the weapons menu
            Game.DisableControlThisFrame(0, Control.SelectWeapon);
            // If the user just pressed TAB/L1/LB, center the cursor
            if (Game.IsDisabledControlJustPressed(0, Control.SelectWeapon))
            {
                // Center the cursor on the screen
                bool OK = Function.Call<bool>(Hash._0xFC695459D4D0E219, 0.5f, 0.5f); // _SET_CURSOR_POSTION
                // If it was not possible
                if (!OK)
                {
                    Logging.Error("Unable to set the cursor on the center of the screen.");
                }
            }

            // Draw the inventory if the player tried to open the weapon selector
            if (Game.IsDisabledControlPressed(0, Control.SelectWeapon))
            {
                // Draw the inventory
                Draw();
                // Show the cursor during this frame
                Function.Call(Hash._SHOW_CURSOR_THIS_FRAME);
                // Disable the fire/aim controls
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
                Game.DisableControlThisFrame(0, Control.Aim);
                // And check the user clicks
                CheckClick();
            }
        }

        /// <summary>
        /// Draws the Inventory on screen.
        /// </summary>
        public static void Draw()
        {
            UIRectangle GeneralBackground = new UIRectangle(GGO.Config.InventoryBackgroundPosition, GGO.Config.InventoryBackgroundSize, Colors.Inventory);
            UIRectangle InfoBackground = new UIRectangle(GGO.Config.InventoryBackgroundPosition, GGO.Config.InventoryInfoSize, Colors.Backgrounds);
            UIRectangle NameBackground = new UIRectangle(GGO.Config.InventoryColourPosition, GGO.Config.InventoryColourSize, Colors.Details);
            UIText PlayerName = new UIText(Game.Player.Name, GGO.Config.InventoryPlayerName, 0.7f, Color.White, GTA.Font.Monospace, false, false, false);
            GeneralBackground.Draw();
            PlayerName.Draw();
            InfoBackground.Draw();
            NameBackground.Draw();

            Bitmap GenderPicture = (Gender)(int)GGO.Config.Inventory["gender"] == Gender.Male ? Resources.GenderMale : Resources.GenderFemale;
            string GenderFilename = (Gender)(int)GGO.Config.Inventory["gender"] == Gender.Male ? nameof(Resources.GenderMale) : nameof(Resources.GenderFemale);

            Toolkit.Image(GenderPicture, GenderFilename, GGO.Config.InventoryGender, GGO.Config.IconSize);

            foreach (Point Position in Positions)
            {
                Toolkit.Image(Resources.InventoryItem, nameof(Resources.InventoryItem), Position + GGO.Config.InventoryRectangleOffset, GGO.Config.InventoryRectangleSize);
            }

            for (int Index = 0; Index < Weapons.Count; Index++)
            {
                string Name = Weapon.GetDisplayNameFromHash((WeaponHash)Weapons[Index]).Replace("WTT_", string.Empty);
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                if (WeaponBitmap != null)
                {
                    Toolkit.Image(WeaponBitmap, "Weapon" + Name, Positions[Index], GGO.Config.InventoryWeaponSize);
                }
            }
        }

        private static void CheckClick()
        {
            if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
            {
                return;
            }

            for (int Index = 0; Index < Weapons.Count; Index++)
            {
                if (Positions[Index].IsClicked(GGO.Config.InventoryWeaponSize))
                {
                    if (!Game.Player.Character.Weapons.HasWeapon((WeaponHash)Weapons[Index]))
                    {
                        Game.Player.Character.Weapons.Give((WeaponHash)Weapons[Index], 100, true, false);
                    }
                    else
                    {
                        Game.Player.Character.Weapons.Select((WeaponHash)Weapons[Index], true);
                    }
                }
            }
        }
    }
}
