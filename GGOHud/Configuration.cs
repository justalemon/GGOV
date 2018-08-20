using GTA;

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
        /// </summary>
        public bool Debug
        {
            get
            {
                return RawSettings.GetValue(ConfigBase, "Debug", false);
            }
        }

        public Configuration(string ConfigFile, string Base)
        {
            RawSettings = ScriptSettings.Load(ConfigFile);
            ConfigBase = Base;
        }
    }
}
