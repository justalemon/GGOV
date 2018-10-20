using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;

namespace GGO.Common
{
    public class Configuration
    {
        /// <summary>
        /// If the Debug window should be shown.
        /// </summary>
        public bool Debug => (bool)Raw["debug"] || Environment.GetEnvironmentVariable("LemonDev", EnvironmentVariableTarget.User) == "true";
        /// <summary>
        /// The name for the current player.
        /// </summary>
        public string Name => (string)Raw["name"];
        /// <summary>
        /// If the default Radar and Hud should be disabled.
        /// </summary>
        public bool DisableHud => (bool)Raw["disable_hud"];

        /// <summary>
        /// Separation between the UI elements.
        /// </summary>
        public Size CommonSpace => CreateSize("elements_relative");
        /// <summary>
        /// Size for the squared backgrounds.
        /// </summary>
        public Size SquaredBackground => CreateSize("icon_background_size");
        /// <summary>
        /// Size for the dividers on the health bars.
        /// </summary>
        public Size DividerSize => CreateSize("divider_size");
        /// <summary>
        /// Position of the name relative to the background.
        /// </summary>
        public Size NamePosition => CreateSize("name_pos");

        /// <summary>
        /// Size for the icons.
        /// </summary>
        public Size ImageSize => CreateSize("icon_image_size");
        /// <summary>
        /// Position of the image relative to the background.
        /// </summary>
        public Size IconPosition => CreateSize("icon_relative_pos");

        /// <summary>
        /// Position of the squad information.
        /// </summary>
        public Point SquadPosition => new Point(CreateSize("squad_general_pos"));
        /// <summary>
        /// Size for the squad information.
        /// </summary>
        public Size SquadInfoSize => CreateSize("squad_info_size");
        /// <summary>
        /// Size for the squad health bars.
        /// </summary>
        public Size SquadHealthSize => CreateSize("squad_health_size");
        /// <summary>
        /// Position of the squad health bar.
        /// </summary>
        public Size SquadHealthPos => CreateSize("squad_health_pos");

        /// <summary>
        /// The position of the player icon.
        /// </summary>
        public Point PlayerIcon => new Point(CreateSize("player_general_pos"));
        /// <summary>
        /// The position of the player information.
        /// </summary>
        public Point PlayerInfo => new Point(PlayerIcon.X + SquaredBackground.Width + CommonSpace.Width, PlayerIcon.Y);
        /// <summary>
        /// Size of the player information.
        /// </summary>
        public Size PlayerInfoSize => CreateSize("player_info_size");
        /// <summary>
        /// Size of the player health bar.
        /// </summary>
        public Size PlayerHealthSize => CreateSize("player_health_size");
        /// <summary>
        /// Position of the player health bar.
        /// </summary>
        public Size PlayerHealthPos => CreateSize("player_health_pos");

        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point PrimaryIcon => new Point(PlayerIcon.X, PlayerIcon.Y + CommonSpace.Height + SquaredBackground.Height);
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point SecondaryIcon => new Point(PlayerIcon.X, PlayerIcon.Y + (CommonSpace.Height * 2) + (SquaredBackground.Height * 2));
        /// <summary>
        /// The position of the ammo for the primary weapon.
        /// </summary>
        public Point PrimaryBackground => new Point(PrimaryIcon.X + SquaredBackground.Width + CommonSpace.Width, PrimaryIcon.Y);
        /// <summary>
        /// The position of the ammo for the secondary weapon.
        /// </summary>
        public Point SecondaryBackground => new Point(SecondaryIcon.X + SquaredBackground.Width + CommonSpace.Width, SecondaryIcon.Y);
        /// <summary>
        /// The position of the primary ammo counter.
        /// </summary>
        public Point PrimaryAmmo => new Point(PrimaryBackground.X + AmmoOffset.X, PrimaryBackground.Y + AmmoOffset.Y);
        /// <summary>
        /// The position of the secondary ammo counter.
        /// </summary>
        public Point SecondaryAmmo => new Point(SecondaryBackground.X + AmmoOffset.X, SecondaryBackground.Y + AmmoOffset.Y);
        /// <summary>
        /// The position of the primary weapon background.
        /// </summary>
        public Point PrimaryWeapon => new Point(PrimaryBackground.X + CommonSpace.Width + SquaredBackground.Width, PrimaryBackground.Y);
        /// <summary>
        /// The position of the secondary weapon background.
        /// </summary>
        public Point SecondaryWeapon => new Point(SecondaryBackground.X + CommonSpace.Width + SquaredBackground.Width, SecondaryBackground.Y);

        /// <summary>
        /// The size of the weapon background
        /// </summary>
        public Size WeaponBackground => new Size(PlayerInfoSize.Width - CommonSpace.Width - SquaredBackground.Width, PlayerInfoSize.Height);
        /// <summary>
        /// Offset of the ammo.
        /// </summary>
        public Point AmmoOffset => new Point(CreateSize("ammo_offset_pos"));
        /// <summary>
        /// Size for the weapon images.
        /// </summary>
        public Size WeaponImageSize => CreateSize("weapon_image_size");

        /// <summary>
        /// Position of the health dividers.
        /// </summary>
        public Size DividerPosition => CreateSize("divider_pos");
        /// <summary>
        /// Size for the health markers.
        /// </summary>
        public Size DeadMarkerSize => CreateSize("dead_marker_size");

        /// <summary>
        /// The current screen resolution.
        /// </summary>
        private Size Resolution { get; set; }
        /// <summary>
        /// The RAW Configuration.
        /// </summary>
        private JObject Raw { get; set; }
        /// <summary>
        /// The list of Ped Names.
        /// </summary>
        public Names PedNames;

        /// <summary>
        /// Loads up the configuration from "GGO.Common.json"
        /// </summary>
        public Configuration(string Location, Size CurrentResolution)
        {
            // Read all of the text that is in the file
            string Content = File.ReadAllText(Location + "\\GGO.Common.json");
            // Load it on the parser
            Raw = JObject.Parse(Content);
            // Dump our ped names
            PedNames = new Names(Location + "\\GGO.Names.json");
            // And store our current resolution
            Resolution = CurrentResolution;
        }

        /// <summary>
        /// Creates a Size from a JSON array.
        /// </summary>
        /// <returns>The working Size.</returns>
        private Size CreateSize(string ConfigOption)
        {
            return new Size((int)(Resolution.Width * (float)Raw[ConfigOption][0]), (int)(Resolution.Height * (float)Raw[ConfigOption][1]));
        }
    }
}
