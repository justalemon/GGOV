using GTA;
using GTA.Native;
using System.Drawing;

namespace GGO.Common
{
    public static class PedExtension
    {
        /// <summary>
        /// Returns the name of a ped based on the model.
        /// </summary>
        /// <param name="ThePed">The ped to check.</param>
        /// <param name="Custom">The custom name for the player ped.</param>
        /// <returns>The name that corresponds for the ped.</returns>
        public static string Name(this Ped ThePed, Configuration Config)
        {
            // If the ped is the player and the custom name has not been changed
            // Return the Social Club username
            if (Config.Name == "default" && ThePed == Game.Player.Character)
            {
                return Game.Player.Name;
            }
            // If the ped is the player and a custom name has been added
            // Return that custom name
            else if (ThePed == Game.Player.Character)
            {
                return Config.Name;
            }
            // If is not the player but there is a custom name available
            // Return that ped name
            else if (Config.PedNames.IsNameDefined(ThePed.Model.Hash))
            {
                return Config.PedNames.GetName(ThePed.Model.Hash);
            }
            // If none of the previous ones work
            // Return the hash as a string
            else
            {
                return ThePed.Model.Hash.ToString();
            }
        }
    }
}
