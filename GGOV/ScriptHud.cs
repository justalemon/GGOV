using GGO.API;
using GGO.API.Native;
using GGO.Extensions;
using GGO.Properties;
using GGO.UserData;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static GGO.Tools;

namespace GGO
{
    public class Hud : Script
    {
        /// <summary>
        /// Configuration for the HUD elements.
        /// </summary>
        private HudConfig Config = JsonConvert.DeserializeObject<HudConfig>(File.ReadAllText("scripts\\GGO\\Hud.json"));
        /// <summary>
        /// Names for the peds on the squad section.
        /// </summary>
        private Dictionary<string, string> Names = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("scripts\\GGO\\Names.json"));
        /// <summary>
        /// List of peds that are near the player.
        /// </summary>
        private Ped[] NearbyPeds = new Ped[0];
        /// <summary>
        /// List of peds that are friendly and part of the squad.
        /// </summary>
        private Ped[] FriendlyPeds = new Ped[0];
        /// <summary>
        /// Next game time that we should update the lists of peds.
        /// </summary>
        private int NextFetch = 0;
        /// <summary>
        /// The list of player fields to be drawn while the game is running.
        /// </summary>
        private List<PlayerField> Fields = new List<PlayerField>();

        public Hud()
        {
            // Don't do nothing if the user requested the menu to be disabled
            if (!Config.Enabled)
            {
                return;
            }

            // Add the player fields
            Fields.Add(new PlayerSidearm());
            Fields.Add(new PlayerMain());
            Fields.Add(new PlayerHealth());
            if (Config.VehicleInfo) { Fields.Add(new PlayerVehicle()); }

            // Add our Tick and Aborted events
            Tick += OnTick;
            Aborted += OnAbort;
        }

        private void OnTick(object Sender, EventArgs Args)
        {
            // Don't draw the UI if the game is loading, paused, player is dead or it cannot be controlled
            if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive || !Game.Player.CanControlCharacter)
            {
                return;
            }

            // Reset the index of the images
            ResetImageIndex();

            // Disable the colliding HUD elements by default
            if (!Config.Collisions)
            {
                UI.HideHudComponentThisFrame(HudComponent.WeaponIcon);
                UI.HideHudComponentThisFrame(HudComponent.AreaName);
                UI.HideHudComponentThisFrame(HudComponent.StreetName);
                UI.HideHudComponentThisFrame(HudComponent.VehicleName);
                UI.HideHudComponentThisFrame(HudComponent.HelpText);
            }

            // If the user wants to disable the Radar and is not hidden, do it now
            if (!Config.Radar && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
            {
                Function.Call(Hash.DISPLAY_RADAR, false);
            }

            // Get the interior where the player is
            int Interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character);

            // Draw get the ped information only if the squad members or dead markers have been enabled
            if (Config.Squad || Config.DeadMarkers)
            {
                // If the current time is higher or equal than the next fetch time
                if (Game.GameTime >= NextFetch)
                {
                    // Get all of the peds and order them by hash
                    NearbyPeds = World.GetAllPeds().OrderBy(P => P.GetHashCode()).ToArray();
                    // THen, filter the friendly squad peds
                    FriendlyPeds = NearbyPeds.Where(P => P.IsFriendly() && P.IsMissionEntity()).ToArray();

                    // Finally, set the next fetch time to one second in the future
                    NextFetch = Game.GameTime + 1000;
                }

                // If the squad members are enabled
                if (Config.Squad)
                {
                    // Store a number of the invalid peds
                    int InvalidPeds = 0;

                    // Iterate over the squad members
                    foreach (Ped SquadMember in FriendlyPeds)
                    {                        
                        // Skip non-existant peds and those inside of the night club (if enabled) and increase the count of invalid
                        if (SquadMember == null || !SquadMember.Exists() || (Config.ClubFix && Interior == 271617 && !SquadMember.IsPlayer))
                        {
                            InvalidPeds += 1;
                            continue;
                        }

                        // Get the number of the ped minus the invalid ones
                        int Number = Array.IndexOf(FriendlyPeds, SquadMember) - InvalidPeds;

                        // Draw the icon and the ped info for it
                        Icon(SquadMember.IsAlive ? "IconAlive" : "IconDead", Config.GetSpecificPosition(Position.SquadIcon, Number, false));
                        // EntityInfo(SquadMember, InfoSize.Small, Number);
                    }
                }


                // If the user wants to, draw the dead markers
                if (Config.DeadMarkers)
                {
                    // Iterate over all the peds
                    foreach (Ped DeadPed in NearbyPeds)
                    {
                        // If the ped is null, does not exists, is alive or is not on screen, skip it
                        if (DeadPed == null || !DeadPed.Exists() || DeadPed.IsAlive || !DeadPed.IsOnScreen)
                        {
                            continue;
                        }

                        // And draw the dead marker
                        DeadMarker(DeadPed);
                    }
                }
            }

