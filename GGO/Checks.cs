using System.Linq;

namespace GGO
{
    public class Checks
    {
        /// <summary>
        /// Relationships that are considered friendly.
        /// </summary>
        public static int[] Relationships = new int[] { 0, 1, 2 };
        /// <summary>
        /// The style of the weapon that the player currently has.
        /// </summary>
        public enum WeaponStyle
        {
            Banned = -1,
            Main = 0,
            Sidearm = 1,
            Melee = 2,
            Double = 3
        }
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static uint[] BannedWeapons = new uint[] { 1595662460u, 1548507267u, 2685387236u, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), SMG.
        /// </summary>
        public static uint[] SecondaryWeapons = new uint[] { 416676503u, 3337201093u };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type).
        /// </summary>
        public static uint[] MeleeWeapons = new uint[] { 3566412244u };

        /// <summary>
        /// Checks if the specified relationship ID is for a friendly ped.
        /// </summary>
        /// <param name="Relationship">The relationship ID.</param>
        /// <returns>True if the ped is friendly, False otherwise.</returns>
        public static bool IsFriendly(int Relationship)
        {
            return Relationships.Contains(Relationship);
        }

        /// <summary>
        /// Checks for the weapon type.
        /// </summary>
        /// <param name="ID">The Weapon Type ID.</param>
        /// <returns>The style of weapon.</returns>
        public static WeaponStyle GetWeaponStyle(uint ID)
        {
            // Return the first match, in order
            // From dangerous to normal
            if (BannedWeapons.Contains(ID))
                return WeaponStyle.Banned;
            else if (MeleeWeapons.Contains(ID))
                return WeaponStyle.Melee;
            else if (SecondaryWeapons.Contains(ID))
                return WeaponStyle.Sidearm;
            else
                return WeaponStyle.Main;
        }
    }
}
