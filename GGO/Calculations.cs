using GGO.UserData;
using GTA;
using System.Drawing;
using static GGO.Tools;

namespace GGO
{
    public class Calculations
    {
        /// <summary>
        /// Gets the position of the dividers either for the player or the squad members.
        /// </summary>
        /// <param name="Player">If the divider positions are for the player.</param>
        /// <param name="Count">The number for the squad member.</param>
        /// <returns>An array with the 5 positions.</returns>
        public static Point[] GetDividerPositions(HudConfig Config, Point Position, bool Player, int Count = 0)
        {
            // Create a list of dividers
            Point[] Positions = new Point[5];

            // Store our positions for the player or squad members
            Size HealthSize = Player ? LiteralSize(Config.PlayerHealthWidth, Config.PlayerHealthHeight) : LiteralSize(Config.SquadHealthWidth, Config.SquadHealthHeight);
            Size HealthPosition = Player ? LiteralSize(Config.PlayerHealthX, Config.PlayerHealthY) : LiteralSize(Config.SquadHealthX, Config.SquadHealthY);

            // For the dividers, get the distance between each one of them
            int HealthSep = HealthSize.Width / 4;

            // Itterate from 0 to 4 to create our separators
            for (int Separator = 0; Separator < 5; Separator++)
            {
                // Calculate the position of the separator and add it in the array
                Positions[Separator] = (Position + HealthPosition) + new Size(HealthSep * Separator, 0) + LiteralSize(Config.DividerX, Config.DividerY);
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
        public static Point GetSquadPosition(HudConfig Config, int Count, bool Info = false)
        {
            // Increase the count by one
            Count++;

            // Return the correct position for the info or icon
            if (Info)
            {
                return new Point((int)(UI.WIDTH * Config.SquadX) + (int)(UI.WIDTH * Config.SquareWidth) + (int)(UI.WIDTH * Config.CommonX),
                                ((int)(UI.HEIGHT * Config.SquadY) + (int)(UI.HEIGHT * Config.CommonY)) * Count);
            }
            else
            {
                return new Point((int)(UI.WIDTH * Config.SquadX),
                                ((int)(UI.HEIGHT * Config.SquadY) + (int)(UI.HEIGHT * Config.CommonY)) * Count);
            }
        }
    }
}
