using GGO.Shared;
using GGO.Shared.Properties;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

namespace GGO.Singleplayer
{
    public static class Toolkit
    {
        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="File">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        public static void Icon(string File, Point Position)
        {
            // Draw the background
            UIRectangle Background = new UIRectangle(Position, GGO.Config.SquaredBackground, Colors.Backgrounds);
            Background.Draw();
            // And the image over it
            UI.DrawTexture(File, 0, 0, 100, Position, GGO.Config.IconSize);
        }

        public static void EntityInfo(object GameEntity, bool Small = false, int Count = 0)
        {
            // Store general usage information
            float HealthNow = 0;
            float HealthMax = 0;
            string EntityName = string.Empty;
            Point BackgroundPosition = Point.Empty;
            // And use single line if's for the rest
            float TextSize = Small ? .3f : .325f;
            Size InformationSize = Small ? GGO.Config.SquadSize : GGO.Config.PlayerSize;
            Size HealthSize = Small ? GGO.Config.SquadHealthSize : GGO.Config.PlayerHealthSize;
            Size HealthPosition = Small ? GGO.Config.SquadHealthPos : GGO.Config.PlayerHealthPos;

            // Check what type of game entity has been sent and set the appropiate parameters
            if (GameEntity.GetType() == typeof(Ped))
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, (Ped)GameEntity) - 100;
                HealthMax = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, (Ped)GameEntity) - 100;
                
                BackgroundPosition = Small ? Calculations.GetSquadPosition(GGO.Config, Count, true) : GGO.Config.PlayerInformation;

                // Set the correct ped name
                if (GGO.Config.Name == "default" && ((Ped)GameEntity).IsPlayer)
                {
                    EntityName = Game.Player.Name;
                }
                else if (((Ped)GameEntity).IsPlayer)
                {
                    EntityName = GGO.Config.Name;
                }
                else if (GGO.Config.Raw["names"][((Ped)GameEntity).Model.Hash.ToString()] != null)
                {
                    EntityName = (string)GGO.Config.Raw["names"][((Ped)GameEntity).Model.Hash.ToString()];
                }
                else
                {
                    EntityName = ((Ped)GameEntity).Model.Hash.ToString();
                }
            }
            else if (GameEntity.GetType() == typeof(Vehicle))
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, (Vehicle)GameEntity);
                HealthMax = 1000;
                EntityName = ((Vehicle)GameEntity).FriendlyName;
                BackgroundPosition = GGO.Config.VehicleInformation;
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
            UIRectangle Health = new UIRectangle(BackgroundPosition + GGO.Config.PlayerHealthPos, HealthSize, Colors.GetHealthColor(HealthNow, HealthMax));
            Health.Draw();

            // Draw the health dividers
            foreach (Point Position in Calculations.GetDividerPositions(GGO.Config, BackgroundPosition, !Small))
            {
                UIRectangle Divider = new UIRectangle(Position, GGO.Config.DividerSize, Colors.Dividers);
                Divider.Draw();
            }

            // Draw the entity name
            UIText Name = new UIText(EntityName, BackgroundPosition + GGO.Config.NamePosition, TextSize);
            Name.Draw();
        }

        /// <summary>
        /// Draws the player weapon information.
        /// </summary>
        /// <param name="Style">The weapon carry style.</param>
        /// <param name="Ammo">The ammo on the current magazine.</param>
        /// <param name="Weapon">The readable name of the weapon.</param>
        public static void WeaponInfo(Checks.WeaponStyle Style, int Ammo, string Weapon)
        {
            // Check if the player is using a secondary weapon
            bool Sidearm = Style == Checks.WeaponStyle.Sidearm;

            // Store the information for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? GGO.Config.SecondaryBackground : GGO.Config.PrimaryBackground;
            Point AmmoLocation = Sidearm ? GGO.Config.SecondaryAmmo : GGO.Config.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? GGO.Config.SecondaryWeapon : GGO.Config.PrimaryWeapon;
            string Name = Sidearm ? "Secondary" : "Primary";

            // Draw the background and ammo quantity
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, GGO.Config.SquaredBackground, Colors.Backgrounds);
            AmmoBackground.Draw();
            UIText Text = new UIText(Ammo.ToString(), AmmoLocation, .6f, Color.White, (GTA.Font)2, true);

            // Get the weapon bitmap
            // If is not there, return
            Bitmap WeaponBitmap = Images.GetWeaponImages(Weapon);
            if (WeaponBitmap == null)
            {
                return;
            }

            // Finally, draw the weapon image with the respective background
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, GGO.Config.WeaponBackground, Colors.Backgrounds);
            WeaponBackground.Draw();
            UI.DrawTexture(Images.ResourceToPNG(WeaponBitmap, "Gun" + Weapon + Name), 0, 0, 100, WeaponLocation + GGO.Config.IconPosition, GGO.Config.WeaponSize);
        }

        /// <summary>
        /// Draws a dead marker over the ped head.
        /// </summary>
        /// <param name="Position">The on-screen position of the ped health.</param>
        /// <param name="Distance">The distance from the player to the ped.</param>
        /// <param name="Hash">The ped hash (not the model hash).</param>
        public static void DeadMarker(Point Position, float Distance, int Hash)
        {
            // Calculate the marker size based on the distance between player and dead ped
            Size MarkerSize = Calculations.GetMarkerSize(GGO.Config, Distance);

            // Offset the marker by half width to center, and full height to put on top.
            Position.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            UI.DrawTexture(Images.ResourceToPNG(Resources.DeadMarker, "DeadMarker" + Hash), 0, 0, 100, Position, MarkerSize);
        }
    }
}
