using GTA;
using System;

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
        /// Loads up a INI file that contains our configuration.
        /// </summary>
        /// <param name="ConfigFile">The configuration file.</param>
        /// <param name="Base">The base of our configuration.</param>
        public Configuration(string ConfigFile, string Base)
        {
            Raw = ScriptSettings.Load(ConfigFile);
            ConfigBase = Base;
        }
    }
}
