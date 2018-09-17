using GTA;
using System.Collections.Generic;

namespace GGO.Common
{
    public static class PedExtension
    {
        public static Dictionary<int, string> Names = new Dictionary<int, string>
        {
            // Missing Ones: Karim, Gus, Norm, McReary, Daryl, Hugh, Karl, Feltz
            // Main Characters
            // Thanks SWolfie for remembering this!
            { -1686040670, "Trevor" },
            { 225514697, "Michael" },
            { -1692214353, "Franklin" },
            // Missions
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
            { -304305299, "Mr K" },
            { -1124046095, "Ron" },
            { 915948376, "Stretch" },
            { -566941131, "Tracy" },
            { -597926235, "Tao Cheng" },
            { 2089096292, "Translator" },
            { -1835459726, "Wade" },
            // Heists
            { 712602007, "Eddie" },
            { 994527967, "Eddie" },
            { -409745176, "Taliana" },
            { 1209091352, "Norm" },
            { 357551935, "Paige" },
            { 1401530684, "Feltz" },
        };

        public static string Name(this Ped ThePed, string Custom = "default")
        {
            if (Custom == "Default" && ThePed == Game.Player.Character)
            {
                return Game.Player.Name;
            }
            else if (ThePed == Game.Player.Character)
            {
                return Custom;
            }
            else if (Names.ContainsKey(ThePed.Model.Hash))
            {
                return Names[ThePed.Model.Hash];
            }
            else
            {
                return ThePed.Model.Hash.ToString();
            }
        }
    }
}
