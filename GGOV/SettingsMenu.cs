using GGO.Items;
using GTA.UI;
using LemonUI.Menus;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GGO
{
    public class Configuration
    {
        [JsonProperty("equip")]
        public bool Equip { get; set; } = true;
        [JsonProperty("squadx")]
        public float SquadX { get; set; } = 0;
        [JsonProperty("squady")]
        public float SquadY { get; set; } = 0;
        [JsonProperty("playerx")]
        public float PlayerX { get; set; } = 0;
        [JsonProperty("playery")]
        public float PlayerY { get; set; } = 0;
        [JsonProperty("deatnmarkers")]
        public bool DeathMarkers { get; set; } = true;
    }

    /// <summary>
    /// The Main Menu of the Settings.
    /// </summary>
    public class SettingsMenu : NativeMenu
    {
        #region Properties

        private static readonly string path = Path.Combine(GGO.location, "GGOV", "Config.json");
        public NativeCheckboxItem EquipWeapons { get; } = new NativeCheckboxItem("Equip Inventory Weapons", "Enabled: Marks the Weapons as Primary or Secondary, allowing you to switch between your Primary, Secondary and Fists with 1, 2 and 3 respectively (like Apex Legends).~n~~n~Disabled: Equips the Weapons directly. Requires you to open the Inventory every time that you want to change weapons.", true);
        public FloatSelectorItem SquadX { get; } = new FloatSelectorItem("Squad Members: X", "The X value of the Squad Health Information.", 103);
        public FloatSelectorItem SquadY { get; } = new FloatSelectorItem("Squad Members: Y", "The Y value of the Squad Health Information.", 66);
        public FloatSelectorItem PlayerX { get; } = new FloatSelectorItem("Player Info: X", "The X value of the Player Information.", -388);
        public FloatSelectorItem PlayerY { get; } = new FloatSelectorItem("Player Info: Y", "The Y value of the Player Information.", -232);
        public NativeCheckboxItem DeathMarkers { get; } = new NativeCheckboxItem("Enable Death Markers", "Enables the Death Markers shown when the Peds around you die.", true);
        public NativeItem Save { get; } = new NativeItem("Save", "Saves all of the current settings.");

        #endregion

        #region Constructor

        public SettingsMenu() : base("", "Gun Gale Online Settings", "", null)
        {
            // Start by loading the configuration
            if (File.Exists(path))
            {
                string contents = File.ReadAllText(path);
                Configuration config = JsonConvert.DeserializeObject<Configuration>(contents);

                EquipWeapons.Checked = config.Equip;
                SquadX.SelectedItem = config.SquadX;
                SquadY.SelectedItem = config.SquadY;
                PlayerX.SelectedItem = config.PlayerX;
                PlayerY.SelectedItem = config.PlayerY;
                DeathMarkers.Checked = config.DeathMarkers;
            }

            // Set the options of the menu
            Alignment = Alignment.Right;
            ResetCursorWhenOpened = false;
            // Add the items and submenus
            Add(EquipWeapons);
            Add(SquadX);
            Add(SquadY);
            Add(PlayerX);
            Add(PlayerY);
            Add(DeathMarkers);
            Add(Save);
            // Subscribe the events
            EquipWeapons.CheckboxChanged += EquipWeapons_CheckboxChanged;
            SquadX.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            SquadY.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            PlayerX.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            PlayerY.ValueChanged += (sender, e) => GGO.Squad.Recalculate();
            Save.Selected += Save_Selected;
        }

        #endregion

        #region Events

        private void EquipWeapons_CheckboxChanged(object sender, EventArgs e)
        {
            // If the feature was disabled, clear the current weapons
            if (!EquipWeapons.Checked)
            {
                GGO.weaponPrimary = 0;
                GGO.weaponSecondary = 0;
            }
        }

        private void Save_Selected(object sender, SelectedEventArgs e)
        {
            Configuration config = new Configuration
            {
                Equip = EquipWeapons.Checked,
                SquadX = SquadX.SelectedItem,
                SquadY = SquadY.SelectedItem,
                PlayerX = PlayerX.SelectedItem,
                PlayerY = PlayerY.SelectedItem,
                DeathMarkers = DeathMarkers.Checked
            };

            string contents = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, contents);

            Notification.Show("Your configuration was ~g~saved~s~!");
        }

        #endregion
    }
}
