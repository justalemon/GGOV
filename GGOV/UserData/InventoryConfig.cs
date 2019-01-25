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
        /// Items used to populate the inventory.
        /// </summary>
        [JsonProperty("items")]
        public List<WeaponHash> Items { get; set; }
        /// <summary>
        /// The player gender.
        /// </summary>
        [JsonProperty("gender")]
        public Gender PlayerGender { get; set; }
        /// <summary>
        /// If the total count of ammo should be shown.
        /// </summary>
        [JsonProperty("ammo_total")]
        public bool AmmoTotal { get; set; }
        /// <summary>
        /// If the count of magazines left should be shown.
        /// </summary>
        [JsonProperty("ammo_mags")]
        public bool AmmoMags { get; set; }

        /// <summary>
        /// If we should automatically add the inventory items and weapons.
        /// </summary>
        [JsonProperty("auto_add")]
        public bool AutoAdd { get; set; }
        /// <summary>
        /// If we should automatically remove the items and weapons that are on the mod.
        /// </summary>
        [JsonProperty("remove_non_listed")]
        public bool RemvoveNonListed { get; set; }

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
        /// X Separation between inventory elements.
        /// </summary>
        [JsonProperty("spacing_x")]
        public float SpacingX { get; set; }
        /// <summary>
        /// Y Separation between inventory elements.
        /// </summary>
        [JsonProperty("spacing_y")]
        public float SpacingY { get; set; }

        /// <summary>
        /// X position of the "Status" text.
        /// </summary>
        [JsonProperty("status_base_x")]
        public float StatusBaseX { get; set; }
        /// <summary>
        /// X position of the current player status.
        /// </summary>
        [JsonProperty("status_current_x")]
        public float StatusCurrentX { get; set; }
        /// <summary>
        /// Y position of the status related text.
        /// </summary>
        [JsonProperty("status_y")]
        public float StatusY { get; set; }

        /// <summary>
        /// X offset of the weapons and item text.
        /// </summary>
        [JsonProperty("text_x")]
        public float TextX { get; set; }
        /// <summary>
        /// Y offset of the weapons and item text.
        /// </summary>
        [JsonProperty("text_y")]
        public float TextY { get; set; }

        /// <summary>
        /// X position of the items.
        /// </summary>
        [JsonProperty("items_x")]
        public float ItemsX { get; set; }
        /// <summary>
        /// Y position of the items.
        /// </summary>
        [JsonProperty("items_y")]
        public float ItemsY { get; set; }
        /// <summary>
        /// Width of the item rectangles.
        /// </summary>
        [JsonProperty("items_width")]
        public float ItemsWidth { get; set; }
        /// <summary>
        /// Height of the item rectangles.
        /// </summary>
        [JsonProperty("items_height")]
        public float ItemsHeight { get; set; }
        /// <summary>
        /// X offset of the item images.
        /// </summary>
        [JsonProperty("items_image_x")]
        public float ItemsImageX { get; set; }
        /// <summary>
        /// Y offset of the item images.
        /// </summary>
        [JsonProperty("items_image_y")]
        public float ItemsImageY { get; set; }
        /// <summary>
        /// Height of the item images.
        /// </summary>
        [JsonProperty("items_image_width")]
        public float ItemsImageWidth { get; set; }
        /// <summary>
        /// Height of the item images.
        /// </summary>
        [JsonProperty("items_image_height")]
        public float ItemsImageHeight { get; set; }
        /// <summary>
        /// X offset of the item separator.
        /// </summary>
        [JsonProperty("items_sep_x")]
        public float ItemsSeparatorX { get; set; }
        /// <summary>
        /// Y offset of the item separator.
        /// </summary>
        [JsonProperty("items_sep_y")]
        public float ItemsSeparatorY { get; set; }
        /// <summary>
        /// Width of the item separator.
        /// </summary>
        [JsonProperty("items_sep_width")]
        public float ItemsSeparatorWidth { get; set; }
        /// <summary>
        /// Height of the item separator.
        /// </summary>
        [JsonProperty("items_sep_height")]
        public float ItemsSeparatorHeight { get; set; }
        /// <summary>
        /// X offset of the quantity of that item.
        /// </summary>
        [JsonProperty("items_quantity_x")]
        public float ItemsQuantityX { get; set; }
        /// <summary>
        /// Y offset of the quantity of that item.
        /// </summary>
        [JsonProperty("items_quantity_y")]
        public float ItemsQuantityY { get; set; }

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
        /// X offset of the weapon images.
        /// </summary>
        [JsonProperty("weapon_image_x")]
        public float WeaponImageX { get; set; }
        /// <summary>
        /// Y offset of the weapon images.
        /// </summary>
        [JsonProperty("weapon_image_y")]
        public float WeaponImageY { get; set; }
        /// <summary>
        /// Width of the weapon images.
        /// </summary>
        [JsonProperty("weapon_image_width")]
        public float WeaponImageWidth { get; set; }
        /// <summary>
        /// Height of the weapon images.
        /// </summary>
        [JsonProperty("weapon_image_height")]
        public float WeaponImageHeight { get; set; }
    }
}
