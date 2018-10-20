using System.Linq;

namespace GGO.Common
{
    public class Checks
    {
        /// <summary>
        /// Relationships that are considered friendly.
        /// </summary>
        public static int[] Relationships = new int[] { 0, 1, 2 };

        /// <summary>
        /// Checks if the specified relationship ID is for a friendly ped.
        /// </summary>
        /// <param name="Relationship">The relationship ID.</param>
        /// <returns>True if the ped is friendly, False otherwise.</returns>
        public static bool IsFriendly(int Relationship)
        {
            return Relationships.Contains(Relationship);
        }
    }
}
