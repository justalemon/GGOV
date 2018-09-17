using GTA;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;

namespace GGO.Common
{
    public static class PedExtension
    {
        /// <summary>
        /// List of names to show on the HUD.
        /// </summary>
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
        /// <summary>
        /// Color for a ped with health over 100% (stupid but posible).
        /// </summary>
        public static Color HealthStupid = Color.FromArgb(255);
        /// <summary>
        /// Color for a ped with health above 50% and under 100%.
        /// </summary>
        public static Color HealthNormal = Color.FromArgb(255, 230, 230, 230);
        /// <summary>
        /// Color for a ped with health under 49% and over 25%.
        /// </summary>
        public static Color HealthDanger = Color.FromArgb(255, 247, 227, 18);
        /// <summary>
        /// Color for a ped with health under 24%.
        /// </summary>
        public static Color HealthDying = Color.FromArgb(255, 200, 0, 0);

        /// <summary>
        /// Returns a color based on the player health.
        /// </summary>
        /// <param name="ThePed">The ped to check.</param>
        /// <returns>A color that match the current ped health.</returns>
        public static Color HealthColor(this Ped ThePed)
        {
            if (ThePed.HealthPercentage() >= 50 && ThePed.HealthPercentage() <= 100)
            {
                return HealthNormal;
            }
            else if (ThePed.HealthPercentage() <= 50 && ThePed.HealthPercentage() >= 25)
            {
                return HealthDanger;
            }
            else if (ThePed.HealthPercentage() <= 25)
            {
                return HealthDying;
            }
            else
            {
                return HealthStupid;
            }
        }

        /// <summary>
        /// Returns the name of a ped based on the model.
        /// </summary>
        /// <param name="ThePed">The ped to check.</param>
        /// <param name="Custom">The custom name for the player ped.</param>
        /// <returns>The name that corresponds for the ped.</returns>
        public static string Name(this Ped ThePed, string Custom = "default")
        {
            if (Custom == "default" && ThePed == Game.Player.Character)
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

        /// <summary>
        /// Gets the health percentage for the ped.
        /// </summary>
        /// <param name="ThePed">The ped to check.</param>
        /// <returns>The health percentage.</returns>
        public static float HealthPercentage(this Ped ThePed)
        {
            float MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, ThePed);
            float CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, ThePed);
            return (CurrentHealth / MaxHealth) * 100;
        }
    }
}
