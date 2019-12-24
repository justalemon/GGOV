using Citron;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;
using System.IO;

namespace GGO
{
    /// <summary>
    /// Set of tools used to make the development of GGO easier.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// The indexes of the images.
        /// </summary>
        private static int Index = 0;

        /// <summary>
        /// Creates a Point with absolute values based on relative ones.
        /// </summary>
        /// <param name="X">The relative X.</param>
        /// <param name="Y">The relative Y.</param>
        /// <returns>A Point with absolute values.</returns>
        public static Point LiteralPoint(float X, float Y)
        {
            return new Point((int)(UI.WIDTH * X), (int)(UI.HEIGHT * Y));
        }

        /// <summary>
        /// Creates a Size with absolute values based on relative ones.
        /// </summary>
        /// <param name="Width">The relative X.</param>
        /// <param name="Height">The relative Y.</param>
        /// <returns>A Size with absolute values.</returns>
        public static Size LiteralSize(float Width, float Height)
        {
            return new Size((int)(UI.WIDTH * Width), (int)(UI.HEIGHT * Height));
        }

        /// <summary>
        /// Cleans up the existing index list.
        /// </summary>
        public static void ResetImageIndex()
        {
            Index = 0;
        }

        /// <summary>
        /// Draws an image based on a Bitmap.
        /// </summary>
        /// <param name="FileName">The name of the file to draw.</param>
        /// <param name="Position">Where the image should be drawn.</param>
        /// <param name="Sizes">The size of the image.</param>
        public static void DrawImage(string FileName, Point Position, Size Sizes)
        {
            // Create the path of the image
            string ImagePath = Path.Combine(Paths.GetCallingPath(), "GGO", "Images", $"{FileName}.png");

            // If the image does not exists, notify the user and return
            if (!File.Exists(ImagePath))
            {
                UI.Notify($"Warning: '{ImagePath}' does not exists!");
                return;
            }

            // Finally, if the file exists draw it
            UI.DrawTexture(ImagePath, Index, 0, 150, Position, Sizes);
            Index++;
        }

        public static void SelectOrGive(WeaponHash SelectedHash)
        {
            // If the current weapon equals the desired one
            if (Game.Player.Character.Weapons.Current.Hash == SelectedHash)
            {
                // Hide the weapon
                Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
            }
            // Check if the player does not has the weapon on the inventory
            else if (!Game.Player.Character.Weapons.HasWeapon(SelectedHash))
            {
                // If not, give them the requested weapon with no ammo
                Game.Player.Character.Weapons.Give(SelectedHash, 0, true, false);
            }
            else
            {
                // If the user has it, change the weapon to it
                Game.Player.Character.Weapons.Select(SelectedHash, true);
            }
        }

        public static Point WorldToScreen(Vector3 position)
        {
        	float pointX, pointY;

            unsafe
            {
                if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
                    return Point.Empty;
            }


            return new Point((int)(pointX * UI.WIDTH), (int)(pointY * UI.HEIGHT));
        }
    }
}
