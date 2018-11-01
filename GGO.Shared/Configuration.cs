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
        public SizeF CommonSpacing => new SizeF((float)Raw["common_spacing"][0], (float)Raw["common_spacing"][1]);
        /// <summary>
        /// Size for the squared backgrounds.
        /// </summary>
        public SizeF SquaredBackground => new SizeF((float)Raw["squared_background"][0], (float)Raw["squared_background"][1]);


        /// <summary>
        /// Size for the icons.
        /// </summary>
        public SizeF IconSize => new SizeF((float)Raw["icon_size"][0], (float)Raw["icon_size"][1]);
        /// <summary>
        /// Position of the image relative to the background.
        /// </summary>
        public SizeF IconPosition => new SizeF((float)Raw["icon_position"][0], (float)Raw["icon_position"][1]);


        /// <summary>
        /// Position of the squad information.
        /// </summary>
        public PointF SquadPosition => new PointF((float)Raw["squad_position"][0], (float)Raw["squad_position"][1]);
        /// <summary>
        /// Position of the name relative to the background.
        /// </summary>
        public SizeF NamePosition => new SizeF((float)Raw["name_position"][0], (float)Raw["name_position"][1]);
        /// <summary>
        /// Size for the squad information.
        /// </summary>
        public SizeF SquadSize => new SizeF((float)Raw["squad_size"][0], (float)Raw["squad_size"][1]);


        /// <summary>
        /// The position of the player information.
        /// </summary>
        public PointF PlayerPosition => new PointF((float)Raw["player_position"][0], (float)Raw["player_position"][1]);
        /// <summary>
        /// Size of the player information.
        /// </summary>
        public SizeF PlayerSize => new SizeF((float)Raw["player_size"][0], (float)Raw["player_size"][1]);
        /// <summary>
        /// Offset of the ammo.
        /// </summary>
        public PointF AmmoOffset => new PointF((float)Raw["ammo_offset"][0], (float)Raw["ammo_offset"][1]);
        /// <summary>
        /// Size for the weapon images.
        /// </summary>
        public SizeF WeaponSize => new SizeF((float)Raw["weapon_size"][0], (float)Raw["weapon_size"][1]);
        /// <summary>
        /// The size of the weapon background
        /// </summary>
        public SizeF WeaponBackground => new SizeF(PlayerSize.Width - CommonSpacing.Width - SquaredBackground.Width, PlayerSize.Height);
        /// <summary>
        /// The position of the player information.
        /// </summary>
        public PointF PlayerInformation => new PointF(PlayerPosition.X + SquaredBackground.Width + CommonSpacing.Width, PlayerPosition.Y);
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public PointF PrimaryIcon => new PointF(PlayerPosition.X, PlayerPosition.Y + CommonSpacing.Height + SquaredBackground.Height);
        /// <summary>
        /// The position of the icon for the primary weapon.
        /// </summary>
        public PointF SecondaryIcon => new PointF(PlayerPosition.X, PlayerPosition.Y + (CommonSpacing.Height * 2) + (SquaredBackground.Height * 2));
        /// <summary>
        /// The position of the ammo for the primary weapon.
        /// </summary>
        public PointF PrimaryBackground => new PointF(PrimaryIcon.X + SquaredBackground.Width + CommonSpacing.Width, PrimaryIcon.Y);
        /// <summary>
        /// The position of the ammo for the secondary weapon.
        /// </summary>
        public PointF SecondaryBackground => new PointF(SecondaryIcon.X + SquaredBackground.Width + CommonSpacing.Width, SecondaryIcon.Y);
        /// <summary>
        /// The position of the primary ammo counter.
        /// </summary>
        public PointF PrimaryAmmo => new PointF(PrimaryBackground.X + AmmoOffset.X, PrimaryBackground.Y + AmmoOffset.Y);
        /// <summary>
        /// The position of the secondary ammo counter.
        /// </summary>
        public PointF SecondaryAmmo => new PointF(SecondaryBackground.X + AmmoOffset.X, SecondaryBackground.Y + AmmoOffset.Y);
        /// <summary>
        /// The position of the primary weapon background.
        /// </summary>
        public PointF PrimaryWeapon => new PointF(PrimaryBackground.X + CommonSpacing.Width + SquaredBackground.Width, PrimaryBackground.Y);
        /// <summary>
        /// The position of the secondary weapon background.
        /// </summary>
        public PointF SecondaryWeapon => new PointF(SecondaryBackground.X + CommonSpacing.Width + SquaredBackground.Width, SecondaryBackground.Y);
        

        /// <summary>
        /// Size for the dividers on the health bars.
        /// </summary>
        public SizeF DividerSize => new SizeF((float)Raw["divider_size"][0], (float)Raw["divider_size"][1]);
        /// <summary>
        /// Position of the health dividers.
        /// </summary>
        public SizeF DividerPosition => new SizeF((float)Raw["divider_position"][0], (float)Raw["divider_position"][1]);
        /// <summary>
        /// Size for the squad health bars.
        /// </summary>
        public SizeF SquadHealthSize => new SizeF((float)Raw["squad_health_size"][0], (float)Raw["squad_health_size"][1]);
        /// <summary>
        /// Position of the squad health bar.
        /// </summary>
        public SizeF SquadHealthPos => new SizeF((float)Raw["squad_health_position"][0], (float)Raw["squad_health_position"][1]);
        /// <summary>
        /// Size of the player health bar.
        /// </summary>
        public SizeF PlayerHealthSize => new SizeF((float)Raw["player_health_size"][0], (float)Raw["player_health_size"][1]);
        /// <summary>
        /// Position of the player health bar.
        /// </summary>
        public SizeF PlayerHealthPos => new SizeF((float)Raw["player_health_position"][0], (float)Raw["player_health_position"][1]);
        
        /// <summary>
        /// Size for the health markers.
        /// </summary>
        public SizeF DeadMarker => new SizeF((float)Raw["dead_marker"][0], (float)Raw["dead_marker"][1]);

        /// <summary>
        /// The RAW Configuration.
        /// </summary>
        private JObject Raw { get; set; }

        /// <summary>
        /// Loads up the configuration from "GGO.Shared.json"
        /// </summary>
        public Configuration(string Location)
        {
            // Read all of the text that is in the file
            string Content = File.ReadAllText(Location + "\\GGO.Shared.json");
            // Load it on the parser
            Raw = JObject.Parse(Content);
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
    }
}
