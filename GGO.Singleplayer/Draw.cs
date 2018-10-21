using GGO.Common;
using GGO.Common.Properties;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace GGO.Singleplayer
{
    public class Draw
    {
        /// <summary>
        /// Draws an icon with it's respective background.
        /// </summary>
        public static void Icon(Configuration Config, string ImageFile, Point Position)
        {
            // Draw the rectangle on the background
            UIRectangle Rect = new UIRectangle(Position, Config.SquaredBackground, Colors.Backgrounds);
            Rect.Draw();

            // Calculate the position of the image
            Point ImagePos = Position + Config.IconPosition;
            // And finally, add the image on top
            UI.DrawTexture(ImageFile, 0, 0, 100, ImagePos, Config.ImageSize);
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
            // Get our health
            int CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Character) - 100;
            int MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Character) - 100;

            // Storing the correct information for either the player or squad member
            Point InfoPosition = Player ? Config.PlayerInfo : Calculations.GetSquadPosition(Config, Count, true);
            Size InfoSize = Player ? Config.PlayerInfoSize : Config.SquadInfoSize;
            Size HealthSize = Calculations.GetHealthSize(Config, Player, MaxHealth, CurrentHealth);
            Size HealthPosition = Player ? Config.PlayerHealthPos : Config.SquadHealthPos;
            float TextSize = Player ? 0.35f : 0.3f;

            // First, draw the black background
            UIRectangle Background = new UIRectangle(InfoPosition, InfoSize, Colors.Backgrounds);
            Background.Draw();

            // Prior to drawing the health bar we need the separators
            foreach (Point Position in Calculations.GetDividerPositions(Config, Player, Count))
            {
                UIRectangle Divider = new UIRectangle(Position, Config.DividerSize, Colors.Dividers);
                Divider.Draw();
            }

            // After the separators are there, draw the health bar on the top
            UIRectangle HealthBar = new UIRectangle(InfoPosition + HealthPosition, HealthSize, Colors.GetHealthColor(CurrentHealth, MaxHealth));
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
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, Config.SquaredBackground, Colors.Backgrounds);
            AmmoBackground.Draw();
            UIText AmmoCount = new UIText(Ammo.ToString(), AmmoLocation, .6f, Color.White, GTA.Font.Monospace, true);
            AmmoCount.Draw();

            // Request the weapon image, and return if is not valid
            Bitmap WeaponBitmap = Images.GetWeaponImages(Weapon);

            if (WeaponBitmap == null)
            {
                return;
            }

            // Draw the background
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, Config.WeaponBackground, Colors.Backgrounds);
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
            Size MarkerSize = Calculations.GetMarkerSize(Config, Distance);

            // Offset the marker by half width to center, and full height to put on top.
            Point MarkerPosition = UI.WorldToScreen(HeadCoord);
            MarkerPosition.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the dead marker.
            UI.DrawTexture(Images.ResourceToPNG(Resources.DeadMarker, "DeadMarker" + Character.GetHashCode()), 0, 0, 100, MarkerPosition, MarkerSize);
        }
    }
}
