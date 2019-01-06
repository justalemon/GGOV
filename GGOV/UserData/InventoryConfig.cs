using GTA.Native;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GGO.UserData
{
    /// <summary>
    /// Class that contains the configuration for the inventory.
    /// </summary>
    public class InventoryConfig
    {
        /// <summary>
        /// If the Custom inventory should be enabled or disabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } 
        /// <summary>
        /// Weapons used to populate the inventory.
        /// </summary>
        [JsonProperty("weapons")]
        public List<WeaponHash> Weapons { get; set; }
        /// <summary>
        /// The player gender.
        /// </summary>
        [JsonProperty("gender")]
        public Gender PlayerGender { get; set; }

        /// <summary>
        /// X position of the inventory background.
        /// </summary>
        [JsonProperty("background_x")]
        public float BackgroundX { get; set; }
        /// <summary>
        /// Y position of the inventory background.
        /// </summary>
        [JsonProperty("background_y")]
        public float BackgroundY { get; set; }
        /// <summary>
        /// Width of the inventory background.
        /// </summary>
        [JsonProperty("background_width")]
        public float BackgroundWidth { get; set; }
        /// <summary>
        /// Height of the inventory background.
        /// </summary>
        [JsonProperty("background_height")]
        public float BackgroundHeight { get; set; }
        /// <summary>
        /// Width of the player information.
        /// </summary>
        [JsonProperty("info_width")]
        public float InfoWidth { get; set; }
        /// <summary>
        /// Height of the player information.
        /// </summary>
        [JsonProperty("info_height")]
        public float InfoHeight { get; set; }

        /// <summary>
        /// Y position of the player rectangle.
        /// </summary>
        [JsonProperty("player_x")]
        public float PlayerX { get; set; }
        /// <summary>
        /// Y position of the player rectangle.
        /// </summary>
        [JsonProperty("player_y")]
        public float PlayerY { get; set; }
        /// <summary>
        /// Width of the player rectangle.
        /// </summary>
        [JsonProperty("player_width")]
        public float PlayerWidth { get; set; }
        /// <summary>
        /// Height of the player rectangle.
        /// </summary>
        [JsonProperty("player_height")]
        public float PlayerHeight { get; set; }
        /// <summary>
        /// X position of the gender icon.
        /// </summary>
        [JsonProperty("gender_x")]
        public float GenderX { get; set; }
        /// <summary>
        /// Y position of the gender icon.
        /// </summary>
        [JsonProperty("gender_y")]
        public float GenderY { get; set; }
        /// <summary>
        /// Width of the gender icon.
        /// </summary>
        [JsonProperty("gender_width")]
        public float GenderWidth { get; set; }
        /// <summary>
        /// Height of the gender icon.
        /// </summary>
        [JsonProperty("gender_height")]
        public float GenderHeight { get; set; }
        /// <summary>
        /// X position of the player name.
        /// </summary>
        [JsonProperty("name_x")]
        public float NameX { get; set; }
        /// <summary>
        /// Y position of the player name.
        /// </summary>
        [JsonProperty("name_y")]
        public float NameY { get; set; }

        /// <summary>
        /// X position of the inventory health bar.
        /// </summary>
        [JsonProperty("health_x")]
        public float HealthX { get; set; }
        /// <summary>
        /// Y position of the inventory health bar.
        /// </summary>
        [JsonProperty("health_y")]
        public float HealthY { get; set; }
        /// <summary>
        /// Width of the inventory health bar.
        /// </summary>
        [JsonProperty("health_width")]
        public float HealthWidth { get; set; }
        /// <summary>
        /// Height of the inventory health bar.
        /// </summary>
        [JsonProperty("health_height")]
        public float HealthHeight { get; set; }
        /// <summary>
        /// X position of the "Life" text.
        /// </summary>
        [JsonProperty("life_x")]
        public float LifeX { get; set; }
        /// <summary>
        /// Y position of the "Life" text.
        /// </summary>
        [JsonProperty("life_y")]
        public float LifeY { get; set; }
        
        /// <summary>
        /// X position of the "Arms" text.
        /// </summary>
        [JsonProperty("arms_x")]
        public float ArmsX { get; set; }
        /// <summary>
        /// Y position of the "Arms" text.
        /// </summary>
        [JsonProperty("arms_y")]
        public float ArmsY { get; set; }
        /// <summary>
        /// X position of the weapons.
        /// </summary>
        [JsonProperty("weapon_x")]
        public float WeaponX { get; set; }
        /// <summary>
        /// Y position of the weapons.
        /// </summary>
        [JsonProperty("weapon_y")]
        public float WeaponY { get; set; }
        /// <summary>
        /// Width of the inventory weapons.
        /// </summary>
        [JsonProperty("weapon_width")]
        public float WeaponWidth { get; set; }
        /// <summary>
        /// Height of the inventory weapons.
        /// </summary>
        [JsonProperty("weapon_height")]
        public float WeaponHeight { get; set; }
        /// <summary>
        /// Separation between inventory weapons.
        /// </summary>
        [JsonProperty("weapon_spacing")]
        public float WeaponSpacing { get; set; }
        /// <summary>
        /// X offset of the weapon rectangles.
        /// </summary>
        [JsonProperty("weapon_rectangle_x")]
        public float WeaponRectangleX { get; set; }
        /// <summary>
        /// Y offset of the weapon rectangles.
        /// </summary>
        [JsonProperty("weapon_rectangle_y")]
        public float WeaponRectangleY { get; set; }
        /// <summary>
        /// Width of the weapon rectangles.
        /// </summary>
        [JsonProperty("weapon_rectangle_width")]
        public float WeaponRectangleWidth { get; set; }
        /// <summary>
        /// Height of the weapon rectangles.
        /// </summary>
        [JsonProperty("weapon_rectangle_height")]
        public float WeaponRectangleHeight { get; set; }
    }
}
