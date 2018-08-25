using GTA;
using System;
using System.Drawing;

namespace GGOHud
{
    /// <summary>
    /// Configuration parser for the script.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The raw ScriptSettings instance.
        /// </summary>
        private ScriptSettings Raw;
        /// <summary>
        /// The base for the config (aka the base element).
        /// </summary>
        private string ConfigBase;

        /// <summary>
        /// If the debug mode should be enabled.
        /// This check for "Debug" on the config file and the environment variable "DevGTA".
        /// </summary>
        public bool Debug
        {
            get
            {
                return Raw.GetValue(ConfigBase, "Debug", false) || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
            }
        }
        /// <summary>
        /// The size of the icon images without counting the background.
        /// </summary>
        public Size IconImage
        {
            get
            {
                return new Size(Absolute(Raw.GetValue(ConfigBase, "IconImageW", 2.7f), Game.ScreenResolution.Width),
                                Absolute(Raw.GetValue(ConfigBase, "IconImageH", 5f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The position of the icon background relative to the image.
        /// </summary>
        public Size IconBackground
        {
            get
            {
                return new Size(Absolute(Raw.GetValue(ConfigBase, "IconBackgroundW", 2.85f), Game.ScreenResolution.Width),
                                Absolute(Raw.GetValue(ConfigBase, "IconBackgroundH", 5.3f), Game.ScreenResolution.Height));
            }
        }
        /// <summary>
        /// The space difference between the image and the background for the icons.
        /// </summary>
        public Size IconRelative
        {
            get
            {
                return new Size(Absolute(Raw.GetValue(ConfigBase, "IconRelativeW", 0.1f), Game.ScreenResolution.Width),
                                Absolute(Raw.GetValue(ConfigBase, "IconRelativeH", 1f), Game.ScreenResolution.Height));
            }
        }

        /// <summary>
        /// Loads up a INI file that contains our configuration.
        /// </summary>
        /// <param name="ConfigFile">The configuration file.</param>
        /// <param name="Base">The base of our configuration.</param>
        public Configuration(string ConfigFile, string Base)
        {
            Raw = ScriptSettings.Load(ConfigFile);
            ConfigBase = Base;
        }

        /// <summary>
        /// Converts a relative screen position into an absolute one.
        /// </summary>
        /// <returns></returns>
        private static int Absolute(float Relative, int Value)
        {
            return Convert.ToInt32((Value / 100) * Relative);
        }
    }
}