            // Count the items that should not be drawn
            int Skipped = 0;
            // Iterate over the count of fields
            for (int i = 0; i < Fields.Count; i++)
            {
                // If the field should not be shown
                if (!Fields[i].ShouldBeShown())
                {
                    // Add one more and skip the iteration
                    Skipped += 1;
                    continue;
                }
                // Then, draw the specified field
                PlayerField(Fields[i], i - Skipped);
            }
        }

        public void OnAbort(object Sender, EventArgs Args)
        {
            // Reset the Radar state to enabled (just if the script is aborted but not started again)
            Function.Call(Hash.DISPLAY_RADAR, true);
        }

        public void PlayerField(PlayerField Field, int Index)
        {
            // We are always going to need an icon
            Icon("Icon" + Field.GetIconName(), Config.GetSpecificPosition(Position.PlayerIcon, Index, true));

            // Store the base position
            Point BasePosition = Config.GetSpecificPosition(Position.PlayerInfo, Index, true);

            // If the field type is health
            if (Field.GetFieldType() == FieldType.Health)
            {
                // Draw the background for the health information
                new UIRectangle(BasePosition, LiteralSize(Config.PlayerWidth, Config.PlayerHeight), Colors.Backgrounds).Draw();

                // Calculate the percentage of health bar
                float Percentage = (Field.GetCurrentValue() / Field.GetMaxValue()) * 100;
                float Width = (Percentage / 100) * LiteralSize(Config.PlayerHealthWidth, 0).Width;
                // And create the size with the real health size
                Size HealthSize = new Size((int)Width, LiteralSize(0, Config.PlayerHealthHeight).Height);

                // Draw the entity health
                new UIRectangle(BasePosition + LiteralSize(Config.PlayerHealthX, Config.PlayerHealthY), HealthSize, Colors.GetHealthColor(Field.GetCurrentValue(), Field.GetMaxValue())).Draw();

                // Draw the health dividers
                foreach (Point Position in Config.GetDividerPositions(BasePosition, true))
                {
                    new UIRectangle(Position, LiteralSize(Config.DividerWidth, Config.DividerHeight), Colors.Dividers).Draw();
                }

                // Draw the field name
                new UIText(Field.GetFirstText(), BasePosition + LiteralSize(Config.SquadNameX, Config.SquadNameY), .325f).Draw();
            }
            // Else if the field type is weapon
            else if (Field.GetFieldType() == FieldType.Weapon)
            {
                // If we should draw the ammo count and weapon image
                if (Field.DataShouldBeShown())
                {
                    // Store the position of the weapon space
                    Point WeaponLocation = Config.GetSpecificPosition(Position.PlayerWeapon, Index, true);

                    // Draw the ammo quantity
                    new UIRectangle(BasePosition, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds).Draw();
                    new UIText(Field.GetCurrentValue().ToString("0"), BasePosition + LiteralSize(Config.AmmoX, Config.AmmoY), .6f, Color.White, GTA.Font.Monospace, true).Draw();

                    // And weapon image
                    new UIRectangle(WeaponLocation, LiteralSize(Config.PlayerWidth, Config.PlayerHeight) - LiteralSize(Config.SquareWidth, 0) - LiteralSize(Config.CommonX, 0), Colors.Backgrounds).Draw();
                    DrawImage("Weapon" + Field.GetWeaponImage(), WeaponLocation + LiteralSize(Config.WeaponX, Config.WeaponY), LiteralSize(Config.WeaponWidth, Config.WeaponHeight));
                }
                // Otherwise, draw a simple placeholder
                else
                {
                    Icon("Placeholder", BasePosition);
                }
            }
            // Otherwise
            else
            {
                // Throw an exception because fuck it
                throw new InvalidOperationException("That Field type is not supported or valid.");
            }
        }

        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="Filename">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        public void Icon(string Filename, Point Position)
        {
            // Draw the background
            new UIRectangle(Position, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds).Draw();
            // And the image over it
            DrawImage(Filename, Position + LiteralSize(Config.IconX, Config.IconY), LiteralSize(Config.IconWidth, Config.IconHeight));
        }

        /// <summary>
        /// Draws a dead marker over the ped head.
        /// </summary>
        /// <param name="GamePed">The ped where the dead marker should be drawn.</param>
        public void DeadMarker(Ped GamePed)
        {
            // Get the coordinates for the head
            Vector3 HeadCoord = GamePed.GetBoneCoord(Bone.SKEL_Head);
            // Get the On Screen position for the head
            Point ScreenPos = UI.WorldToScreen(HeadCoord);

            // Get distance ratio by Ln(Distance + Sqrt(e)), then calculate size of marker using intercept thereom.
            double Ratio = Math.Log(Vector3.Distance(Game.Player.Character.Position, HeadCoord) + 1.65);
            // Calculate the marker size based on the distance between player and dead ped
            Size LiteralMarker = LiteralSize(Config.DeadMarkerWidth, Config.DeadMarkerHeight);
            Size MarkerSize = new Size((int)(LiteralMarker.Width / Ratio), (int)(LiteralMarker.Height / Ratio));
            // Offset the marker by half width to center, and full height to put on top.
            ScreenPos.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            DrawImage("DeadMarker", ScreenPos, MarkerSize);
        }
    }
}
