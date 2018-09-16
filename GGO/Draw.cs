using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO
{
    public class Draw
    {
        /// <summary>
        /// Draws an icon with it's respective background.
        /// </summary>
        public static void Icon(string ImageFile, Point Position, Size Background, Size Relative, Size Icon)
        {
            UIRectangle Rect = new UIRectangle(Position, Background, Colors.Background);
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
        public static void PedInfo(Ped Character, Point Position, Size InfoSize, Size HealthSize, Size Offset, Size DividerOffset, Size Divider, Size PlayerOffset)
        {
            UIRectangle Background = new UIRectangle(Position, InfoSize, Colors.Background);
            Background.Draw();

            float MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Character);
            float CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Character);
            float Percentage = (CurrentHealth / MaxHealth) * 100;
            float Width = (Percentage / 100) * HealthSize.Width;
            Size NewHealthSize = new Size(Convert.ToInt32(Width), HealthSize.Height);
            Point HealthPosition = Position + Offset;

            UIRectangle DividerOne = new UIRectangle(HealthPosition + DividerOffset, DividerOffset, Colors.Divider);
            DividerOne.Draw();

            UIRectangle DividerFive = new UIRectangle(HealthPosition + new Size(NewHealthSize.Width, 0) + DividerOffset - new Size(Divider.Width, 0), Divider, Colors.Divider);
            DividerFive.Draw();

            UIRectangle HealthBar = new UIRectangle(HealthPosition, NewHealthSize, Colors.GetPedHealthColor(Character));
            HealthBar.Draw();

            UIText Name = new UIText(Character.Model.GetHashCode().ToString(), Position + PlayerOffset, 0.3f);
            Name.Draw();
        }
    }
}
