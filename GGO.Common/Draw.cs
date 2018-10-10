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
            // Draw the rectangle on the background
            UIRectangle Rect = new UIRectangle(Position, Config.IconBackgroundSize, CBackground);
            Rect.Draw();

            // Calculate the position of the image
            Point ImagePos = Position + Config.IconPosition;
            // And finally, add the image on top
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
            // First, draw the black background
            UIRectangle Background = new UIRectangle(Position, Config.SquadInfoSize, CBackground);
            Background.Draw();

            // Then, calculate the health bar: (Percentage / 100) * DefaultWidth
            float Width = (Character.HealthPercentage() / 100) * Config.SquadHealthSize.Width;
            // Create a Size with the required size
            Size NewHealthSize = new Size(Convert.ToInt32(Width), Config.SquadHealthSize.Height);

            // For the dividers, get the distance between each one of them
            int HealthSep = Config.SquadHealthSize.Width / 4;

            // Prior to drawing the health bar we need the separators
            for (int Count = 0; Count < 5; Count++)
            {
                // Calculate the position of the separator
                Point Pos = (Position + Config.SquadHealthPos) + new Size(HealthSep * Count, 0) + Config.DividerPosition;
                // And draw it on screen
                UIRectangle Divider = new UIRectangle(Pos, Config.DividerSize, CDivider);
                Divider.Draw();
            }

            // After the separators are there, draw the health bar on the top
            UIRectangle HealthBar = new UIRectangle(Position + Config.SquadHealthPos, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            // And finally, draw the ped name
            UIText Name = new UIText(Character.Name(Config), Position + Config.NamePosition, 0.3f);
            Name.Draw();
        }

        /// <summary>
        /// Draws all of the player info, or top bar, of the player HUD.
        /// </summary>
        /// <param name="Config">Config settings.</param>
        /// <param name="Position">Position of player icon.</param>
        /// <param name="InfoPosition">Position of name and health bar.</param>
        public static void PlayerInfo(Configuration Config, Point Position, Point InfoPosition)
        {
            // Get the ped info to access health methods
            Ped Character = new Ped(Game.Player.Character.Handle);

            // Draw the player icon
            Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageCharacter, "PlayerAlive"), Position);

            // Draw the black background for the info
            UIRectangle Background = new UIRectangle(InfoPosition, Config.PlayerInfoSize, CBackground);
            Background.Draw();

            // Calculate the health bar: (Percentage / 100) * DefaultWidth
            float Width = (Character.HealthPercentage() / 100) * Config.PlayerHealthSize.Width;
            // Create a Size with the required size
            Size NewHealthSize = new Size(Convert.ToInt32(Width), Config.PlayerHealthSize.Height);

            // For the dividers, get the distance between each one of them
            int HealthSep = Config.PlayerHealthSize.Width / 4;

            // Prior to drawing the health bar we need the separators
            for (int Count = 0; Count < 5; Count++)
            {
                // Calculate the position of the separator
                Point Pos = (InfoPosition + Config.PlayerHealthPos) + new Size(HealthSep * Count, 0) + Config.DividerPosition;
                // And draw it on screen
                UIRectangle Divider = new UIRectangle(Pos, Config.DividerSize, CDivider);
                Divider.Draw();
            }

            // After the separators are there, draw the health bar on the top
            UIRectangle HealthBar = new UIRectangle(InfoPosition + Config.PlayerHealthPos, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            // And finally, draw the ped name
            UIText Name = new UIText(Character.Name(Config), InfoPosition + Config.NamePosition, 0.4f);
            Name.Draw();
        }

        /// <summary>
        /// Draws the main hand weapon, or middle bar, to the HUD.
        /// </summary>
        /// <param name="Config">Config settings.</param>
        /// <param name="HandPosition">Position for the ammo icon.</param>
        /// <param name="AmmoPosition">Position for the ammo counter.</param>
        /// <param name="WeaponPosition">Position for the weapon icon.</param>
        public static void PlayerMainHand(Configuration Config, Point HandPosition, Point AmmoPosition, Point WeaponPosition)
        {
            // Do not display main hand weapons for these types.
            if (Weapons.CurrentWeaponType == Weapons.Type.Banned || Weapons.CurrentWeaponType == Weapons.Type.Sidearm)
            {
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoWeaponMain"), HandPosition);
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoAmmoMain"), AmmoPosition);
                return;
            }
            // If the type is melee, do not display ammo icon or ammo counter
            else if (Weapons.CurrentWeaponType == Weapons.Type.Melee)
            {
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "MeleeWeaponMain"), HandPosition);
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoAmmoMain"), AmmoPosition);
            }
            else
            {
                // Draw the ammo icon
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageWeapon, "WeaponMain"), HandPosition);
                // Then draw the background for the ammo counter
                UIRectangle AmmoBackground = new UIRectangle(AmmoPosition, Config.IconBackgroundSize, CBackground);
                AmmoBackground.Draw();
                // Finally, draw the ammo counter on top
                UIText Ammo = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), AmmoPosition + Config.NamePosition, 1f);
                Ammo.Draw();
            }

            // Calculate the background width for the weapon size.
            int width = Config.PlayerInfoSize.Width - Config.IconBackgroundSize.Width - Config.ElementsRelative.Width;
            // Draw the weapon icon background
            UIRectangle WeaponBackground = new UIRectangle(WeaponPosition, new Size(width, Config.PlayerInfoSize.Height), CBackground);
            WeaponBackground.Draw();

            // Get the image for the current weapon
            string ImageFile = Common.Image.ResourceToPNG(Weapons.CurrentWeaponResource, Weapons.CurrentWeaponName);
            // Draw the weapon icon on top of the background
            Point ImagePos = WeaponPosition + Config.IconPosition;
            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, Config.WeaponImageSize);
        }

        /// <summary>
        /// Draws the off hand weapon, or bottom bar, to the HUD.
        /// </summary>
        /// <param name="Config">Config settings.</param>
        /// <param name="HandPosition">Position for the ammo icon.</param>
        /// <param name="AmmoPosition">Position for the ammo counter.</param>
        /// <param name="WeaponPosition">Position for the weapon icon.</param>
        public static void PlayerOffHand(Configuration Config, Point HandPosition, Point AmmoPosition, Point WeaponPosition)
        {
            // Do not display off hand weapons for these types.
            if (Weapons.CurrentWeaponType == Weapons.Type.Banned || Weapons.CurrentWeaponType == Weapons.Type.Main || Weapons.CurrentWeaponType == Weapons.Type.Melee)
            {
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoWeaponOff"), HandPosition);
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoAmmoOff"), AmmoPosition);
                return;
            }
            else
            {
                // Draw the ammo icon
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageWeapon, "WeaponMain"), HandPosition);
                // Then draw the background for the ammo counter
                UIRectangle AmmoBackground = new UIRectangle(AmmoPosition, Config.IconBackgroundSize, CBackground);
                AmmoBackground.Draw();
                // Finally, draw the ammo counter on top
                UIText Ammo = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), AmmoPosition + Config.NamePosition, 1f);
                Ammo.Draw();
            }

            // Calculate the background width for the weapon size.
            int width = Config.PlayerInfoSize.Width - Config.IconBackgroundSize.Width - Config.ElementsRelative.Width;
            // Draw the weapon icon background
            UIRectangle WeaponBackground = new UIRectangle(WeaponPosition, new Size(width, Config.PlayerInfoSize.Height), CBackground);
            WeaponBackground.Draw();

            // Get the image for the current weapon
            string ImageFile = Common.Image.ResourceToPNG(Weapons.CurrentWeaponResource, Weapons.CurrentWeaponName);
            // Draw the weapon icon on top of the background
            Point ImagePos = WeaponPosition + Config.IconPosition;
            UI.DrawTexture(ImageFile, 0, 0, 200, ImagePos, Config.WeaponImageSize);
        }
    }
}
