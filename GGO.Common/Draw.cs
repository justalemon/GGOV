using GTA;
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
        public static void Icon(Configuration Config, string ImageFile, Point Position)
        {
            UIRectangle Rect = new UIRectangle(Position, Config.IconBackgroundSize, CBackground);
            Rect.Draw();

            Point ImagePos = Position + Config.IconPosition;

            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, Config.IconImageSize);
        }

        /// <summary>
        /// Draws the complete information of a ped. That includes name and health.
        /// </summary>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Position">The position on the screen.</param>
        /// <param name="TotalSize">The full size of the information field.</param>
        public static void PedInfo(Configuration Config, Ped Character, Point Position)
        {
            UIRectangle Background = new UIRectangle(Position, Config.SquadInfoSize, CBackground);
            Background.Draw();

            float Width = (Character.HealthPercentage() / 100) * Config.SquadHealthSize.Width;
            Size NewHealthSize = new Size(Convert.ToInt32(Width), Config.SquadHealthSize.Height);
            Point HealthPosition = Position + Config.SquadHealthPos;

            int HealthSep = Config.SquadHealthSize.Width / 4;

            UIRectangle DividerOne = new UIRectangle(HealthPosition + Config.DividerPosition, Config.DividerSize, CDivider);
            DividerOne.Draw();

            UIRectangle DividerTwo = new UIRectangle(HealthPosition + new Size(HealthSep * 1, 0) + Config.DividerPosition, Config.DividerSize, CDivider);
            DividerTwo.Draw();

            UIRectangle DividerThree = new UIRectangle(HealthPosition + new Size(HealthSep * 2, 0) + Config.DividerPosition, Config.DividerSize, CDivider);
            DividerThree.Draw();

            UIRectangle DividerFour = new UIRectangle(HealthPosition + new Size(HealthSep * 3, 0) + Config.DividerPosition, Config.DividerSize, CDivider);
            DividerFour.Draw();

            UIRectangle DividerFive = new UIRectangle(HealthPosition + new Size(Config.SquadHealthSize.Width, 0) + Config.DividerPosition - new Size(Config.DividerSize.Width, 0), Config.DividerSize, CDivider);
            DividerFive.Draw();

            UIRectangle HealthBar = new UIRectangle(HealthPosition, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            UIText Name = new UIText(Character.Name(Config.Name), Position + Config.NamePosition, 0.3f);
            Name.Draw();
        }
    }
}
