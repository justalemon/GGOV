using GGOHud.Properties;
using System.Collections.Generic;

namespace GGOHud
{
    /// <summary>
    /// A class with information like Names and Images.
    /// </summary>
    class Data
    {
        /// <summary>
        /// A dictionary with the UI images.
        /// </summary>
        public static Dictionary<string, string> Images = new Dictionary<string, string>
        {
            { "PlayerIcon", Tools.ResourceToFile(Resources.ImagePlayer) },
            { "PrimaryIcon", Tools.ResourceToFile(Resources.ImageGun) },
            { "SecondaryIcon", Tools.ResourceToFile(Resources.ImageGun) },
            { "SquadIcon1", Tools.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon2", Tools.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon3", Tools.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon4", Tools.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon5", Tools.ResourceToFile(Resources.ImagePlayer) }
        };
        /// <summary>
        /// A dictionary with the character names.
        /// </summary>
        public static Dictionary<int, string> Names = new Dictionary<int, string>
        {
            { -597926235, "Cheng" },
            { 2089096292, "Translator" },
            { 1240128502, "Chef" },
            { 1706635382, "Lamar" },
            { 915948376, "Stretch" },
            { -1643617475, "A. Owner" }
        };
    }
}
