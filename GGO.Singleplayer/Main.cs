using GGO.Shared;
using GGO.Shared.Properties;
using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace GGO.Singleplayer
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

            // Add our Tick and Aborted events
            Tick += OnTick;
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

        private void OnTick(object Sender, EventArgs Args)
        {
            // Don't draw the UI if the game is loading, paused, player is dead or it cannot be controlled
            if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive || !Game.Player.CanControlCharacter)
            {
                return;
            }

            // Disable the original game HUD and radar if is requested
            if (Config.DisableHud)
            {
                UI.HideHudComponentThisFrame(HudComponent.WeaponIcon);
                UI.HideHudComponentThisFrame(HudComponent.AreaName);
                UI.HideHudComponentThisFrame(HudComponent.StreetName);
                UI.HideHudComponentThisFrame(HudComponent.VehicleName);
                UI.HideHudComponentThisFrame(HudComponent.HelpText);
            }

            // Get all of the peds and store separate the squad members from the dead ones
            Ped[] NearbyPeds = World.GetAllPeds().OrderBy(P => P.GetHashCode()).ToArray();
            Ped[] FriendlyPeds = NearbyPeds.Where(P => Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Game.Player.Character, P) <= 2 && Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, P)).ToArray();
            Ped[] DeadPeds = NearbyPeds.Where(P => P.IsDead && P.IsOnScreen).ToArray();

            // Draw the squad information on the top left
            foreach (Ped SquadMember in FriendlyPeds)
            {
                // Get the number of the ped
                int Number = Array.IndexOf(FriendlyPeds, SquadMember);

                // Select the correct image and name for the file
                string ImageName = SquadMember.IsAlive ? "SquadAlive" : "SquadDead";
                Bitmap ImageType = SquadMember.IsAlive ? Resources.ImageCharacter : Resources.ImageDead;

                // Draw the icon and the ped info
                Toolkit.Icon(Images.ResourceToPNG(ImageType, ImageName + Number), Calculations.GetSquadPosition(Config, Number));
                Toolkit.EntityInfo(SquadMember, true, Number);
            }

            // Draw the dead ped markers over their heads
            foreach (Ped DeadPed in DeadPeds)
            {
                // Get the coordinates for the head
                Vector3 HeadCoord = DeadPed.GetBoneCoord(Bone.SKEL_Head);
                // And draw the dead marker
                Toolkit.DeadMarker(UI.WorldToScreen(HeadCoord), Vector3.Distance(Game.Player.Character.Position, HeadCoord), DeadPed.GetHashCode());
            }

            // Then, start by drawing the player info
            Toolkit.Icon(Images.ResourceToPNG(Resources.ImageCharacter, "IconPlayer"), Config.PlayerPosition);
            Toolkit.EntityInfo(Game.Player.Character);

            // If the player is on a vehicle, also draw that information
            if (Game.Player.Character.CurrentVehicle != null)
            {
                Toolkit.Icon(Images.ResourceToPNG(Resources.ImageCharacter, "IconVehicle"), Config.VehicleIcon);
                Toolkit.EntityInfo(Game.Player.Character.CurrentVehicle);
            }

            // Get the current weapon style
            Checks.WeaponStyle CurrentStyle = Checks.GetWeaponStyle((uint)Game.Player.Character.Weapons.Current.Group);

            // And draw the weapon information for both the primary and secondary
            // If they are not available, draw dummies instead
            if (CurrentStyle == Checks.WeaponStyle.Main || CurrentStyle == Checks.WeaponStyle.Double)
            {
                Toolkit.Icon(Images.ResourceToPNG(Resources.ImageWeapon, "WeaponPrimary"), Config.PrimaryIcon);
                Toolkit.WeaponInfo(CurrentStyle, Game.Player.Character.Weapons.Current.AmmoInClip, Weapon.GetDisplayNameFromHash(Game.Player.Character.Weapons.Current.Hash));
            }
            else
            {
                Toolkit.Icon(Images.ResourceToPNG(Resources.NoWeapon, "DummyPrimary"), Config.PrimaryIcon);
                Toolkit.Icon(Images.ResourceToPNG(Resources.NoWeapon, "AmmoPrimary"), Config.PrimaryBackground);
            }
            if (CurrentStyle == Checks.WeaponStyle.Sidearm || CurrentStyle == Checks.WeaponStyle.Double)
            {
                Toolkit.Icon(Images.ResourceToPNG(Resources.ImageWeapon, "WeaponSecondary"), Config.SecondaryIcon);
                Toolkit.WeaponInfo(CurrentStyle, Game.Player.Character.Weapons.Current.AmmoInClip, Weapon.GetDisplayNameFromHash(Game.Player.Character.Weapons.Current.Hash));
            }
            else
            {
                Toolkit.Icon(Images.ResourceToPNG(Resources.NoWeapon, "DummySecondary"), Config.SecondaryIcon);
                Toolkit.Icon(Images.ResourceToPNG(Resources.NoWeapon, "AmmoSecondary"), Config.SecondaryBackground);
            }
        }

        public static void OnAbort(object Sender, EventArgs Args)
        {

        }
    }
}
