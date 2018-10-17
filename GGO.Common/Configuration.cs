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
        /// The position of the player icon.
        /// </summary>
        public Point PlayerIcon => new Point(CreateSize("player_general_pos"));
        /// <summary>
        /// The position of the player information.
        /// </summary>
        public Point PlayerInfo => new Point(PlayerIcon.X + SquaredBackground.Width + CommonSpace.Width, PlayerIcon.Y);

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

        public Size DividerPosition => CreateSize("divider_pos");
        public Size NamePosition => CreateSize("name_pos");

        public Size IconImageSize => CreateSize("icon_image_size");
        public Size IconPosition => CreateSize("icon_relative_pos");

        public Point SquadPosition => new Point(CreateSize("squad_general_pos"));
        public Size SquadInfoSize => CreateSize("squad_info_size");
        public Size SquadHealthSize => CreateSize("squad_health_size");
        public Size SquadHealthPos => CreateSize("squad_health_pos");
        public Size PlayerInfoSize => CreateSize("player_info_size");
        public Size PlayerHealthSize => CreateSize("player_health_size");
        public Size PlayerHealthPos => CreateSize("player_health_pos");
        public Size WeaponImageSize => CreateSize("weapon_image_size");
        public Point AmmoOffset => new Point(CreateSize("ammo_offset_pos"));

        private Size Resolution { get; set; }
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
            PedNames = new Names(Location +  "\\GGO.Names.json");
            // And store our current resolution
            Resolution = CurrentResolution;
        }

        /// <summary>
        /// Gets the specific position for the squad member.
        /// </summary>
        /// <param name="Count">The index of the squad member (zero based).</param>
        /// <param name="Info">If the location of the info should be returned.</param>
        /// <returns>A Point with the on screen position.</returns>
        public Point GetSquadPosition(int Count, bool Info = false)
        {
            Count++;

            if (Info)
            {
                return new Point(SquadPosition.X + SquaredBackground.Width + CommonSpace.Width, (SquadPosition.Y + CommonSpace.Height) * Count);
            }
            else
            {
                return new Point(SquadPosition.X, (SquadPosition.Y + CommonSpace.Height) * Count);
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
