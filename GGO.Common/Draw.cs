using GTA;
using GTA.Native;
using System;
using System.Drawing;
using System.Collections.Generic;

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
            UI.DrawTexture(ImageFile, 0, 0, 100, ImagePos, Config.IconImageSize);
        }

        /// <summary>
        /// Draws the icon and background for currently held weapon.
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="WeaponPosition"></param>
        public static void Weapon(Configuration Config, Point WeaponPosition)
        {
            // Calculate the background width for the weapon size.
            int width = Config.PlayerInfoSize.Width - Config.IconBackgroundSize.Width - Config.ElementsRelative.Width;
            // Draw the weapon icon background
            UIRectangle WeaponBackground = new UIRectangle(WeaponPosition, new Size(width, Config.PlayerInfoSize.Height), CBackground);
            WeaponBackground.Draw();

            // Get the image for the current weapon
            string ImageFile = Common.Image.ResourceToPNG(Weapons.CurrentWeaponResource, Weapons.CurrentWeaponName);
            // Draw the weapon icon on top of the background
            Point ImagePos = WeaponPosition + Config.IconPosition;
            UI.DrawTexture(ImageFile, 0, 0, 100, ImagePos, Config.WeaponImageSize);
        }

        /// <summary>
        /// Draws the complete information of a ped. That includes name and health.
        /// </summary>
        /// <param name="Config">Configuration settings.</param>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Position">The position for the ped icon.</param>
        /// <param name="InfoPosition">The position for the ped information.</param>
        /// <param name="Player">Whether this is the player HUD or squad HUD.</param>
        /// <param name="SquadCount">The number of the friendly within the squad.</param>
        public static void PedInfo(Configuration Config, Ped Character, Point Position, Point InfoPosition, bool isPlayer, int SquadCount = 0)
        {
            Size InfoSize = isPlayer ? Config.PlayerInfoSize : Config.SquadInfoSize;
            Size HealthSize = isPlayer ? Config.PlayerHealthSize : Config.SquadHealthSize;
            Size HealthPos = isPlayer ? Config.PlayerHealthPos : Config.SquadHealthPos;
            string iconText = isPlayer ? "Player" : "Squad" + SquadCount;
            Single textScale = isPlayer ? 0.4f : 0.3f;

            // Draw the player icon
            if(Character.IsAlive)
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageCharacter, iconText + "Alive"), Position);
            else
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageDead, iconText + "Dead"), Position);

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
            for (int Count = 0; Count < 5; Count++)
            {
                // Calculate the position of the separator
                Point Pos = (InfoPosition + HealthPos) + new Size(HealthSep * Count, 0) + Config.DividerPosition;
                // And draw it on screen
                UIRectangle Divider = new UIRectangle(Pos, Config.DividerSize, CDivider);
                Divider.Draw();
            }

            // After the separators are there, draw the health bar on the top
            UIRectangle HealthBar = new UIRectangle(InfoPosition + HealthPos, NewHealthSize, Character.HealthColor());
            HealthBar.Draw();

            // And finally, draw the ped name
            UIText Name = new UIText(Character.Name(Config), InfoPosition + Config.NamePosition, textScale);
            Name.Draw();
        }

        /// <summary>
        /// Draws the main hand weapon, or middle bar, to the HUD.
        /// </summary>
        /// <param name="Config">Config settings.</param>
        /// <param name="HandPosition">Position for the ammo icon.</param>
        /// <param name="AmmoPosition">Position for the ammo counter.</param>
        /// <param name="WeaponPosition">Position for the weapon icon.</param>
        /// <param name="isOffHand">Whether the Weapon is the off hand weapon or main hand weapon.</param>
        public static void PlayerWeapon(Configuration Config, Point HandPosition, Point AmmoPosition, Point WeaponPosition, bool isOffHand = false)
        {
            //Setup for whether this is the main or off hand.
            string hand = isOffHand ? "Off" : "Main";
            List<Weapons.Type> NoDraw = new List<Weapons.Type>() { Weapons.Type.Banned, Weapons.Type.Melee };
            NoDraw.Add(isOffHand ? Weapons.Type.Main : Weapons.Type.Sidearm);

            // Do not display main hand weapons for these types.
            if (NoDraw.Contains(Weapons.CurrentWeaponType))
            {
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoWeapon" + hand), HandPosition);
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.NoWeapon, "NoAmmo" + hand), AmmoPosition);
                // Only continue on to draw weapon if it is melee and this is the off hand.
                if(!(Weapons.CurrentWeaponType == Weapons.Type.Melee && isOffHand))
                    return;
            }
            else
            {
                // Draw the ammo icon
                Icon(Config, Common.Image.ResourceToPNG(Properties.Resources.ImageWeapon, "Weapon" + hand), HandPosition);
                // Then draw the background for the ammo counter
                UIRectangle AmmoBackground = new UIRectangle(AmmoPosition, Config.IconBackgroundSize, CBackground);
                AmmoBackground.Draw();
                // Finally, draw the ammo counter on top, adding offset so the text is always centered.
                UIText Ammo = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), new Point(AmmoPosition.X + Config.AmmoOffset.X, AmmoPosition.Y + Config.AmmoOffset.Y), .6f, Color.White, GTA.Font.Monospace, true);
                Ammo.Draw();
            }

            // Draw the weapon
            Weapon(Config, WeaponPosition);
        }
      
        public static void Ammo(Configuration Config, int CurrentAmmo)
        {
            Point BackgroundPos = new Point(Config.PlayerPosition.X + Config.ElementsRelative.Width + Config.IconBackgroundSize.Width,
                                            Config.PlayerPosition.Y + Config.ElementsRelative.Height + Config.IconBackgroundSize.Height);
            UIRectangle Background = new UIRectangle(BackgroundPos, Config.AmmoBackgroundSize, CBackground);
            Background.Draw();
        }
    }
}
