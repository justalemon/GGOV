using GGO.Common;
using GGO.Common.Properties;
using GTA;
using GTA.Native;
using System;
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
            public static Configuration Config = new Configuration("scripts", new Size(UI.WIDTH, UI.HEIGHT)); //Use UI HEIGHT & WIDTH, UI set to static 1280x720 and scaled up to resolution.
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

                // Draw the squad and player info
                UpdateSquadInfo();
                UpdatePlayerInfo();
            }

            private void UpdatePlayerInfo()
            {
                // the incriments at which the elements are moved, to avoid confusion and cut down on operations
                int xPos = Config.PlayerPosition.X + Config.IconBackgroundSize.Width + Config.CommonSpace.Width;
                int yIncriment = Config.IconBackgroundSize.Height + Config.CommonSpace.Height;
                int xIncriment = Config.IconBackgroundSize.Width + Config.CommonSpace.Width;

                // Generate the points for the name and name info, this will be the player icon as well as the name and health bar.
                Point NamePosition = new Point(Config.PlayerPosition.X, Config.PlayerPosition.Y);
                Point NameInfoPosition = new Point(xPos, Config.PlayerPosition.Y);

                // Send the points off to be drawn
                Draw.PedInfo(Config, new Ped(Game.Player.Character.Handle), NamePosition, NameInfoPosition, true);

                // Generate the points for the main hand weapon, this will include the ammo icon, ammo counter, and weapon icon (when applicable).
                Point MainHandPosition = new Point(Config.PlayerPosition.X, (NamePosition.Y + yIncriment));
                Point MainHandAmmoPosition = new Point(xPos, (NameInfoPosition.Y + yIncriment));
                Point MainHandWeaponPosition = new Point(xPos + xIncriment, (NameInfoPosition.Y + yIncriment));

                // Send the points off to be drawn
                Draw.PlayerWeapon(Config, MainHandPosition, MainHandAmmoPosition, MainHandWeaponPosition);

                // Generate the points for the off hand weapon, this will include the ammo icon, ammo counter, and weapon icon (when applicable).
                Point OffHandPosition = new Point(Config.PlayerPosition.X, (MainHandPosition.Y + yIncriment));
                Point OffHandAmmoPosition = new Point(xPos, (MainHandPosition.Y + yIncriment));
                Point OffHandWeaponPosition = new Point(xPos + xIncriment, (MainHandPosition.Y + yIncriment));

                // Send the points off to be drawn
                Draw.PlayerWeapon(Config, OffHandPosition, OffHandAmmoPosition, OffHandWeaponPosition, true);
            }

            private void UpdateSquadInfo()
            {
                // Draw the squad information on the top left
                // First, create a list to start counting
                int Count = 0;

                // Then, Run over the peds and draw them on the screen (up to 6 of them, including the player)
                // NOTE: We order them by ped hash because the players have lower hash codes than the rest of entities
                foreach (Ped Friendly in World.GetNearbyPeds(Game.Player.Character.Position, 50f).OrderBy(P => P.GetHashCode()))
                {
                    // Check that the ped is a mission entity and is friendly
                    if (Friendly.IsMissionEntity() && Friendly.IsFriendly() && Count <= 6)
                    {
                        Draw.PedInfo(Config, Friendly, false, Count);

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
