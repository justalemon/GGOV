using GTA;

namespace GGO.Extensions
{
    public static class PedExtensions
    {
        /// <summary>
        /// Checks if the specified relationship ID is for a friendly ped.
        /// </summary>
        /// <param name="Relationship">The relationship ID.</param>
        /// <returns>True if the ped is friendly, False otherwise.</returns>
        public static bool IsFriendly(this Ped GamePed)
        {
            return (int)Game.Player.Character.GetRelationshipWithPed(GamePed) <= 2 && (int)GamePed.GetRelationshipWithPed(Game.Player.Character) <= 2;
        }
    }
}
