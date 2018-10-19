using GTA;
using GTA.Math;
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
            // Draw the rectangle on the background
            UIRectangle Rect = new UIRectangle(Position, Config.SquaredBackground, CBackground);
            Rect.Draw();

            // Calculate the position of the image
            Point ImagePos = Position + Config.IconPosition;
            // And finally, add the image on top
            UI.DrawTexture(ImageFile, 0, 0, 100, ImagePos, Config.IconImageSize);
        }

        /// <summary>
        /// Draws the complete information of a ped. That includes name and health.
        /// </summary>
        /// <param name="Config">Configuration settings.</param>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Player">Whether this is the player HUD or squad HUD.</param>
        /// <param name="Count">The number of the friendly within the squad.</param>
        public static void PedInfo(Configuration Config, Ped Character, bool Player, int Count = 0)
        {
            // Start by storing the correct information for either the player or squad member
            Point InfoPosition = Player ? Config.PlayerInfo : Config.GetSquadPosition(Count, true);
            Size InfoSize = Player ? Config.PlayerInfoSize : Config.SquadInfoSize;
            Size HealthSize = Player ? Config.PlayerHealthSize : Config.SquadHealthSize;
            Size HealthPosition = Player ? Config.PlayerHealthPos : Config.SquadHealthPos;
            float TextSize = Player ? 0.35f : 0.3f;

            // First, draw the black background
            UIRectangle Background = new UIRectangle(InfoPosition, InfoSize, CBackground);
            Background.Draw();

            // Then, calculate the health bar: (Percentage / 100) * DefaultWidth
            float Width = (Character.HealthPercentage() / 100) * HealthSize.Width;
            // Create a Size with the required size
            Size NewHealthSize = new Size(Convert.ToInt32(Width), HealthSize.Height);

            // For the dividers, get the distance between each one of them
            int HealthSep = HealthSize.Width / 4;

            // Prior to drawing the health bar we need the separators
            for (int Separator = 0; Separator < 5; Separator++)
            {
                // Calculate the position of the separator
                Point Pos = (InfoPosition + HealthPosition) + new Size(HealthSep * Separator, 0) + Config.DividerPosition;
                // And draw it on screen
                UIRectangle Divider = new UIRectangle(Pos, Config.DividerSize, CDivider);
                Divider.Draw();
            }

            // After the separators are there, draw the health bar on the top
            UIRectangle HealthBar = new UIRectangle(InfoPosition + HealthPosition, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            // And finally, draw the ped name
            UIText Name = new UIText(Character.Name(Config), InfoPosition + Config.NamePosition, TextSize);
            Name.Draw();
        }
        
        /// <summary>
        /// Draws the information about the player ammo.
        /// </summary>
        /// <param name="Config">The mod settings.</param>
        /// <param name="Sidearm">If the specified ammo is for the sidearm.</param>
        public static void WeaponInfo(Configuration Config, bool Sidearm, int Ammo, string Weapon)
        {
            // Start by selecting the correct location for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? Config.SecondaryBackground : Config.PrimaryBackground;
            Point AmmoLocation = Sidearm ? Config.SecondaryAmmo : Config.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? Config.SecondaryWeapon : Config.PrimaryWeapon;
            string Name = Sidearm ? "Secondary" : "Primary";

            // Then, draw the ammo information
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, Config.SquaredBackground, CBackground);
            AmmoBackground.Draw();
            UIText AmmoCount = new UIText(Ammo.ToString(), AmmoLocation, .6f, Color.White, GTA.Font.Monospace, true);
            AmmoCount.Draw();

            // Request the weapon image, and return if is not valid
            Bitmap WeaponBitmap = Images.GetBitmap(Weapon);

            if (WeaponBitmap == null)
            {
                return;
            }

            // Draw the background
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, Config.WeaponBackground, CBackground);
            WeaponBackground.Draw();

            // With the weapon image
            UI.DrawTexture(Images.ResourceToPNG(WeaponBitmap, "Gun" + Weapon + Name), 0, 0, 100, WeaponLocation, Config.WeaponBackground);
        }

        /// <summary>
        /// Draws the dead marker on top of dead peds heads.
        /// </summary>
        /// <param name="Config">The mod settings.</param>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Count">The dead ped count.</param>
        public static void DeadMarker(Configuration Config, Ped Character)
        {
            // Get the coordinates for the head of the dead ped.
            Vector3 HeadCoord = Character.GetBoneCoord(Bone.SKEL_Head);

            // Calculate the distance between player and dead ped's head.
            float Distance = Vector3.Distance(Game.Player.Character.Position, HeadCoord);

            // Get distance ratio by Ln(Distance + Sqrt(e)), then calculate size of marker using intercept thereom.
            double Ratio = Math.Log(Distance + 1.65);
            Size MarkerSize = new Size((int)(Config.DeadMarkerSize.Width / Ratio), (int)(Config.DeadMarkerSize.Height / Ratio));

            // Offset the marker by half width to center, and full height to put on top.
            Point MarkerPosition = UI.WorldToScreen(HeadCoord);
            MarkerPosition.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the dead marker.
            UI.DrawTexture(Images.ResourceToPNG(Properties.Resources.DeadMarker, "DeadMarker" + Character.GetHashCode()), 0, 0, 100, MarkerPosition, MarkerSize);
        }
    }
}
