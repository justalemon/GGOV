using GTA;
using System;
using System.Drawing;

namespace GGO
{
    public static class Inventory
    {
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
        }
    }
}
