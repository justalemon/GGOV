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

        public Hud()
        {
            // Don't do nothing if the user requested the menu to be disabled
            if (!Config.Enabled)
            {
                return;
            }

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
            if (Config.Radar && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
            {
                Function.Call(Hash.DISPLAY_RADAR, false);
            }

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
                        // Skip non-existant peds and increase the count of invalid
                        if (SquadMember == null || !SquadMember.Exists())
                        {
                            InvalidPeds += 1;
                            continue;
                        }

                        // Get the number of the ped minus the invalid ones
                        int Number = Array.IndexOf(FriendlyPeds, SquadMember) - InvalidPeds;

                        // Draw the icon and the ped info for it
                        Icon(SquadMember.IsAlive ? "IconAlive" : "IconDead", Config.GetSpecificPosition(Position.SquadIcon, Number));
                        EntityInfo(SquadMember, InfoSize.Small, Number);
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

            // Then, start by drawing the player info
            Icon("IconAlive", Config.GetSpecificPosition(Position.PlayerIcon, 1));
            EntityInfo(Game.Player.Character, InfoSize.Normal);

            // If the player is on a vehicle, also draw that information
            if (Game.Player.Character.CurrentVehicle != null && Config.VehicleInfo)
            {
                Icon("IconVehicle", Config.GetSpecificPosition(Position.PlayerIcon, 0));
                EntityInfo(Game.Player.Character.CurrentVehicle, InfoSize.Normal);
            }

            // Get the current weapon style
            WeaponStyle CurrentStyle = Game.Player.Character.Weapons.GetStyle();

            // Calculate and store the position of the primary and secondary icons
            Point PrimaryIcon = Config.GetSpecificPosition(Position.PlayerIcon, 2);
            Point PrimaryBackground = Config.GetSpecificPosition(Position.PlayerAmmo, 2);
            Point SecondaryIcon = Config.GetSpecificPosition(Position.PlayerIcon, 3);
            Point SecondaryBackground = Config.GetSpecificPosition(Position.PlayerAmmo, 3);

            // And draw the weapon information for both the primary and secondary
            // If they are not available, draw dummies instead
            if (CurrentStyle == WeaponStyle.Main || CurrentStyle == WeaponStyle.Double)
            {
                Icon("IconWeapon", PrimaryIcon);
                WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Icon("NoWeapon", PrimaryIcon);
                Icon("NoWeapon", PrimaryBackground);
            }
            if (CurrentStyle == WeaponStyle.Sidearm || CurrentStyle == WeaponStyle.Double)
            {
                Icon("IconWeapon", SecondaryIcon);
                WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Icon("NoWeapon", SecondaryIcon);
                Icon("NoWeapon", SecondaryBackground);
            }
        }

        public void OnAbort(object Sender, EventArgs Args)
        {
            // Reset the Radar state to enabled (just if the script is aborted but not started again)
            Function.Call(Hash.DISPLAY_RADAR, true);
        }

        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="Filename">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        public void Icon(string Filename, Point Position)
        {
            // Draw the background
            UIRectangle Background = new UIRectangle(Position, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds);
            Background.Draw();
            // And the image over it
            DrawImage(Filename, Position + LiteralSize(Config.IconX, Config.IconY), LiteralSize(Config.IconWidth, Config.IconHeight));
        }

        /// <summary>
        /// Draws the information of a Game Entity.
        /// This can be a Ped or a Vehicle.
        /// </summary>
        /// <param name="GameEntity">The game entity.</param>
        /// <param name="EntitySize">Size of the information.</param>
        /// <param name="Count">If drawing the small boxes, the number of it.</param>
        public void EntityInfo(object GameEntity, InfoSize EntitySize, int Count = 0)
        {
            // Store general usage information
            float HealthNow = 0;
            float HealthMax = 0;
            string EntityName = string.Empty;
            Point BackgroundPosition = Point.Empty;
            // And use single line if's for the rest
            float TextSize = EntitySize == InfoSize.Small ? .3f : .325f;
            Size InformationSize = EntitySize == InfoSize.Small ? LiteralSize(Config.SquadWidth, Config.SquadHeight) : LiteralSize(Config.PlayerWidth, Config.PlayerHeight);
            Size HealthSize = EntitySize == InfoSize.Small ? LiteralSize(Config.SquadHealthWidth, Config.SquadHealthHeight) : LiteralSize(Config.PlayerHealthWidth, Config.PlayerHealthHeight);
            Size HealthPosition = EntitySize == InfoSize.Small ? LiteralSize(Config.SquadHealthX, Config.SquadHealthY) : LiteralSize(Config.PlayerHealthX, Config.PlayerHealthY);

            // Check what type of game entity has been sent and set the appropiate parameters
            if (GameEntity is Ped GamePed)
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, GamePed) - 100;
                HealthMax = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, GamePed) - 100;

                BackgroundPosition = EntitySize == InfoSize.Small ? Config.GetSpecificPosition(Position.SquadInfo, Count) : Config.GetSpecificPosition(Position.PlayerInfo, 1);

                // Set the correct ped name
                if (GamePed.IsPlayer)
                {
                    EntityName = Game.Player.Name;
                }
                else if (Names.ContainsKey(GamePed.Model.GetHashCode().ToString()))
                {
                    EntityName = Names[GamePed.Model.GetHashCode().ToString()];
                }
                else
                {
                    EntityName = GamePed.Model.Hash.ToString();
                }
            }
            else if (GameEntity is Vehicle Car)
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Car);
                HealthMax = 1000;
                EntityName = Car.FriendlyName;
                BackgroundPosition = new Point((int)(UI.WIDTH * Config.PlayerX) + (int)(UI.WIDTH * Config.SquareWidth) + (int)(UI.WIDTH * Config.CommonX), (int)(UI.HEIGHT * Config.PlayerY));
            }
            else
            {
                throw new InvalidCastException("This is not a valid Entity.");
            }

            // Draw the general background
            UIRectangle Background = new UIRectangle(BackgroundPosition, InformationSize, Colors.Backgrounds);
            Background.Draw();

            // Calculate the percentage of health and width
            float Percentage = (HealthNow / HealthMax) * 100;
            float Width = (Percentage / 100) * HealthSize.Width;

            // Finally, return the new size
            HealthSize = new Size((int)Width, HealthSize.Height);

            // Draw the entity health
            UIRectangle Health = new UIRectangle(BackgroundPosition + LiteralSize(Config.PlayerHealthX, Config.PlayerHealthY), HealthSize, Colors.GetHealthColor(HealthNow, HealthMax));
            Health.Draw();

            // Draw the health dividers
            foreach (Point Position in Config.GetDividerPositions(BackgroundPosition, !(EntitySize == InfoSize.Small)))
            {
                UIRectangle Divider = new UIRectangle(Position, LiteralSize(Config.DividerWidth, Config.DividerHeight), Colors.Dividers);
                Divider.Draw();
            }

            // Draw the entity name
            UIText Name = new UIText(EntityName, BackgroundPosition + LiteralSize(Config.SquadNameX, Config.SquadNameY), TextSize);
            Name.Draw();
        }

        /// <summary>
        /// Draws the player weapon information.
        /// </summary>
        /// <param name="Weapon">The player weapon.</param>
        /// <param name="Style">The weapon carry style.</param>
        public void WeaponInfo(Weapon PlayerWeapon, WeaponStyle Style)
        {
            // Check if the player is using a secondary weapon
            bool Sidearm = Style == WeaponStyle.Sidearm;

            // Store the weapon name
            string Name = Weapon.GetDisplayNameFromHash(PlayerWeapon.Hash).Replace("WTT_", string.Empty);

            // Store the information for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? Config.GetSpecificPosition(Position.PlayerInfo, 3) : Config.GetSpecificPosition(Position.PlayerInfo, 2);
            Point WeaponLocation = Sidearm ? Config.GetSpecificPosition(Position.PlayerWeapon, 3) : Config.GetSpecificPosition(Position.PlayerWeapon, 2);

            // Draw the background and ammo quantity
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds);
            AmmoBackground.Draw();
            UIText Text = new UIText(PlayerWeapon.AmmoInClip.ToString(), BackgroundLocation + LiteralSize(Config.AmmoX, Config.AmmoY), .6f, Color.White, (GTA.Font)2, true);
            Text.Draw();

            // Get the weapon bitmap
            // If is not there, return
            Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
            if (WeaponBitmap == null)
            {
                return;
            }

            // Finally, draw the weapon image with the respective background
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, LiteralSize(Config.PlayerWidth, Config.PlayerHeight) - LiteralSize(Config.SquareWidth, 0) - LiteralSize(Config.CommonX, 0), Colors.Backgrounds);
            WeaponBackground.Draw();
            DrawImage($"Weapon{Name}", WeaponLocation + LiteralSize(Config.WeaponX, Config.WeaponY), LiteralSize(Config.WeaponWidth, Config.WeaponHeight));
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
            Size MarkerSize = LiteralSize(Config.DeadMarkerWidth, Config.DeadMarkerHeight);
            // Offset the marker by half width to center, and full height to put on top.
            ScreenPos.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            DrawImage("DeadMarker", ScreenPos, MarkerSize);
        }
    }
}
