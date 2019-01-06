using GTA;
using GTA.Native;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace GGO
{
    public class GGO : Script
    {
        /// <summary>
        /// Our configuration parameters.
        /// </summary>
        public static Configuration Config = new Configuration("scripts", new Size(UI.WIDTH, UI.HEIGHT));

        public GGO()
        {
            // Notify that we are starting the script
            Logging.Info("===== GGOV for SHVDN is booting up... =====");

            // Create the inventory positions
            Inventory.StorePositions();

            // Add our Tick and Aborted events
            Tick += OnTick;
            Tick += Inventory.Tick;
            Aborted += OnAbort;

            // If the debug mode is enabled
            if (Config.Debug)
            {
                // Set the logging level to debug
                Logging.CurrentLevel = Logging.Level.Debug;
                // And print the configuration values
                Logging.Debug("Configuration Values:");

                foreach (PropertyDescriptor Descriptor in TypeDescriptor.GetProperties(Config))
                {
                    Logging.Debug(Descriptor.Name + ": " + Descriptor.GetValue(Config).ToString());
                }
            }

            // Notify that the script has started
            Logging.Info("GGOV for SHVDN is up and running");
        }

        Ped[] NearbyPeds = new Ped[0], FriendlyPeds = new Ped[0];
        int nextGetPeds = 0;

        private void OnTick(object Sender, EventArgs Args)
        {
            // Don't draw the UI if the game is loading, paused, player is dead or it cannot be controlled
            if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive || !Game.Player.CanControlCharacter)
            {
                return;
            }

            // Reset the index of the images
            Toolkit.ResetIndex();

            // Disable the colliding HUD elements by default
            if (Config.DisableHud)
            {
                UI.HideHudComponentThisFrame(HudComponent.WeaponIcon);
                UI.HideHudComponentThisFrame(HudComponent.AreaName);
                UI.HideHudComponentThisFrame(HudComponent.StreetName);
                UI.HideHudComponentThisFrame(HudComponent.VehicleName);
                UI.HideHudComponentThisFrame(HudComponent.HelpText);
            }

            // If the user wants to disable the Radar and is not hidden, do it now
            if (Config.DisableRadar && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
            {
                Function.Call(Hash.DISPLAY_RADAR, false);
            }

            // Do stuff on nearby peds
            // Only do logic if a relevant configuration is enabled
            if (Config.SquadMembers || Config.DeadMarkers)
            {
                // Get all of the peds and store them, if it has been a second a more since we last did
                if (Game.GameTime >= nextGetPeds)
                {
                    NearbyPeds = World.GetAllPeds().OrderBy(P => P.GetHashCode()).ToArray();
                    FriendlyPeds = NearbyPeds.Where(P => P.IsFriendly() && P.IsMissionEntity()).ToArray();

                    nextGetPeds = Game.GameTime + 1000;
                }

                if (Config.SquadMembers)
                {
                    foreach (Ped ped in FriendlyPeds)
                    {
                        // Skip non-existant peds
                        if (ped == null || !ped.Exists()) continue;

                        // Get the number of the ped
                        int Number = Array.IndexOf(FriendlyPeds, ped);

                        // Select the correct image
                        string ImagePath = ped.IsAlive ? "IconAlive.png" : "IconDead.png";

                        // Draw the icon and the ped info
                        Toolkit.Icon(ImagePath, Calculations.GetSquadPosition(Config, Number));
                        Toolkit.EntityInfo(ped, true, Number);
                    }
                }

                if (Config.DeadMarkers)
                {
                    foreach (Ped ped in NearbyPeds)
                    {
                        // Only existing dead on-screen peds
                        if (ped == null || !ped.Exists() || ped.IsAlive || !ped.IsOnScreen) continue;

                        // And draw the dead marker
                        Toolkit.DeadMarker(ped);
                    }
                }
            }

            // Then, start by drawing the player info
            Toolkit.Icon("IconAlive.png", Config.PlayerPosition);
            Toolkit.EntityInfo(Game.Player.Character);

            // If the player is on a vehicle, also draw that information
            if (Game.Player.Character.CurrentVehicle != null && Config.VehicleInfo)
            {
                Toolkit.Icon("IconVehicle.png", Config.VehicleIcon);
                Toolkit.EntityInfo(Game.Player.Character.CurrentVehicle);
            }

            // Get the current weapon style
            WeaponStyle CurrentStyle = Game.Player.Character.Weapons.GetStyle();

            // And draw the weapon information for both the primary and secondary
            // If they are not available, draw dummies instead
            if (CurrentStyle == WeaponStyle.Main || CurrentStyle == WeaponStyle.Double)
            {
                Toolkit.Icon("IconWeapon.png", Config.PrimaryIcon);
                Toolkit.WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Toolkit.Icon("NoWeapon.png", Config.PrimaryIcon);
                Toolkit.Icon("NoWeapon.png", Config.PrimaryBackground);
            }

            if (CurrentStyle == WeaponStyle.Sidearm || CurrentStyle == WeaponStyle.Double)
            {
                Toolkit.Icon("IconWeapon.png", Config.SecondaryIcon);
                Toolkit.WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Toolkit.Icon("NoWeapon.png", Config.SecondaryIcon);
                Toolkit.Icon("NoWeapon.png", Config.SecondaryBackground);
            }
        }

        public static void OnAbort(object Sender, EventArgs Args)
        {
            // Reset the Radar state to enabled (just if the script is aborted but not started again)
            Function.Call(Hash.DISPLAY_RADAR, true);
        }
    }
}
