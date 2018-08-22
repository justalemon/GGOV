using GTA;
using System;

namespace GGOHud
{
    public class Configuration
    {
        /// <summary>
        /// The raw ScriptSettings instance.
        /// </summary>
        private ScriptSettings RawSettings;
        /// <summary>
        /// The base for the config (aka the base element).
        /// </summary>
        private string ConfigBase;
        /// <summary>
        /// If the debug mode should be enabled.
        /// This check for "Debug" on the config file and the environment variable "DevGTA"
        /// </summary>
        public bool Debug
        {
            get
            {
                return RawSettings.GetValue(ConfigBase, "Debug", false) || Environment.GetEnvironmentVariable("DevGTA", EnvironmentVariableTarget.User) == "true";
            }
        }

        public Configuration(string ConfigFile, string Base)
        {
            RawSettings = ScriptSettings.Load(ConfigFile);
            ConfigBase = Base;
        }
    }
}
