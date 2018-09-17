using GGO.Common;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO.Common
{
    public class Draw
    {
        /// <summary>
        /// Color for the backgrounds of the items.
        /// </summary>
        public static Color CBackground = Color.FromArgb(175, 0, 0, 0);
        /// <summary>
        /// Color for the dividers of the health bar.
        /// </summary>
        public static Color CDivider = Color.FromArgb(125, 230, 230, 230);

        /// <summary>
        /// Draws an icon with it's respective background.
        /// </summary>
        public static void Icon(string ImageFile, Point Position, Size Background, Size Relative, Size Icon)
        {
            UIRectangle Rect = new UIRectangle(Position, Background, CBackground);
            Rect.Draw();

            Point ImagePos = Position + Relative;

            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, Icon);
        }

        /// <summary>
        /// Draws the complete information of a ped. That includes name and health.
        /// </summary>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Position">The position on the screen.</param>
        /// <param name="TotalSize">The full size of the information field.</param>
        public static void PedInfo(Ped Character, Point Position, Size InfoSize, Size HealthSize, Size Offset, Size DividerOffset, Size Divider, Size PlayerOffset, string CName)
        {
            UIRectangle Background = new UIRectangle(Position, InfoSize, CBackground);
            Background.Draw();

            float Width = (Character.HealthPercentage() / 100) * HealthSize.Width;
            Size NewHealthSize = new Size(Convert.ToInt32(Width), HealthSize.Height);
            Point HealthPosition = Position + Offset;

            int HealthSep = HealthSize.Width / 4;

            UIRectangle DividerOne = new UIRectangle(HealthPosition + DividerOffset, Divider, CDivider);
            DividerOne.Draw();

            UIRectangle DividerTwo = new UIRectangle(HealthPosition + new Size(HealthSep * 1, 0) + DividerOffset, Divider, CDivider);
            DividerTwo.Draw();

            UIRectangle DividerThree = new UIRectangle(HealthPosition + new Size(HealthSep * 2, 0) + DividerOffset, Divider, CDivider);
            DividerThree.Draw();

            UIRectangle DividerFour = new UIRectangle(HealthPosition + new Size(HealthSep * 3, 0) + DividerOffset, Divider, CDivider);
            DividerFour.Draw();

            UIRectangle DividerFive = new UIRectangle(HealthPosition + new Size(HealthSize.Width, 0) + DividerOffset - new Size(Divider.Width, 0), Divider, CDivider);
            DividerFive.Draw();

            UIRectangle HealthBar = new UIRectangle(HealthPosition, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            UIText Name = new UIText(Character.Name(CName), Position + PlayerOffset, 0.3f);
            Name.Draw();
        }
    }
}
