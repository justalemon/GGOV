using System.Drawing;

namespace GGO
{
    /// <summary>
    /// Colors used on the mod.
    /// </summary>
    public class Colors
    {
        /// <summary>
        /// Color for the backgrounds.
        /// </summary>
        public static Color Backgrounds = Color.FromArgb(175, 0, 0, 0);
        /// <summary>
        /// Color for the inventory background.
        /// </summary>
        public static Color Inventory = Color.FromArgb(100, 0, 0, 0);
        /// <summary>
        /// Color for the inventory name.
        /// </summary>
        public static Color Details = Color.FromArgb(255, 42, 156, 204);
        /// <summary>
        /// Color for the dividers of the health bars.
        /// </summary>
        public static Color Dividers = Color.FromArgb(125, 230, 230, 230);
        /// <summary>
        /// Color for a ped with health over 100% (stupid but posible).
        /// </summary>
        public static Color HealthOverflow = Color.FromArgb(255, 0, 191, 255);
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
    }
}
