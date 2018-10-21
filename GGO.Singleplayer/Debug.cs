using GGO.Common;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GGO.Singleplayer
{
    public partial class Debug : Form
    {
        public Debug(Configuration Config)
        {
            InitializeComponent();

            // Get the current GGOHud version
            Version Current = Assembly.GetExecutingAssembly().GetName().Version;

            // Set the icon as the GTA V executable
            Icon GameIcon = Icon.ExtractAssociatedIcon("GTA5.exe");
            Icon = GameIcon;

            // And then set the corresponding texts on the fields
            Version.Text = "Running GGOHud v" + Current;
            PlayerName.Text = "Custom name: " + Config.Name;
            HudDisabled.Text = "HUD Disabled: " + Config.DisableHud;

            ElementsRelative.Text = "elements_relative: " + Config.CommonSpace;
            DividerSize.Text = "divider_size: " + Config.DividerSize;
            DividerPosition.Text = "divider_pos: " + Config.DividerPosition;
            NamePosition.Text = "name_pos: " + Config.NamePosition;

            IconBackgroundSize.Text = "icon_background_size: " + Config.SquaredBackground;
            IconPosition.Text = "icon_relative_pos: " + Config.IconPosition;

            SquadPosition.Text = "squad_general_pos: " + Config.SquadPosition;
            SquadInfoSize.Text = "squad_info_size: " + Config.SquadInfoSize;
            SquadHealthSize.Text = "squad_health_size: " + Config.SquadHealthSize;
            SquadHealthPos.Text = "squad_health_pos: " + Config.SquadHealthPos;

            PlayerPosition.Text = "player_general_pos: " + Config.PlayerIcon;
            PlayerInfoSize.Text = "player_info_size: " + Config.PlayerInfoSize;
            PlayerHealthSize.Text = "player_health_size: " + Config.PlayerHealthSize;
            PlayerHealthPosition.Text = "player_health_pos: " + Config.PlayerHealthPos;

            WeaponImageSize.Text = "weapon_image_size: " + Config.WeaponImageSize;
            AmmoPosition.Text = "ammo_offset_pos: " + Config.AmmoOffset;
        }
    }
}
