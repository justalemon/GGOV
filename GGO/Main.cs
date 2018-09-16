using GGO.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace GGO
{
    public class GGO : Script
    {
        /// <summary>
        /// Class to get our configuration values.
        /// </summary>
        public static Configuration Config = new Configuration("scripts\\GGO.ini", "GGO");
        /// <summary>
        /// Our list of images.
        /// </summary>
        public static Dictionary<string, string> Images = new Dictionary<string, string>
        {
            { "Squad1", Tools.ResourceToPNG(Resources.ImageCharacter) },
            { "Squad2", Tools.ResourceToPNG(Resources.ImageCharacter) },
            { "Squad3", Tools.ResourceToPNG(Resources.ImageCharacter) },
            { "Squad4", Tools.ResourceToPNG(Resources.ImageCharacter) },
            { "Squad5", Tools.ResourceToPNG(Resources.ImageCharacter) },
            { "Squad6", Tools.ResourceToPNG(Resources.ImageCharacter) }
        };

        public GGO()
        {
            // Patch our locale so we don't have the "coma vs dot" problem
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Add our OnTick event
            Tick += OnTick;

            // Show some debug messages if the user wants to
            if (Config.Debug)
            {
                UI.Notify("~p~GGO~s~: Starting the Mod...");

                UI.Notify("~g~GGO~s~: IconImage: " + Config.IconImage.Width.ToString() + "w, " + Config.IconImage.Height.ToString() + "h");
                UI.Notify("~g~GGO~s~: IconBackground: " + Config.IconBackground.Width.ToString() + "w, " + Config.IconBackground.Height.ToString() + "h");
                UI.Notify("~g~GGO~s~: IconRelative: " + Config.IconRelative.Width.ToString() + "w, " + Config.IconRelative.Height.ToString() + "h");
                UI.Notify("~g~GGO~s~: SquadRelative: " + Config.SquadRelative.Width.ToString() + "w, " + Config.SquadRelative.Height.ToString() + "h");
                UI.Notify("~g~GGO~s~: SquadInfoSize: " + Config.SquadInfoSize.Width.ToString() + "w, " + Config.SquadInfoSize.Height.ToString() + "h");
                UI.Notify("~g~GGO~s~: HealthBarSize: " + Config.HealthBarSize.Width.ToString() + "w, " + Config.HealthBarSize.Height.ToString() + "h");

                UI.Notify("~b~GGO~s~: SquadPosition: " + Config.SquadPosition.X.ToString() + "x, " + Config.SquadPosition.Y.ToString() + "y");
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
            if (Config.HudDisabled)
            {
                Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
            }

            // Draw the squad information on the top left
            // First, create a list and a count with the player
            int Count = 1;
            List<Ped> Squad = new List<Ped>
            {
                Game.Player.Character
            };

            // Run over the peds and add them to the list, up to 6 of them including the player
            foreach (Ped NearbyPed in World.GetNearbyPeds(Game.Player.Character.Position, 50f))
            {
                if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, NearbyPed) &&
                    Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, NearbyPed, Game.Player.Character) != 5 &&
                    !Function.Call<bool>(Hash.IS_PED_A_PLAYER, NearbyPed))
                {
                    Squad.Add(NearbyPed);
                }
            }

            // At this point the list is full of peds that are on our squad, so draw them on the HUD
            foreach (Ped Friendly in Squad)
            {
                if (Count > 6)
                {
                    return;
                }

                Point Position = new Point(Config.SquadPosition.X, (Config.SquadPosition.Y + Config.SquadRelative.Height) * Count);
                Draw.Icon(Images["Squad" + Count.ToString()], Position);

                Point InfoPosition = new Point(Config.SquadPosition.X + Config.IconBackground.Width + Config.SquadRelative.Width, (Config.SquadPosition.Y + Config.SquadRelative.Height) * Count);
                Draw.PedInfo(Friendly, InfoPosition);

                Count++;
            }
        }
    }
}
