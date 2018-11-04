using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;

namespace GGO.Shared
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
        public Size CommonSpacing => CreateSize("common_spacing");
        /// <summary>
        /// Size for the squared backgrounds.
        /// </summary>
        public Size SquaredBackground => CreateSize("squared_background");


        /// <summary>
        /// Size for the icons.
        /// </summary>
        public Size IconSize => CreateSize("icon_size");
        /// <summary>
        /// Position of the image relative to the background.
        /// </summary>
        public Size IconPosition => CreateSize("icon_position");


        /// <summary>
        /// Position of the squad information.
        /// </summary>
        public Point SquadPosition => new Point(CreateSize("squad_position"));
        /// <summary>
        /// Position of the name relative to the background.
        /// </summary>
        public Size NamePosition => CreateSize("name_position");
        /// <summary>
        /// Size for the squad information.
        /// </summary>
        public Size SquadSize => CreateSize("squad_size");


        /// <summary>
        /// The position of the player information.
        /// </summary>
        public Point PlayerPosition => new Point(CreateSize("player_position"));
        /// <summary>
        /// Size of the player information.
        /// </summary>
        public Size PlayerSize => CreateSize("player_size");
        /// <summary>
        /// Offset of the ammo.
        /// </summary>
        public Point AmmoOffset => new Point(CreateSize("ammo_offset"));
        /// <summary>
        /// Size for the weapon images.
        /// </summary>
        public Size WeaponSize => CreateSize("weapon_size");
        /// <summary>
        /// The size of the weapon background
        /// </summary>
        public Size WeaponBackground => new Size(PlayerSize.Width - CommonSpacing.Width - SquaredBackground.Width, PlayerSize.Height);
        /// <summary>
        /// The position of the player information.
        /// </summary>
        public Point PlayerInformation => new Point(PlayerPosition.X + SquaredBackground.Width + CommonSpacing.Width, PlayerPosition.Y);
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point PrimaryIcon => new Point(PlayerPosition.X, PlayerPosition.Y + CommonSpacing.Height + SquaredBackground.Height);
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point SecondaryIcon => new Point(PlayerPosition.X, PlayerPosition.Y + (CommonSpacing.Height * 2) + (SquaredBackground.Height * 2));
        /// <summary>
        /// The position of the ammo for the primary weapon.
        /// </summary>
        public Point PrimaryBackground => new Point(PrimaryIcon.X + SquaredBackground.Width + CommonSpacing.Width, PrimaryIcon.Y);
        /// <summary>
        /// The position of the ammo for the secondary weapon.
        /// </summary>
        public Point SecondaryBackground => new Point(SecondaryIcon.X + SquaredBackground.Width + CommonSpacing.Width, SecondaryIcon.Y);
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
        public Point PrimaryWeapon => new Point(PrimaryBackground.X + CommonSpacing.Width + SquaredBackground.Width, PrimaryBackground.Y);
        /// <summary>
        /// The position of the secondary weapon background.
        /// </summary>
        public Point SecondaryWeapon => new Point(SecondaryBackground.X + CommonSpacing.Width + SquaredBackground.Width, SecondaryBackground.Y);
        /// <summary>
        /// The position of the vehicle information.
        /// </summary>
        public Point VehicleIcon => new Point(PlayerPosition.X, PlayerPosition.Y - SquaredBackground.Width - CommonSpacing.Width);
        /// <summary>
        /// The position of the vehicle information.
        /// </summary>
        public Point VehicleInformation => new Point(VehicleIcon.X + SquaredBackground.Width + CommonSpacing.Width, VehicleIcon.Y);

        /// <summary>
        /// Size for the dividers on the health bars.
        /// </summary>
        public Size DividerSize => CreateSize("divider_size");
        /// <summary>
        /// Position of the health dividers.
        /// </summary>
        public Size DividerPosition => CreateSize("divider_position");
        /// <summary>
        /// Size for the squad health bars.
        /// </summary>
        public Size SquadHealthSize => CreateSize("squad_health_size");
        /// <summary>
        /// Position of the squad health bar.
        /// </summary>
        public Size SquadHealthPos => CreateSize("squad_health_position");
        /// <summary>
        /// Size of the player health bar.
        /// </summary>
        public Size PlayerHealthSize => CreateSize("player_health_size");
        /// <summary>
        /// Position of the player health bar.
        /// </summary>
        public Size PlayerHealthPos => CreateSize("player_health_position");
        
        /// <summary>
        /// Size for the health markers.
        /// </summary>
        public Size DeadMarker => CreateSize("dead_marker");

        /// <summary>
        /// The current screen resolution.
        /// </summary>
        private Size Resolution { get; set; }
        /// <summary>
        /// The RAW Configuration.
        /// </summary>
        private JObject Raw { get; set; }

        /// <summary>
        /// Loads up the configuration from "GGO.Shared.json"
        /// </summary>
        public Configuration(string Location, Size CurrentResolution)
        {
            // Read all of the text that is in the file
            string Content = File.ReadAllText(Location + "\\GGO.Shared.json");
            // Load it on the parser
            Raw = JObject.Parse(Content);
            // And store our current resolution
            Resolution = CurrentResolution;
        }

        /// <summary>
        /// Gets the name for the specified ped hash.
        /// </summary>
        /// <param name="Player">If the name is for the player.</param>
        /// <param name="Hash">The hash for the ped model.</param>
        /// <param name="DefaultName">The default name to be used.</param>
        /// <returns>The ped name.</returns>
        public string GetName(bool Player, int Hash, string DefaultName = "")
        {
            // If the ped is the player and the custom name has not been changed
            // Return the Social Club username
            if (Name == "default" && Player)
            {
                return DefaultName;
            }
            // If the ped is the player and a custom name has been added
            // Return that custom name
            else if (Player)
            {
                return Name;
            }
            // If is not the player but there is a custom name available
            // Return that ped name
            else if (Raw["names"][Hash.ToString()] != null)
            {
                return (string)Raw["names"][Hash.ToString()];
            }
            // If none of the previous ones work
            // Return the hash as a string
            else
            {
                return Hash.ToString();
            }
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
