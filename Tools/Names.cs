using GTA;

namespace GGOHud.Tools
{
    class Names
    {
        /// <summary>
        /// Gets the Player name.
        /// </summary>
        /// <returns>The Player desired name.</returns>
        public static string Player()
        {
            // If the user wants the default name, return their RGSC username
            // This is a double edged sword, because it may crash on pirate copies
            if (ScriptHUD.ScriptConfig.GetValue("GGOHud", "CharacterName", "default") == "default")
            {
                return Game.Player.Name;
            }
            // If not, return the custom name specified on the configuration
            else
            {
                return ScriptHUD.ScriptConfig.GetValue("GGOHud", "CharacterName");
            }
        }

        /// <summary>
        /// Get the name of a Ped based on our list of characters.
        /// </summary>
        /// <param name="ThePed">The Ped that we need.</param>
        /// <returns>The Player Name, Character Name or Model Has. The first one that finds.</returns>
        public static string Ped(Ped ThePed)
        {
            // If the Ped is the player, return the player name
            if (ThePed.IsPlayer)
            {
                return Player();
            }
            // If not, return the name from our table
            else if (Data.Names.ContainsKey(ThePed.Model.Hash))
            {
                return Data.Names[ThePed.Model.Hash];
            }
            // If there is no name, return the Hash as string
            else
            {
                return ThePed.Model.Hash.ToString();
            }
        }
    }
}
