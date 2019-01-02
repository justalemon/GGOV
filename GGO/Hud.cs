using GGO.Properties;
using GGO.UserData;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using System;
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
        private HudConfig Config;

        public Hud()
        {
            // Start by parsing the config
            Config = JsonConvert.DeserializeObject<HudConfig>(File.ReadAllText("scripts\\GGO\\Hud.json"));

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

            // Get all of the peds and store them during this tick
            Ped[] NearbyPeds = World.GetAllPeds().OrderBy(P => P.GetHashCode()).ToArray();

            // Draw the squad information on the top left if the user wants to
            if (Config.Squad)
            {
                // Store the peds that are friend of us
                Ped[] FriendlyPeds = NearbyPeds.Where(P => P.IsFriendly() && P.IsMissionEntity()).ToArray();

                // And iterate over them
                foreach (Ped SquadMember in FriendlyPeds)
                {
                    // Get the number of the ped
                    int Number = Array.IndexOf(FriendlyPeds, SquadMember);

                    // Select the correct image and name for the file
                    Bitmap ImageType = SquadMember.IsAlive ? Resources.IconAlive : Resources.IconDead;
                    string ImageName = SquadMember.IsAlive ? nameof(Resources.IconAlive) : nameof(Resources.IconDead);

                    // Draw the icon and the ped info
                    Icon(ImageType, ImageName, Calculations.GetSquadPosition(Config, Number));
                    EntityInfo(SquadMember, true, Number);
                }
            }

            // Draw the dead ped markers over their heads, if the user wants to
            if (Config.DeadMarkers)
            {
                // Iterate over the dead peds
                foreach (Ped DeadPed in NearbyPeds.Where(P => P.IsDead && P.IsOnScreen).ToArray())
                {
                    // And draw the dead marker
                    DeadMarker(DeadPed);
                }
            }

            // Then, start by drawing the player info
            Icon(Resources.IconAlive, nameof(Resources.IconAlive), LiteralPoint(Config.PlayerX, Config.PlayerY));
            EntityInfo(Game.Player.Character);

            // If the player is on a vehicle, also draw that information
            if (Game.Player.Character.CurrentVehicle != null && Config.VehicleInfo)
            {
                Point VehicleIcon = new Point((int)(UI.WIDTH * Config.PlayerX), (int)(UI.WIDTH * Config.PlayerY) - (int)(UI.WIDTH * Config.SquareWidth) - (int)(UI.WIDTH * Config.CommonY));
                Icon(Resources.IconVehicle, nameof(Resources.IconVehicle), VehicleIcon);
                EntityInfo(Game.Player.Character.CurrentVehicle);
            }

            // Get the current weapon style
            WeaponStyle CurrentStyle = Game.Player.Character.Weapons.GetStyle();

            // Calculate and store the position of the primary and secondary icons
            Point PrimaryIcon = new Point((int)(UI.WIDTH * Config.PlayerX), (int)(UI.WIDTH * Config.PlayerY) + (int)(UI.WIDTH * Config.CommonY) + (int)(UI.WIDTH * Config.SquareWidth));
            Point PrimaryBackground = new Point(PrimaryIcon.X + (int)(UI.WIDTH * Config.SquareWidth) + (int)(UI.WIDTH * Config.CommonX), PrimaryIcon.Y);
            Point SecondaryIcon = new Point((int)(UI.WIDTH * Config.PlayerX), (int)(UI.WIDTH * Config.PlayerY) + ((int)(UI.WIDTH * Config.CommonY) * 2) + ((int)(UI.WIDTH * Config.SquareWidth) * 2));
            Point SecondaryBackground = new Point(SecondaryIcon.X + (int)(UI.WIDTH * Config.SquareWidth) + (int)(UI.WIDTH * Config.CommonX), SecondaryIcon.Y);

            // And draw the weapon information for both the primary and secondary
            // If they are not available, draw dummies instead
            if (CurrentStyle == WeaponStyle.Main || CurrentStyle == WeaponStyle.Double)
            {
                Icon(Resources.IconWeapon, nameof(Resources.IconWeapon), PrimaryIcon);
                WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Icon(Resources.NoWeapon, nameof(Resources.NoWeapon), PrimaryIcon);
                Icon(Resources.NoWeapon, nameof(Resources.NoWeapon), PrimaryBackground);
            }
            if (CurrentStyle == WeaponStyle.Sidearm || CurrentStyle == WeaponStyle.Double)
            {
                Icon(Resources.IconWeapon, nameof(Resources.IconWeapon), SecondaryIcon);
                WeaponInfo(Game.Player.Character.Weapons.Current, CurrentStyle);
            }
            else
            {
                Icon(Resources.NoWeapon, nameof(Resources.NoWeapon), SecondaryIcon);
                Icon(Resources.NoWeapon, nameof(Resources.NoWeapon), SecondaryBackground);
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
        /// <param name="File">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        public void Icon(Bitmap Original, string Filename, Point Position)
        {
            // Draw the background
            UIRectangle Background = new UIRectangle(Position, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds);
            Background.Draw();
            // And the image over it
            DrawImage(Original, Filename, Position + LiteralSize(Config.IconX, Config.IconY), LiteralSize(Config.IconWidth, Config.IconHeight));
        }

        /// <summary>
        /// Draws the information of a Game Entity.
        /// This can be a Ped or a Vehicle.
        /// </summary>
        /// <param name="GameEntity">The game entity.</param>
        /// <param name="Small">If a small information box should be drawn.</param>
        /// <param name="Count">If drawing the small boxes, the number of it.</param>
        public void EntityInfo(object GameEntity, bool Small = false, int Count = 0)
        {
            // Store general usage information
            float HealthNow = 0;
            float HealthMax = 0;
            string EntityName = string.Empty;
            Point BackgroundPosition = Point.Empty;
            // And use single line if's for the rest
            float TextSize = Small ? .3f : .325f;
            Size InformationSize = Small ? LiteralSize(Config.SquadWidth, Config.SquadHeight) : LiteralSize(Config.PlayerWidth, Config.PlayerHeight);
            Size HealthSize = Small ? LiteralSize(Config.SquadHealthWidth, Config.SquadHealthHeight) : LiteralSize(Config.PlayerHealthWidth, Config.PlayerHealthHeight);
            Size HealthPosition = Small ? LiteralSize(Config.SquadHealthX, Config.SquadHealthY) : LiteralSize(Config.PlayerHealthX, Config.PlayerHealthY);

            // Check what type of game entity has been sent and set the appropiate parameters
            if (GameEntity is Ped GamePed)
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, GamePed) - 100;
                HealthMax = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, GamePed) - 100;

                BackgroundPosition = Small ? Calculations.GetSquadPosition(Config, Count, true) : new Point((int)(UI.WIDTH * Config.PlayerX) + (int)(UI.WIDTH * Config.SquareWidth) + (int)(UI.WIDTH * Config.CommonX), (int)(UI.HEIGHT * Config.PlayerY));

                // Set the correct ped name
                if (GamePed.IsPlayer)
                {
                    EntityName = Game.Player.Name;
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
            foreach (Point Position in Calculations.GetDividerPositions(Config, BackgroundPosition, !Small))
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
            /*
            // Check if the player is using a secondary weapon
            bool Sidearm = Style == WeaponStyle.Sidearm;

            // Store the weapon name
            string Name = Weapon.GetDisplayNameFromHash(PlayerWeapon.Hash).Replace("WTT_", string.Empty);

            // Store the information for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? GGO.Config.SecondaryBackground : GGO.Config.PrimaryBackground;
            Point AmmoLocation = Sidearm ? GGO.Config.SecondaryAmmo : GGO.Config.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? GGO.Config.SecondaryWeapon : GGO.Config.PrimaryWeapon;

            // Draw the background and ammo quantity
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, LiteralSize(Config.SquareWidth, Config.SquareHeight), Colors.Backgrounds);
            AmmoBackground.Draw();
            UIText Text = new UIText(PlayerWeapon.AmmoInClip.ToString(), AmmoLocation, .6f, Color.White, (GTA.Font)2, true);
            Text.Draw();

            // Get the weapon bitmap
            // If is not there, return
            Bitmap WeaponBitmap = (Bitmap)Resources.ResourceManager.GetObject("Weapon" + Name);
            if (WeaponBitmap == null)
            {
                return;
            }

            // Finally, draw the weapon image with the respective background
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, new Size((int)(UI.WIDTH * Config.PlayerWidth) - (int)(UI.WIDTH * Config.CommonX) - (int)(UI.WIDTH * Config.SquareWidth), (int)(UI.WIDTH * Config.PlayerHeight)), Colors.Backgrounds);
            WeaponBackground.Draw();
            DrawImage(WeaponBitmap, "Weapon" + Name, WeaponLocation + LiteralSize(Config.WeaponX, Config.WeaponY), LiteralSize(Config.WeaponWidth, Config.WeaponHeight));
            */
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
            Size MarkerSize = new Size((int)((int)(UI.WIDTH * Config.DeadMarkerWidth) / Ratio), (int)((int)(UI.WIDTH * Config.DeadMarkerHeight) / Ratio));
            // Offset the marker by half width to center, and full height to put on top.
            ScreenPos.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            DrawImage(Resources.DeadMarker, nameof(Resources.DeadMarker), ScreenPos, MarkerSize);
        }
    }
}
