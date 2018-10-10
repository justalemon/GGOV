using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;

namespace GGO.Common
{
    public class Configuration
    {
        public bool Debug
        {
            get
            {
                return (bool)Raw["debug"] || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
            }
        }
        public string Name
        {
            get
            {
                return (string)Raw["name"];
            }
        }
        public bool DisableHud
        {
            get
            {
                return (bool)Raw["disable_hud"];
            }
        }

        public Size ElementsRelative
        {
            get
            {
                return CreateSize("elements_relative");
            }
        }
        public Size DividerSize
        {
            get
            {
                return CreateSize("divider_size");
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
        public Size IconBackgroundSize
        {
            get
            {
                return CreateSize("icon_background_size");
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
        public Point PlayerPosition
        {
            get
            {
                return new Point(CreateSize("player_general_pos"));
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
        public Size AmmoBackgroundSize
        {
            get
            {
                return CreateSize("player_ammo_size");
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
        /// Creates a Size from a JSON array.
        /// </summary>
        /// <returns>The working Size.</returns>
        private Size CreateSize(string ConfigOption)
        {
            return new Size((int)(Resolution.Width * (float)Raw[ConfigOption][0]), (int)(Resolution.Height * (float)Raw[ConfigOption][1]));
        }
    }
}
