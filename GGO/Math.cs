using System;

namespace GGO
{
    public static class Math
    {
        /// <summary>
        /// Calculates the percentage of a number.
        /// </summary>
        /// <returns>The value that corresponds to that percentage.</returns>
        public static int Percentage(float Percentage, int Of)
        {
            return Convert.ToInt32((Percentage / 100) * Of);
        }
    }
}
