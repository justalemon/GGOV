using GGO.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace GGO.Singleplayer
{
    public class Main
    {
        public class GGO : Script
        {
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
                if (Configuration.Debug)
                {
                    UI.Notify("~p~GGO~s~: Starting the Mod...");

                    UI.Notify("~g~GGO~s~: IconImage: " + Configuration.IconImage.Width.ToString() + "w, " + Configuration.IconImage.Height.ToString() + "h");
                    UI.Notify("~g~GGO~s~: IconBackground: " + Configuration.IconBackground.Width.ToString() + "w, " + Configuration.IconBackground.Height.ToString() + "h");
                    UI.Notify("~g~GGO~s~: IconRelative: " + Configuration.IconRelative.Width.ToString() + "w, " + Configuration.IconRelative.Height.ToString() + "h");
                    UI.Notify("~g~GGO~s~: SquadRelative: " + Configuration.SquadRelative.Width.ToString() + "w, " + Configuration.SquadRelative.Height.ToString() + "h");
                    UI.Notify("~g~GGO~s~: SquadInfoSize: " + Configuration.SquadInfoSize.Width.ToString() + "w, " + Configuration.SquadInfoSize.Height.ToString() + "h");
                    UI.Notify("~g~GGO~s~: HealthBarSize: " + Configuration.HealthBarSize.Width.ToString() + "w, " + Configuration.HealthBarSize.Height.ToString() + "h");

                    UI.Notify("~b~GGO~s~: SquadPosition: " + Configuration.SquadPosition.X.ToString() + "x, " + Configuration.SquadPosition.Y.ToString() + "y");
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
                if (Configuration.HudDisabled)
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

                    Point Position = new Point(Configuration.SquadPosition.X, (Configuration.SquadPosition.Y + Configuration.SquadRelative.Height) * Count);
                    Draw.Icon(Images["Squad" + Count.ToString()], Position, Configuration.IconBackground, Configuration.IconRelative, Configuration.IconImage);

                    Point InfoPosition = new Point(Configuration.SquadPosition.X + Configuration.IconBackground.Width + Configuration.SquadRelative.Width, (Configuration.SquadPosition.Y + Configuration.SquadRelative.Height) * Count);
                    Draw.PedInfo(Friendly, InfoPosition, Configuration.SquadInfoSize, Configuration.HealthBarSize, Configuration.HealthBarOffset, Configuration.HealthDividerOffset, Configuration.HealthDividerSize, Configuration.PlayerNameOffset);

                    Count++;
                }
            }
        }
    }
}
