using GTA;
using GTA.Math;
using GTA.Native;
using LemonUI.Elements;
using LemonUI.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GGOV
{
    /// <summary>
    /// Script that draws the ped markers.
    /// </summary>
    public class PedMarkers : Script
    {
        #region Fields

        private readonly Dictionary<Ped, ScaledTexture> markers = new Dictionary<Ped, ScaledTexture>();
        private int nextUpdate = 0;

        #endregion

        #region Constructor

        public PedMarkers()
        {
            Tick += PedMarkers_Tick;
        }

        #endregion

        #region Event

        private void PedMarkers_Tick(object sender, EventArgs e)
        {
            // If is time for the next update or is the first update
            if (nextUpdate <= Game.GameTime || nextUpdate == 0)
            {
                // Iterate the the peds on the game world
                foreach (Ped ped in World.GetAllPeds())
                {
                    // If the ped is dead and is not part of the markers, add it
                    if (ped.IsDead && !markers.ContainsKey(ped))
                    {
                        markers.Add(ped, new ScaledTexture(PointF.Empty, new SizeF(220 * 0.75f, 124 * 0.75f), "ggo", "marker_dead"));
                    }
                }

                // Finally, set the new update time
                nextUpdate = Game.GameTime + 500;
            }

            // Iterate over the existing items
            // (creating the new dictionary is required to prevent the "collection was edited" exception)
            foreach (KeyValuePair<Ped, ScaledTexture> marker in new Dictionary<Ped, ScaledTexture>(markers))
            {
                Ped ped = marker.Key;

                // If the ped is no longer present in the game world, remove it and continue
                if (!ped.Exists())
                {
                    markers.Remove(ped);
                    continue;
                }

                // If the ped is not on the screen, skip it
                if (!ped.IsOnScreen)
                {
                    continue;
                }

                // Get the position of the ped head
                Vector3 headPos = ped.Bones[Bone.SkelHead].Position;

                // And then conver it to screen coordinates
                OutputArgument originalX = new OutputArgument();
                OutputArgument originalY = new OutputArgument();
                bool ok = Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, headPos.X, headPos.Y, headPos.Z, originalX, originalY);

                // If it was unable to get the position, continue
                if (!ok)
                {
                    continue;
                }

                // Otherwise, convert the position from relative to absolute
                PointF screenPos = new PointF(originalX.GetResult<float>(), originalY.GetResult<float>()).ToAbsolute();
                // And set it for the correct
                marker.Value.Position = new PointF(screenPos.X - (marker.Value.Size.Width * 0.5f), screenPos.Y - marker.Value.Size.Height);

                // Finally, draw it
                marker.Value.Draw();
            }
        }

        #endregion
    }
}
