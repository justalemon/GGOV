using System;
using System.Drawing;

namespace GGO.Shared
{
    public class Calculations
    {
        /// <summary>
        /// Gets the position of the dividers either for the player or the squad members.
        /// </summary>
        /// <param name="Player">If the divider positions are for the player.</param>
        /// <param name="Count">The number for the squad member.</param>
        /// <returns>An array with the 5 positions.</returns>
        public static PointF[] GetDividerPositions(Configuration Config, bool Player, int Count = 0)
        {
            // Create a list of dividers
            PointF[] Positions = new PointF[5];

            // Store our positions for the player or squad members
            PointF InfoPosition = Player ? Config.PlayerInformation : GetSquadPosition(Config, Count, true);
            SizeF HealthSize = Player ? Config.PlayerHealthSize : Config.SquadHealthSize;
            SizeF HealthPosition = Player ? Config.PlayerHealthPos : Config.SquadHealthPos;

            // For the dividers, get the distance between each one of them
            float HealthSep = HealthSize.Width / 4;

            // Itterate from 0 to 4 to create our separators
            for (int Separator = 0; Separator < 5; Separator++)
            {
                // Calculate the position of the separator and add it in the array
                Positions[Separator] = (InfoPosition + HealthPosition) + new SizeF(HealthSep * Separator, 0) + Config.DividerPosition;
            }

            // Finally, return the divider positions
            return Positions;
        }

        /// <summary>
        /// Gets the specific position for the squad member.
        /// </summary>
        /// <param name="Count">The index of the squad member (zero based).</param>
        /// <param name="Info">If the location of the info should be returned.</param>
        /// <returns>A Point with the on screen position.</returns>
        public static PointF GetSquadPosition(Configuration Config, int Count, bool Info = false)
        {
            // Increase the count by one
            Count++;

            // Return the correct position for the info or icon
            if (Info)
            {
                return new PointF(Config.SquadPosition.X + Config.SquaredBackground.Width + Config.CommonSpacing.Width, (Config.SquadPosition.Y + Config.CommonSpacing.Height) * Count);
            }
            else
            {
                return new PointF(Config.SquadPosition.X, (Config.SquadPosition.Y + Config.CommonSpacing.Height) * Count);
            }
        }

        /// <summary>
        /// Calculates the size of the dead marked based on the player-to-ped distance.
        /// </summary>
        /// <param name="Config">The mod configuration.</param>
        /// <param name="Distance">The distance between the player and the ped.</param>
        /// <returns>The relative position.</returns>
        public static SizeF GetMarkerSize(Configuration Config, float Distance)
        {
            // Get distance ratio by Ln(Distance + Sqrt(e)), then calculate size of marker using intercept thereom.
            double Ratio = Math.Log(Distance + 1.65);
            SizeF MarkerSize = new SizeF((float)(Config.DeadMarker.Width / Ratio), (float)(Config.DeadMarker.Height / Ratio));

            // And finish by returning the new size
            return MarkerSize;
        }

        public static SizeF GetHealthSize(Configuration Config, bool Player, float Max, float Current)
        {
            // Store the original size for the health bar
            SizeF OriginalSize = Player ? Config.PlayerHealthSize : Config.SquadHealthSize;

            // Calculate the percentage of health and width
            float HealthPercentage = (Current / Max) * 100f;
            float Width = (HealthPercentage / 100f) * OriginalSize.Width;

            // Finally, return the new size
            return new SizeF(Width, OriginalSize.Height);
        }
    }
}
