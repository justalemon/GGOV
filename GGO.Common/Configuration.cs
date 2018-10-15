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
        public bool Debug
        {
            get
            {
                return (bool)Raw["debug"] || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
            }
        }
        /// <summary>
        /// The name for the current player.
        /// </summary>
        public string Name
        {
            get
            {
                return (string)Raw["name"];
            }
        }
        /// <summary>
        /// If the default Radar and Hud should be disabled.
        /// </summary>
        public bool DisableHud
        {
            get
            {
                return (bool)Raw["disable_hud"];
            }
        }

        /// <summary>
        /// Separation between the UI elements.
        /// </summary>
        public Size CommonSpace
        {
            get
            {
                return CreateSize("elements_relative");
            }
        }
        /// <summary>
        /// Size for the squared backgrounds.
        /// </summary>
        public Size SquaredBackground
        {
            get
            {
                return CreateSize("icon_background_size");
            }
        }
        /// <summary>
        /// Size for the dividers on the health bars.
        /// </summary>
        public Size DividerSize
        {
            get
            {
                return CreateSize("divider_size");
            }
        }

        /// <summary>
        /// The position of the player icon.
        /// </summary>
        public Point PlayerIcon
        {
            get
            {
                return new Point(CreateSize("player_general_pos"));
            }
        }
        /// <summary>
        /// The position of the player information.
        /// </summary>
        public Point PlayerInfo
        {
            get
            {
                return new Point(PlayerIcon.X + SquaredBackground.Width + CommonSpace.Width, PlayerIcon.Y);
            }
        }

        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point PrimaryIcon
        {
            get
            {
                return new Point(PlayerIcon.X, PlayerIcon.Y + CommonSpace.Height + SquaredBackground.Height);
            }
        }
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public Point SecondaryIcon
        {
            get
            {
                return new Point(PlayerIcon.X, PlayerIcon.Y + (CommonSpace.Height * 2) + (SquaredBackground.Height * 2));
            }
        }
        /// <summary>
        /// The position of the ammo for the primary weapon.
        /// </summary>
        public Point PrimaryAmmo
        {
            get
            {
                return new Point(PrimaryIcon.X + SquaredBackground.Width + CommonSpace.Width, PrimaryIcon.Y);
            }
        }
        /// <summary>
        /// The position of the ammo for the secondary weapon.
        /// </summary>
        public Point SecondaryAmmo
        {
            get
            {
                return new Point(SecondaryIcon.X + SquaredBackground.Width + CommonSpace.Width, SecondaryIcon.Y);
            }
        }

        public Size DividerPosition
        {
            get
            {
                return CreateSize("divider_pos");
            }
        }
        public Size NamePosition
        {
            get
            {
                return CreateSize("name_pos");
            }
        }

        public Size IconImageSize
        {
            get
            {
                return CreateSize("icon_image_size");
            }
        }
        public Size IconPosition
        {
            get
            {
                return CreateSize("icon_relative_pos");
            }
        }

        public Point SquadPosition
        {
            get
            {
                return new Point(CreateSize("squad_general_pos"));
            }
        }
        public Size SquadInfoSize
        {
            get
            {
                return CreateSize("squad_info_size");
            }
        }
        public Size SquadHealthSize
        {
            get
            {
                return CreateSize("squad_health_size");
            }
        }
        public Size SquadHealthPos
        {
            get
            {
                return CreateSize("squad_health_pos");
            }
        }
        public Size PlayerInfoSize
        {
            get
            {
                return CreateSize("player_info_size");
            }
        }
        public Size PlayerHealthSize
        {
            get
            {
                return CreateSize("player_health_size");
            }
        }
        public Size PlayerHealthPos
        {
            get
            {
                return CreateSize("player_health_pos");
            }
        }
        public Size WeaponImageSize
        {
            get
            {
                return CreateSize("weapon_image_size");
            }
        }
        public Point AmmoOffset
        {
            get
            {
                return new Point(CreateSize("ammo_offset_pos"));
            }
        }

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
