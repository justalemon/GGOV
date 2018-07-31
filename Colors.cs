using System.Drawing;

namespace GGOHud
{
    class Colors
    {
        // Background of the HUD
        public static readonly Color Background = Color.FromArgb(200, 0, 0, 0);
        // Health: Over 50%
        public static readonly Color Healthy = Color.FromArgb(255, 230, 230, 230);
        // Health: Under 50% and Over 20%
        public static readonly Color Damaged = Color.FromArgb(255, 247, 227, 18);
        // Health: Under 20%
        public static readonly Color NearDeath = Color.FromArgb(255, 200, 0, 0);

        /// <summary>
        /// Returns a color based of the current player health.
        /// </summary>
        /// <param name="Max">The maximum health that the player can have. This is usually 100.</param>
        /// <param name="Value">The current health of the player.</param>
        /// <returns>A color based on the player health.</returns>
        public static Color FromHealth(int Max, int Value)
        {
            int Percentage = (Max / 100) * Value;

            if (Value <= 25)
                return NearDeath;
            else if (Value <= 50 && Value >= 25)
                return Damaged;
            else
                return Healthy;
        }
    }
}
