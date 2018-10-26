using GGO.Common.Properties;
using System.Drawing;

namespace GGO.Common
{
    /// <summary>
    /// Class that handles the drawing of UI elements.
    /// </summary>
    public abstract class Drawing
    {
        /// <summary>
        /// Our configuration instance.
        /// </summary>
        private Configuration StoredConfig;

        public Drawing(Configuration Config)
        {
            StoredConfig = Config;
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="Position">The position of the rectangle.</param>
        /// <param name="Sizes">The size of the rectangle.</param>
        /// <param name="Colour">The color of the rectangle.</param>
        public abstract void Rectangle(Point Position, Size Sizes, Color Colour);
        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="File">The file to draw.</param>
        /// <param name="Position">The on sceen position.</param>
        /// <param name="Sizes">The size of the image.</param>
        public abstract void Image(string File, Point Position, Size Sizes);
        /// <summary>
        /// Draws a text on screen.
        /// </summary>
        /// <param name="Text">The text to draw.</param>
        /// <param name="Position">The position on screen.</param>
        /// <param name="Scale">The size of the text.</param>
        /// <param name="Font">The GTA font to use.</param>
        /// <param name="Center">If the text should be centered.</param>
        public abstract void Text(string Text, Point Position, float Scale, int Font = 0, bool Center = false);

        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="File">The file to draw.</param>
        /// <param name="Position">The screen position.</param>
        public void Icon(string File, Point Position)
        {
            // Draw the background
            Rectangle(Position, StoredConfig.SquaredBackground, Colors.Backgrounds);
            // And the image over it
            Image(File, Position + StoredConfig.IconPosition, StoredConfig.ImageSize);
        }

        /// <summary>
        /// Draws the ped information.
        /// </summary>
        /// <param name="Player">If the information is for the player.</param>
        /// <param name="Squad">If this is part of the squad members.</param>
        /// <param name="Hash">The hash for the ped model.</param>
        /// <param name="CurrentHealth">The currrent ped health.</param>
        /// <param name="MaxHealth">The maximum ped health.</param>
        /// <param name="Count">The number of ped for the squad information.</param>
        /// <param name="Name">The Player in-game name.</param>
        public void PedInfo(bool Player, bool Squad, int Hash, int CurrentHealth, int MaxHealth, int Count = 0, string Name = "")
        {
            // Store the respective information for the player or squad
            Point InfoPosition = Squad ? StoredConfig.PlayerInfo : Calculations.GetSquadPosition(StoredConfig, Count, true);
            Size InfoSize = Squad ? StoredConfig.PlayerInfoSize : StoredConfig.SquadInfoSize;
            Size HealthPosition = Squad ? StoredConfig.PlayerHealthPos : StoredConfig.SquadHealthPos;
            Size HealthSize = Calculations.GetHealthSize(StoredConfig, Squad, MaxHealth, CurrentHealth);
            float TextSize = Squad ? 0.35f : 0.3f;

            // Draw the background rectangle
            Rectangle(InfoPosition, InfoSize, Colors.Backgrounds);

            // Draw the health dividers.
            foreach (Point Position in Calculations.GetDividerPositions(StoredConfig, Squad, Count))
            {
                Rectangle(Position, StoredConfig.DividerSize, Colors.Dividers);
            }

            // Draw the health bar
            Rectangle(InfoPosition + HealthPosition, HealthSize, Colors.GetHealthColor(CurrentHealth, MaxHealth));
            // And the player name
            Text(StoredConfig.GetName(Player, Hash, Name), InfoPosition + StoredConfig.NamePosition, TextSize);
        }

        /// <summary>
        /// Draws the player weapon information.
        /// </summary>
        /// <param name="Style">The weapon carry style.</param>
        /// <param name="Ammo">The ammo on the current magazine.</param>
        /// <param name="Weapon">The readable name of the weapon.</param>
        public void WeaponInfo(Checks.WeaponStyle Style, int Ammo, string Weapon)
        {
            // Check if the player is using a secondary weapon
            bool Sidearm = Style == Checks.WeaponStyle.Sidearm;

            // Store the information for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? StoredConfig.SecondaryBackground : StoredConfig.PrimaryBackground;
            Point AmmoLocation = Sidearm ? StoredConfig.SecondaryAmmo : StoredConfig.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? StoredConfig.SecondaryWeapon : StoredConfig.PrimaryWeapon;
            string Name = Sidearm ? "Secondary" : "Primary";

            // Draw the background and ammo quantity
            Rectangle(BackgroundLocation, StoredConfig.SquaredBackground, Colors.Backgrounds);
            Text(Ammo.ToString(), AmmoLocation, .6f, 2, true);

            // Get the weapon bitmap
            // If is not there, return
            Bitmap WeaponBitmap = Images.GetWeaponImages(Weapon);
            if (WeaponBitmap == null)
            {
                return;
            }

            // Finally, draw the weapon image with the respective background
            Rectangle(WeaponLocation, StoredConfig.WeaponBackground, Colors.Backgrounds);
            Image(Images.ResourceToPNG(WeaponBitmap, "Gun" + Weapon + Name), WeaponLocation + StoredConfig.IconPosition, StoredConfig.WeaponImageSize);
        }

        /// <summary>
        /// Draws a dead marker over the ped head.
        /// </summary>
        /// <param name="Position">The on-screen position of the ped health.</param>
        /// <param name="Distance">The distance from the player to the ped.</param>
        /// <param name="Hash">The ped hash (not the model hash).</param>
        public void DeadMarker(Point Position, float Distance, int Hash)
        {
            // Calculate the marker size based on the distance between player and dead ped
            Size MarkerSize = Calculations.GetMarkerSize(StoredConfig, Distance);

            // Offset the marker by half width to center, and full height to put on top.
            Position.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            Image(Images.ResourceToPNG(Resources.DeadMarker, "DeadMarker" + Hash), Position, MarkerSize);
        }
    }
}
