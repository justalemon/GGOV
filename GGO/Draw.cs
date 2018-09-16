using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO
{
    class Draw
    {
        /// <summary>
        /// Draws an icon with it's respective background.
        /// </summary>
        public static void Icon(string ImageFile, Point Position)
        {
            UIRectangle Rect = new UIRectangle(Position, GGO.Config.IconBackground, Colors.Background);
            Rect.Draw();

            Point ImagePos = Position + GGO.Config.IconRelative;

            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, GGO.Config.IconImage);
        }

        /// <summary>
        /// Draws the complete information of a ped. That includes name and health.
        /// </summary>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Position">The position on the screen.</param>
        /// <param name="TotalSize">The full size of the information field.</param>
        public static void PedInfo(Ped Character, Point Position)
        {
            UIRectangle Background = new UIRectangle(Position, GGO.Config.SquadInfoSize, Colors.Background);
            Background.Draw();

            float MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Character);
            float CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Character);
            float Percentage = (CurrentHealth / MaxHealth) * 100;
            float Width = (Percentage / 100) * GGO.Config.HealthBarSize.Width;
            Size HealthSize = new Size(Convert.ToInt32(Width), GGO.Config.HealthBarSize.Height);
            Point HealthPosition = Position + GGO.Config.HealthBarOffset;

            UIRectangle DividerOne = new UIRectangle(HealthPosition + GGO.Config.HealthDividerOffset, GGO.Config.HealthDividerSize, Colors.Divider);
            DividerOne.Draw();

            UIRectangle DividerFive = new UIRectangle(HealthPosition + new Size(HealthSize.Width, 0) + GGO.Config.HealthDividerOffset - new Size(GGO.Config.HealthDividerSize.Width, 0), GGO.Config.HealthDividerSize, Colors.Divider);
            DividerFive.Draw();

            UIRectangle HealthBar = new UIRectangle(HealthPosition, HealthSize, Colors.GetPedHealthColor(Character));
            HealthBar.Draw();

            UI.ShowSubtitle(HealthSize.ToString());

            UIText Name = new UIText(Character.Model.GetHashCode().ToString(), Position + GGO.Config.PlayerNameOffset, 0.3f);
            Name.Draw();
        }
    }
}
