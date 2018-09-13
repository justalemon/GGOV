using GTA;
using GTA.Native;
using System.Drawing;

namespace GGOHud
{
    class Colors
    {
        /// <summary>
        /// Color for the backgrounds of the items.
        /// </summary>
        public static Color Background = Color.FromArgb(175, 0, 0, 0);
        /// <summary>
        /// Color for the dividers of the health bar.
        /// </summary>
        public static Color Divider = Color.FromArgb(125, 230, 230, 230);
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
        /// <param name="ThePed"></param>
        /// <returns></returns>
        public static Color GetPedHealthColor(Ped ThePed)
        {
            float MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, ThePed);
            float CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, ThePed);

            float Percentage = (CurrentHealth / MaxHealth) * 100;

            if (Percentage >= 50 && Percentage <= 100)
            {
                return HealthNormal;
            }
            else if (Percentage <= 50 && Percentage >= 25)
            {
                return HealthDanger;
            }
            else if (Percentage <= 25)
            {
                return HealthDying;
            }
            else
            {
                return HealthStupid;
            }
        }
    }
}
