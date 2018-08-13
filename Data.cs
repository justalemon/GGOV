using GGOHud.Properties;
using GGOHud.Tools;
using System.Collections.Generic;

namespace GGOHud
{
    /// <summary>
    /// A class with information like Names and Images.
    /// </summary>
    class Data
    {
        /// <summary>
        /// Animations used by jedijosh920's Dual Wield mod.
        /// Note: I got used to 'list = [["a", "b"], ["1", "2"]]' in Python, so...
        /// </summary>
        public static List<List<string>> DualWieldAnims = new List<List<string>> {
            new List<string> { "holster", "unholster_2h" },
            new List<string> { "holster", "melee@holster" },
            new List<string> { "anim@veh@armordillo@turret@base", "sit_aim_down" }
        };
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static readonly List<int> BannedWeapons = new List<int> { 1595662460, 1548507267, -1609580060, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), Stun Gun (Type), SMG.
        /// </summary>
        public static readonly List<int> SecondaryWeapons = new List<int> { 416676503, 690389602, -957766203 };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type), Boxer (Type).
        /// </summary>
        public static readonly List<int> MeleeWeapons = new List<int> { -1609580060, -728555052 };
        /// <summary>
        /// A dictionary with the UI images.
        /// </summary>
        public static Dictionary<string, string> Icons = new Dictionary<string, string>
        {
            { "PlayerIcon", Images.ResourceToFile(Resources.ImagePlayer) },
            { "PrimaryIcon", Images.ResourceToFile(Resources.ImageGun) },
            { "SecondaryIcon", Images.ResourceToFile(Resources.ImageGun) },
            { "SquadIcon1", Images.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon2", Images.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon3", Images.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon4", Images.ResourceToFile(Resources.ImagePlayer) },
            { "SquadIcon5", Images.ResourceToFile(Resources.ImagePlayer) }
        };
        /// <summary>
        /// A dictionary with the character names.
        /// </summary>
        public static Dictionary<int, string> Names = new Dictionary<int, string>
        {
            // Main Missions
            { 1830688247, "Amanda" },
            { -1111799518, "Brad" },
            { -520477356, "Casey" },
            { 1240128502, "Chef" },
            { 365775923, "Dave" },
            { -1674727288, "Dom" },
            { 351016938, "Chop" },
            { -1313761614, "Floyd" },
            { -1692214353, "Franklin" },
            { 1459905209, "Jimmy" },
            { 1302784073, "Lester" },
            { 1706635382, "Lamar" },
            { 225514697, "Michael" },
            { -304305299, "Mr K" },
            { -1124046095, "Ron" },
            { 915948376, "Stretch" },
            { -566941131, "Tracy" },
            { -597926235, "Tao Cheng" },
            { 2089096292, "Translator" },
            { -1686040670, "Trevor" },
            { -1835459726, "Wade" },
            // Heists
            // Missing Ones: Karim, Gus, Norm, McReary, Daryl, Hugh, Karl, Feltz
            { 712602007, "Eddie" },
            { 994527967, "Eddie" },
            { -409745176, "Taliana" },
            { 1209091352, "Norm" },
            { 357551935, "Paige" },
            { 1401530684, "Feltz" },
        };
    }
}
