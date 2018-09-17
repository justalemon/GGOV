using GTA;
using System;
using System.Drawing;

namespace GGO.Singleplayer
{
    /// <summary>
    /// Configuration parser for the script.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The raw ScriptSettings instance.
        /// </summary>
        private static ScriptSettings Raw = ScriptSettings.Load("scripts\\GGO.Singleplayer.ini");

        /// <summary>
        /// If the debug mode should be enabled.
        /// This check for "Debug" on the config file and the environment variable "DevGTA".
        /// </summary>
        public static bool Debug = Raw.GetValue("GGO", "Debug", false) || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
        /// <summary>
        /// The player desired name.
        /// </summary>
        public static string Name = Raw.GetValue("GGO", "Name", "default");
        /// <summary>
        /// If the HUD and Radar should be disabled
        /// </summary>
        public static bool HudDisabled = Raw.GetValue("GGO", "HudDisabled", false);
        /// <summary>
        /// The size of the icon images without counting the background.
        /// </summary>
        public static Size IconImage = Common.Math.SizeFromConfig(Raw, "IconImage");
        /// <summary>
        /// The position of the icon background relative to the image.
        /// </summary>
        public static Size IconBackground = Common.Math.SizeFromConfig(Raw, "IconBackground");
        /// <summary>
        /// The space difference between the image and the background for the icons.
        /// </summary>
        public static Size IconRelative = Common.Math.SizeFromConfig(Raw, "IconRelative");
        /// <summary>
        /// The position of the squad related elements, starting by the first icon.
        /// </summary>
        public static Point SquadPosition = Common.Math.PointFromConfig(Raw, "SquadPosition");
        /// <summary>
        /// The relative separation between the squad elements.
        /// </summary>
        public static Size SquadRelative = Common.Math.SizeFromConfig(Raw, "SquadRelative");
        /// <summary>
        /// The size of the squad friend information.
        /// </summary>
        public static Size SquadInfoSize = Common.Math.SizeFromConfig(Raw, "SquadInfoSize");
        /// <summary>
        /// The size of the health bar based on the squad background.
        /// </summary>
        public static Size HealthBarSize = Common.Math.SizeFromConfig(Raw, "HealthBarSize");
        /// <summary>
        /// The position of the health bar based on the background.
        /// </summary>
        public static Size HealthBarOffset = Common.Math.SizeFromConfig(Raw, "HealthBarOffset");
        /// <summary>
        /// The offset for the player name related to the background.
        /// </summary>
        public static Size PlayerNameOffset = Common.Math.SizeFromConfig(Raw, "PlayerNameOffset");
        /// <summary>
        /// The offset for the player name related to the background.
        /// </summary>
        public static Size HealthDividerSize = Common.Math.SizeFromConfig(Raw, "HealthDividerSize");
        /// <summary>
        /// The offset for the health bar dividers.
        /// </summary>
        public static Size HealthDividerOffset = Common.Math.SizeFromConfig(Raw, "HealthDividerOffset");
    }
}
