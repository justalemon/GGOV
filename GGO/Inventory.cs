using GGO.Properties;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO
{
    public static class Inventory
    {
        private static uint? Primary => (uint)GGO.Config.Inventory["weapons"]["primary"];
        private static uint? Secondary => (uint)GGO.Config.Inventory["weapons"]["secondary"];
        private static uint? Backup => (uint)GGO.Config.Inventory["weapons"]["backup"];

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
            GeneralBackground.Draw();
            UIRectangle InfoBackground = new UIRectangle(GGO.Config.InventoryBackgroundPosition, GGO.Config.InventoryInfoSize, Colors.Backgrounds);
            InfoBackground.Draw();
            UIRectangle NameBackground = new UIRectangle(GGO.Config.InventoryColourPosition, GGO.Config.InventoryColourSize, Colors.Details);
            NameBackground.Draw();
            UIText PlayerName = new UIText(Game.Player.Name, GGO.Config.InventoryPlayerName, 0.7f, Color.White, GTA.Font.Monospace, false, false, false);
            PlayerName.Draw();

            if (Primary != null && Primary != 0)
            {
                string Name = Weapon.GetDisplayNameFromHash((WeaponHash)Primary).Replace("WTT_", string.Empty);
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                if (WeaponBitmap != null)
                {
                    Toolkit.Image(WeaponBitmap, "Weapon" + Name, GGO.Config.InventoryWeaponPrimary, GGO.Config.InventoryWeaponSize);
                }
            }
            if (Secondary != null && Secondary != 0)
            {
                string Name = Weapon.GetDisplayNameFromHash((WeaponHash)Secondary).Replace("WTT_", string.Empty);
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                if (WeaponBitmap != null)
                {
                    Toolkit.Image(WeaponBitmap, "Weapon" + Name, GGO.Config.InventoryWeaponSecondary, GGO.Config.InventoryWeaponSize);
                }
            }
            if (Backup != null && Backup != 0)
            {
                string Name = Weapon.GetDisplayNameFromHash((WeaponHash)Backup).Replace("WTT_", string.Empty);
                Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
                if (WeaponBitmap != null)
                {
                    Toolkit.Image(WeaponBitmap, "Weapon" + Name, GGO.Config.InventoryWeaponBackup, GGO.Config.InventoryWeaponSize);
                }
            }
        }

        private static void CheckClick()
        {
            if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
            {
                return;
            }

            if (Primary != null && Primary != 0 && GGO.Config.InventoryWeaponPrimary.IsClicked(GGO.Config.InventoryWeaponSize))
            {
                if (!Game.Player.Character.Weapons.HasWeapon((WeaponHash)Primary))
                {
                    Game.Player.Character.Weapons.Give((WeaponHash)Primary, 100, true, false);
                }
                else
                {
                    Game.Player.Character.Weapons.Select((WeaponHash)Primary, true);
                }
            }
            else if (Secondary != null && Secondary != 0 && GGO.Config.InventoryWeaponSecondary.IsClicked(GGO.Config.InventoryWeaponSize))
            {
                if (!Game.Player.Character.Weapons.HasWeapon((WeaponHash)Secondary))
                {
                    Game.Player.Character.Weapons.Give((WeaponHash)Secondary, 100, true, false);
                }
                else
                {
                    Game.Player.Character.Weapons.Select((WeaponHash)Secondary, true);
                }
            }
            else if (Backup != null && Backup != 0 && GGO.Config.InventoryWeaponBackup.IsClicked(GGO.Config.InventoryWeaponSize))
            {
                if (!Game.Player.Character.Weapons.HasWeapon((WeaponHash)Backup))
                {
                    Game.Player.Character.Weapons.Give((WeaponHash)Backup, 100, true, false);
                }
                else
                {
                    Game.Player.Character.Weapons.Select((WeaponHash)Backup, true);
                }
            }
        }
    }
}
