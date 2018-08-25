using GTA;
using System.Drawing;

namespace GGOHud
{
    class Draw
    {
        /// <summary>
        /// Draws an icon with it's respective background.
        /// </summary>
        public static void Icon(string ImageFile, Point Position)
        {
            UIRectangle Rect = new UIRectangle(Position, GGOHud.Config.IconBackground, Color.Black);
            Rect.Draw();

            Point ImagePos = Position + GGOHud.Config.IconRelative;

            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, GGOHud.Config.IconImage);
        }
    }
}
