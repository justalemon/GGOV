using GTA;

namespace GGO.Extensions
{
    public static class PlayerExtensions
    {
        /// <summary>
        /// Gets the player state based on what is doing currently.
        /// </summary>
        /// <returns>A string with the current player state.</returns>
        public static string GetState(this Player GamePlayer)
        {
            // Wanted Higher or Equal to 3: Strage (see Psycho LLENN)
            if (Game.Player.WantedLevel >= 3)
            {
                return "Strage";
            }
            // Lower than 3: Raising (Is being chased by a reason)
            else if (Game.Player.WantedLevel < 3 && Game.Player.WantedLevel != 0)
            {
                return "Raising";
            }
            // On Mission: Concentrated (Most people )
            else if (Game.MissionFlag)
            {
                return "Concentrated";
            }
            // Otherwise: Neutral (Player is not really doing something)
            else
            {
                return "Neutral";
            }
        }
    }
}
