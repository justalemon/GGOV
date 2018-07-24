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
    }
}
