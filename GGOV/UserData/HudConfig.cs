using GTA.Native;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GGO.UserData
{
    /// <summary>
    /// Class that contains the configuration for the Screen HUD.
    /// </summary>
    public class HudConfig
    {
        /// <summary>
        /// If the Custom HUD should be enabled or disabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// If the colliding vanilla HUD elements should be enabled or disabled.
        /// </summary>
        [JsonProperty("collisions")]
        public bool Collisions { get; set; }
        /// <summary>
        /// If the vanilla radar should be enabled or disabled.
        /// </summary>
        [JsonProperty("radar")]
        public bool Radar { get; set; }
        /// <summary>
        /// If the squad section should be shown.
        /// </summary>
        [JsonProperty("squad")]
        public bool Squad { get; set; }
        /// <summary>
        /// If the vehicle information should be shown.
        /// </summary>
        [JsonProperty("vehicle_info")]
        public bool VehicleInfo { get; set; }
        /// <summary>
        /// If the over-the-head dead markers should be shown.
        /// </summary>
        [JsonProperty("dead_markers")]
        public bool DeadMarkers { get; set; }


        /// <summary>
        /// X spacing between UI elements.
        /// </summary>
        [JsonProperty("common_x")]
        public float CommonX { get; set; }
        /// <summary>
        /// Y spacing between UI elements.
        /// </summary>
        [JsonProperty("common_y")]
        public float CommonY { get; set; }
        /// <summary>
        /// Width of the common squared backgrounds.
        /// </summary>
        [JsonProperty("square_width")]
        public float SquareWidth { get; set; }
        /// <summary>
        /// Height of the common squared backgrounds.
        /// </summary>
        [JsonProperty("square_height")]
        public float SquareHeight { get; set; }

        /// <summary>
        /// X position of the icons relative to the squared background.
        /// </summary>
        [JsonProperty("icon_x")]
        public float IconX { get; set; }
        /// <summary>
        /// Y position of the icons relative to the squared background.
        /// </summary>
        [JsonProperty("icon_y")]
        public float IconY { get; set; }
        /// <summary>
        /// Width of the icons.
        /// </summary>
        [JsonProperty("icon_width")]
        public float IconWidth { get; set; }
        /// <summary>
        /// Height of the icons.
        /// </summary>
        [JsonProperty("icon_height")]
        public float IconHeight { get; set; }

        /// <summary>
        /// X position of the squad members.
        /// </summary>
        [JsonProperty("squad_x")]
        public float SquadX { get; set; }
        /// <summary>
        /// Y position of the squad members.
        /// </summary>
        [JsonProperty("squad_y")]
        public float SquadY { get; set; }
        /// <summary>
        /// Width of the squad information background.
        /// </summary>
        [JsonProperty("squad_width")]
        public float SquadWidth { get; set; }
        /// <summary>
        /// Height of the squad information background.
        /// </summary>
        [JsonProperty("squad_height")]
        public float SquadHeight { get; set; }
        /// <summary>
        /// X position of the squad member name relative to the background.
        /// </summary>
        [JsonProperty("squad_name_x")]
        public float SquadNameX { get; set; }
        /// <summary>
        /// Y position of the squad member name relative to the background.
        /// </summary>
        [JsonProperty("squad_name_y")]
        public float SquadNameY { get; set; }
        /// <summary>
        /// X position of the squad health bar relative to the background.
        /// </summary>
        [JsonProperty("squad_health_x")]
        public float SquadHealthX { get; set; }
        /// <summary>
        /// Y position of the squad health bar relative to the background.
        /// </summary>
        [JsonProperty("squad_health_y")]
        public float SquadHealthY { get; set; }
        /// <summary>
        /// Width of the squad health bar.
        /// </summary>
        [JsonProperty("squad_health_width")]
        public float SquadHealthWidth { get; set; }
        /// <summary>
        /// Height of the squad health bar.
        /// </summary>
        [JsonProperty("squad_health_height")]
        public float SquadHealthHeight { get; set; }

        /// <summary>
        /// X position of the player information.
        /// </summary>
        [JsonProperty("player_x")]
        public float PlayerX { get; set; }
        /// <summary>
        /// Y position of the player information.
        /// </summary>
        [JsonProperty("player_y")]
        public float PlayerY { get; set; }
        /// <summary>
        /// Width of the player information spaces.
        /// </summary>
        [JsonProperty("player_width")]
        public float PlayerWidth { get; set; }
        /// <summary>
        /// Height of the player information spaces.
        /// </summary>
        [JsonProperty("player_height")]
        public float PlayerHeight { get; set; }
        /// <summary>
        /// X position of the player health bar relative to the background.
        /// </summary>
        [JsonProperty("player_health_x")]
        public float PlayerHealthX { get; set; }
        /// <summary>
        /// Y position of the player health bar relative to the background.
        /// </summary>
        [JsonProperty("player_health_y")]
        public float PlayerHealthY { get; set; }
        /// <summary>
        /// Width of the player health bar.
        /// </summary>
        [JsonProperty("player_health_width")]
        public float PlayerHealthWidth { get; set; }
        /// <summary>
        /// Height of the player health bar.
        /// </summary>
        [JsonProperty("player_health_height")]
        public float PlayerHealthHeight { get; set; }
        /// <summary>
        /// X position of the weeapon ammo relative to the background.
        /// </summary>
        [JsonProperty("ammo_x")]
        public float AmmoX { get; set; }
        /// <summary>
        /// Y position of the weeapon ammo relative to the background.
        /// </summary>
        [JsonProperty("ammo_y")]
        public float AmmoY { get; set; }
        /// <summary>
        /// X position of the weapon image relative to the background.
        /// </summary>
        [JsonProperty("weapon_x")]
        public float WeaponX { get; set; }
        /// <summary>
        /// Y position of the weapon image relative to the background.
        /// </summary>
        [JsonProperty("weapon_y")]
        public float WeaponY { get; set; }
        /// <summary>
        /// Width of the weapon image.
        /// </summary>
        [JsonProperty("weapon_width")]
        public float WeaponWidth { get; set; }
        /// <summary>
        /// Height of the weapon image.
        /// </summary>
        [JsonProperty("weapon_height")]
        public float WeaponHeight { get; set; }

        /// <summary>
        /// X position of the health dividers relative to the bar.
        /// </summary>
        [JsonProperty("divider_x")]
        public float DividerX { get; set; }
        /// <summary>
        /// Y position of the health dividers relative to the bar.
        /// </summary>
        [JsonProperty("divider_y")]
        public float DividerY { get; set; }
        /// <summary>
        /// Width of the health dividers.
        /// </summary>
        [JsonProperty("divider_width")]
        public float DividerWidth { get; set; }
        /// <summary>
        /// Height of the health dividers.
        /// </summary>
        [JsonProperty("divider_height")]
        public float DividerHeight { get; set; }

        /// <summary>
        /// Width of the dead markers.
        /// </summary>
        [JsonProperty("dead_width")]
        public float DeadMarkerWidth { get; set; }
        /// <summary>
        /// Height of the dead markers.
        /// </summary>
        [JsonProperty("dead_height")]
        public float DeadMarkerHeight { get; set; }
    }
}
