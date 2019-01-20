using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO.Extensions
{
    public static class PointExtensions
    {
        /// <summary>
        /// Checks if the position is being clicked by the user.
        /// </summary>
        /// <param name="Position">The starting position.</param>
        /// <param name="Area">The size of the area being clicked.</param>
        /// <returns>True if the area is being clicked, false otherwise.</returns>
        public static bool IsClicked(this Point Position, Size Area)
        {
            int MouseX = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX) * UI.WIDTH);
            int MouseY = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY) * UI.HEIGHT);

            return (MouseX >= Position.X && MouseX <= Position.X + Area.Width) &&
                      (MouseY > Position.Y && MouseY < Position.Y + Area.Height);
        }
    }
}
