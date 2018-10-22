using GGO.Common.Properties;
using System.Drawing;

namespace GGO.Common
{
    /// <summary>
    /// Class that handles the drawing of UI elements.
    /// </summary>
    public abstract class Drawing
    {
        private Configuration StoredConfig;

        public Drawing(Configuration Config)
        {
            StoredConfig = Config;
        }

        public abstract void Rectangle(Point Position, Size Sizes, Color Colour);
        public abstract void Image(string File, Point Position, Size Sizes);
        public abstract void Text(string Text, Point Position, float Scale, int Font = 0, bool Center = false);

        public void Icon(string File, Point Position)
        {
            Rectangle(Position, StoredConfig.SquaredBackground, Colors.Backgrounds);
            Image(File, Position + StoredConfig.IconPosition, StoredConfig.ImageSize);
        }

        public void PedInfo(bool Player, string Name, int Hash, int CurrentHealth, int MaxHealth, int Count = 0)
        {
            Point InfoPosition = Player ? StoredConfig.PlayerInfo : Calculations.GetSquadPosition(StoredConfig, Count, true);
            Size InfoSize = Player ? StoredConfig.PlayerInfoSize : StoredConfig.SquadInfoSize;
            Size HealthPosition = Player ? StoredConfig.PlayerHealthPos : StoredConfig.SquadHealthPos;
            Size HealthSize = Calculations.GetHealthSize(StoredConfig, Player, MaxHealth, CurrentHealth);
            float TextSize = Player ? 0.35f : 0.3f;

            Rectangle(InfoPosition, InfoSize, Colors.Backgrounds);

            foreach (Point Position in Calculations.GetDividerPositions(StoredConfig, Player, Count))
            {
                Rectangle(Position, StoredConfig.DividerSize, Colors.Dividers);
            }

            Rectangle(InfoPosition + HealthPosition, HealthSize, Colors.GetHealthColor(CurrentHealth, MaxHealth));
            Text(StoredConfig.GetName(Player, Hash, Name), InfoPosition + StoredConfig.NamePosition, TextSize);
        }

        public void WeaponInfo(Checks.WeaponStyle Style, int Ammo, string Weapon)
        {
            bool Sidearm = Style == Checks.WeaponStyle.Sidearm;

            Point BackgroundLocation = Sidearm ? StoredConfig.SecondaryBackground : StoredConfig.PrimaryBackground;
            Point AmmoLocation = Sidearm ? StoredConfig.SecondaryAmmo : StoredConfig.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? StoredConfig.SecondaryWeapon : StoredConfig.PrimaryWeapon;
            string Name = Sidearm ? "Secondary" : "Primary";

            Rectangle(BackgroundLocation, StoredConfig.SquaredBackground, Colors.Backgrounds);
            Text(Ammo.ToString(), AmmoLocation, .6f, 2, true);

            Bitmap WeaponBitmap = Images.GetWeaponImages(Weapon);
            if (WeaponBitmap == null)
            {
                return;
            }

            Rectangle(WeaponLocation, StoredConfig.WeaponBackground, Colors.Backgrounds);
            Image(Images.ResourceToPNG(WeaponBitmap, "Gun" + Weapon + Name), WeaponLocation, StoredConfig.WeaponBackground);
        }

        public void DeadMarker(Point Position, Size Sizes, float Distance, int Hash)
        {
            Position.Offset(-Sizes.Width / 2, -Sizes.Height);
            Image(Images.ResourceToPNG(Resources.DeadMarker, "DeadMarker" + Hash), Position, Sizes);
        }
    }
}
