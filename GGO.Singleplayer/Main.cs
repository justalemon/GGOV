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
            public static Configuration Config = new Configuration("scripts", new Size(UI.WIDTH, UI.HEIGHT));
            /// <summary>
            /// The window with our debug information.
            /// </summary>
            public static Debug DebugWindow = new Debug(Config);
            /// <summary>
            /// Class with our new cleaner functions.
            /// </summary>
            public static Draw DrawFunctions = new Draw(Config);

            public GGO()
            {
                // Add our Tick and Aborted events
                Tick += OnTick;
                Aborted += OnAbort;

                // Show the debug window if the mode is enabled
                if (Config.Debug)
                {
                    DebugWindow.Show();
                }
            }

            private void OnTick(object Sender, EventArgs Args)
            {
                // If the debug mode is used, update the weapon hash
                if (Config.Debug)
                {
                    Weapon PlayerWeapon = Game.Player.Character.Weapons.Current;
                    DebugWindow.WeaponHash.Text = "Weapon Hash: " + PlayerWeapon.Model.Hash + "/" + Weapon.GetDisplayNameFromHash(PlayerWeapon.Hash);
                }

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
                int Count = 0;

                // Then, Run over the peds and draw them on the screen (up to 6 of them, including the player)
                // NOTE: We order them by ped hash because the players have lower hash codes than the rest of entities
                foreach (Ped NearbyPed in World.GetNearbyPeds(Game.Player.Character.Position, 50f).OrderBy(P => P.GetHashCode()))
                {
                    // Check that the ped is a mission entity and is friendly
                    if (Count <= 6 && Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, NearbyPed) &&
                        Checks.IsFriendly(Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, NearbyPed, Game.Player.Character)))
                    {
                        // Get the ped current and max health
                        int CurrentHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, NearbyPed) - 100;
                        int MaxHealth = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, NearbyPed) - 100;

                        // Select the correct image and name for the file
                        string ImageName = NearbyPed.IsAlive ? "SquadAlive" : "SquadDead";
                        Bitmap ImageType = NearbyPed.IsAlive ? Resources.ImageCharacter : Resources.ImageDead;

                        // Draw the icon and the ped info
                        DrawFunctions.Icon(Images.ResourceToPNG(ImageType, ImageName + Count), Calculations.GetSquadPosition(Config, Count));
                        DrawFunctions.PedInfo(NearbyPed.IsPlayer, false, NearbyPed.Model.Hash, CurrentHealth, MaxHealth, Count, Game.Player.Name);

                        // To end this up, increase the count of peds "rendered"
                        Count++;
                    }

                    // Check for on screen dead Peds to display dead markers, limit to 10 to not clutter the screen.
                    if (NearbyPed.IsDead && NearbyPed.IsOnScreen)
                    {
                        // Draw marker
                        OldDraw.DeadMarker(Config, NearbyPed);
                    }
                }

                // Then, start by drawing the player info
                OldDraw.Icon(Config, Images.ResourceToPNG(Resources.ImageCharacter, "IconPlayer"), Config.PlayerIcon);
                OldDraw.PedInfo(Config, Game.Player.Character, true);

                // Get the current weapon style
                Checks.WeaponStyle CurrentStyle = Checks.GetWeaponStyle((uint)Game.Player.Character.Weapons.Current.Group);

                // And draw the required elements
                if (CurrentStyle == Checks.WeaponStyle.Main)
                {
                    DrawFunctions.Icon(Images.ResourceToPNG(Resources.ImageWeapon, "WeaponPrimary"), Config.PrimaryIcon);
                    OldDraw.WeaponInfo(Config, false, Game.Player.Character.Weapons.Current.AmmoInClip, Weapon.GetDisplayNameFromHash(Game.Player.Character.Weapons.Current.Hash));
                }
                else
                {
                    DrawFunctions.Icon(Images.ResourceToPNG(Resources.NoWeapon, "DummyPrimary"), Config.PrimaryIcon);
                    OldDraw.Icon(Config, Images.ResourceToPNG(Resources.NoWeapon, "AmmoPrimary"), Config.PrimaryBackground);
                }

                if (CurrentStyle == Checks.WeaponStyle.Sidearm)
                {
                    DrawFunctions.Icon(Images.ResourceToPNG(Resources.ImageWeapon, "WeaponSecondary"), Config.SecondaryIcon);
                    OldDraw.WeaponInfo(Config, true, Game.Player.Character.Weapons.Current.AmmoInClip, Weapon.GetDisplayNameFromHash(Game.Player.Character.Weapons.Current.Hash));
                }
                else
                {
                    DrawFunctions.Icon(Images.ResourceToPNG(Resources.NoWeapon, "DummySecondary"), Config.SecondaryIcon);
                    OldDraw.Icon(Config, Images.ResourceToPNG(Resources.NoWeapon, "AmmoSecondary"), Config.SecondaryBackground);
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
