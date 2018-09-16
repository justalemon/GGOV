using GTA;
using System;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// Configuration parser for the script.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The raw ScriptSettings instance.
        /// </summary>
        private static ScriptSettings Raw = ScriptSettings.Load("scripts\\GGO.ini");
        /// <summary>
        /// The base for the config (aka the base element).
        /// </summary>
        private static string ConfigBase = "GGO";

        /// <summary>
        /// If the debug mode should be enabled.
        /// This check for "Debug" on the config file and the environment variable "DevGTA".
        /// </summary>
        public static bool Debug
        {
            get
            {
                return Raw.GetValue(ConfigBase, "Debug", false) || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
            }
        }
        /// <summary>
        /// If the HUD and Radar should be disabled
        /// </summary>
        public static bool HudDisabled
        {
            get
            {
                return Raw.GetValue(ConfigBase, "HudDisabled", false);
            }
        }
        /// <summary>
        /// The size of the icon images without counting the background.
        /// </summary>
        public static Size IconImage
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "IconImageW", 2.7f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "IconImageH", 5f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The position of the icon background relative to the image.
        /// </summary>
        public static Size IconBackground
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "IconBackgroundW", 2.85f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "IconBackgroundH", 5.3f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The space difference between the image and the background for the icons.
        /// </summary>
        public static Size IconRelative
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "IconRelativeW", 0.1f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "IconRelativeH", 0.1f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The position of the squad related elements, starting by the first icon.
        /// </summary>
        public static Point SquadPosition
        {
            get
            {
                return new Point(PercentageOf(Raw.GetValue(ConfigBase, "SquadPositionX", 0.1f), Game.ScreenResolution.Width),
                                 PercentageOf(Raw.GetValue(ConfigBase, "SquadPositionY", 0.1f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The relative separation between the squad elements.
        /// </summary>
        public static Size SquadRelative
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "SquadRelativeW", 0.1f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "SquadRelativeH", 0.1f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The size of the squad friend information.
        /// </summary>
        public static Size SquadInfoSize
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "SquadInfoSizeW", 0.1f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "SquadInfoSizeH", 0.1f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The size of the health bar based on the squad background.
        /// </summary>
        public static Size HealthBarSize
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "HealthBarSizeW", 0.1f), Game.ScreenResolution.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "HealthBarSizeH", 0.1f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The position of the health bar based on the background.
        /// </summary>
        public static Size HealthBarOffset
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "HealthBarOffsetW", 0.1f), SquadInfoSize.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "HealthBarOffsetH", 0.1f), SquadInfoSize.Height));
            }
        }
        /// <summary>
        /// The offset for the player name related to the background.
        /// </summary>
        public static Size PlayerNameOffset
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "PlayerNameOffsetW", 0.1f), SquadInfoSize.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "PlayerNameOffsetH", 0.1f), SquadInfoSize.Height));
            }
        }
        /// <summary>
        /// The offset for the player name related to the background.
        /// </summary>
        public static Size HealthDividerSize
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "HealthDividerSizeW", 0.1f), SquadInfoSize.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "HealthDividerSizeH", 0.1f), SquadInfoSize.Height));
            }
        }
        /// <summary>
        /// The offset for the health bar dividers.
        /// </summary>
        public static Size HealthDividerOffset
        {
            get
            {
                return new Size(PercentageOf(Raw.GetValue(ConfigBase, "HealthDividerOffsetW", 0.1f), SquadInfoSize.Width),
                                PercentageOf(Raw.GetValue(ConfigBase, "HealthDividerOffsetH", 0.1f), SquadInfoSize.Height));
            }
        }

        /// <summary>
        /// Calculates the percentage of a number.
        /// </summary>
        /// <returns>The value that corresponds to that percentage.</returns>
        private static int PercentageOf(float Percentage, int Of)
        {
            return Convert.ToInt32((Percentage / 100) * Of);
        }
    }
}
