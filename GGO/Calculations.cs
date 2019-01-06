using GGO.UserData;
using GTA;
using System;
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
        /// <param name="Config">The current HUD configuration.</param>
        /// <param name="HudPos">Position of the screen calculations.</param>
        /// <param name="Index">Multiplier for the position.</param>
        /// <returns>A Point with the on screen position.</returns>
        public static Point GetSpecificPosition(HudConfig Config, Position HudPos, int Index)
        {
            // Set a dummy position to change later
            Point DefaultPosition = Point.Empty;

            // Get the correct position
            switch (HudPos)
            {
                case Position.SquadIcon:
                    DefaultPosition = LiteralPoint(Config.SquadX, Config.SquadY);
                    break;
                case Position.SquadInfo:
                    DefaultPosition = LiteralPoint(Config.SquadX, Config.SquadY) + LiteralSize(Config.SquareWidth, 0) + LiteralSize(Config.CommonX, 0);
                    break;
                case Position.PlayerIcon:
                    DefaultPosition = LiteralPoint(Config.PlayerX, Config.PlayerY);
                    break;
                case Position.PlayerInfo:
                    DefaultPosition = LiteralPoint(Config.PlayerX, Config.PlayerY) + LiteralSize(Config.SquareWidth, 0) + LiteralSize(Config.CommonX, 0);
                    break;
                default:
                    throw new NotSupportedException("You can't calculate the position for this Screen location.");
            }

            // Finally, return the correct position
            return new Point(DefaultPosition.X, DefaultPosition.Y + (LiteralPoint(Config.CommonX, 0).X + LiteralPoint(Config.SquareWidth, 0).X) * Index);
        }
    }
}
