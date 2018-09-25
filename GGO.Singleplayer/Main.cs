using GGO.Common;
using GGO.Common.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GGO.Singleplayer
{
    public class Main
    {
        public class GGO : Script
        {
            /// <summary>
            /// Our configuration parameters.
            /// </summary>
            public static Configuration Config = new Configuration("scripts", Game.ScreenResolution);
            /// <summary>
            /// Our list of images.
            /// </summary>
            public static Dictionary<string, string> Images = new Dictionary<string, string>
            {
                { "SquadAlive1", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadAlive2", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadAlive3", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadAlive4", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadAlive5", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadAlive6", Common.Image.ResourceToPNG(Resources.ImageCharacter) },
                { "SquadDead1", Common.Image.ResourceToPNG(Resources.ImageDead) },
                { "SquadDead2", Common.Image.ResourceToPNG(Resources.ImageDead) },
                { "SquadDead3", Common.Image.ResourceToPNG(Resources.ImageDead) },
                { "SquadDead4", Common.Image.ResourceToPNG(Resources.ImageDead) },
                { "SquadDead5", Common.Image.ResourceToPNG(Resources.ImageDead) },
                { "SquadDead6", Common.Image.ResourceToPNG(Resources.ImageDead) }
            };
            public static Debug DebugWindow = new Debug(Config);

            public GGO()
            {
                // Add our OnTick event
                Tick += OnTick;
                Aborted += OnAbort;

                // Show the debug window if the user wants to
                if (Config.Debug)
                {
                    DebugWindow.Show();
                }
            }

            private void OnTick(object Sender, EventArgs Args)
            {
                // Do not draw the UI elements if the game is loading, paused, player is dead or it cannot be controlled
                if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive ||
                    !Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player))
                {
                    return;
                }

                // Disable the original game HUD and radar if is requested
                if (Config.DisableHud)
                {
                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                }

                // Draw the squad information on the top left
                // First, create a list to start counting
                int Count = 1;

                // Then, Run over the peds and draw them on the screen (up to 6 of them, including the player)
                // NOTE: We order them by ped hash because the players have lower hash codes than the rest of entities
                foreach (Ped Friendly in World.GetNearbyPeds(Game.Player.Character.Position, 50f).OrderBy(P => P.GetHashCode()))
                {
                    // Check that the ped is a mission entity and is friendly
                    if (Friendly.IsMissionEntity() && Friendly.IsFriendly() && Count <= 6)
                    {
                        // Select the icon image by checking that the ped is either dead or alive
                        string ImagePath;
                        if (Friendly.IsDead)
                        {
                            ImagePath = Images["SquadDead" + Count.ToString()];
                        }
                        else
                        {
                            ImagePath = Images["SquadAlive" + Count.ToString()];
                        }

                        // Finally, draw the icon
                        Point Position = new Point(Config.SquadPosition.X, (Config.SquadPosition.Y + Config.ElementsRelative.Height) * Count);
                        Draw.Icon(Config, ImagePath, Position);
                        // And the information of it
                        Point InfoPosition = new Point(Config.SquadPosition.X + Config.IconBackgroundSize.Width + Config.ElementsRelative.Width, (Config.SquadPosition.Y + Config.ElementsRelative.Height) * Count);
                        Draw.PedInfo(Config, Friendly, InfoPosition);

                        // To end this up, increase the count of peds "rendered"
                        Count++;
                    }
                }
            }

            public static void OnAbort(object Sender, EventArgs Args)
            {
                // Close the debug window
                DebugWindow.Close();
            }
        }
    }
}
