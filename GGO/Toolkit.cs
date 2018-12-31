using GGO.Properties;
using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GGO
{
    public static class Toolkit
    {
        /// <summary>
        /// The indexes of the images.
        /// </summary>
        private static int Index = 0;
        
        /// <summary>
        /// Draws an image based on a Bitmap.
        /// </summary>
        /// <param name="Resource">The Bitmap image to draw.</param>
        /// <param name="Position">Where the image should be drawn.</param>
        /// <param name="Sizes">The size of the image.</param>
        public static void Image(Bitmap Resource, string Filename, Point Position, Size Sizes)
        {
            // This is going to be our image location
            string OutputFile = Path.Combine(Path.GetTempPath(), "GGO", Filename + ".png");

            // If the file already exists, return it and don't waste resources
            if (!File.Exists(OutputFile))
            {
                // If our %TEMP%\GGO folder does not exist, create it
                if (!Directory.Exists(Path.GetDirectoryName(OutputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(OutputFile));
                }

                // Create a memory stream
                MemoryStream ImageStream = new MemoryStream();
                // Dump the image into it
                Resource.Save(ImageStream, ImageFormat.Png);
                // And close the stream
                ImageStream.Close();
                // Finally, write the stream into the disc
                File.WriteAllBytes(OutputFile, ImageStream.ToArray());
            }

            UI.DrawTexture(OutputFile, Index, 0, 200, Position, Sizes);
            Index++;
        }

        /// <summary>
        /// Cleans up the existing index list.
        /// </summary>
        public static void ResetIndex()
        {
            Index = 0;
        }

        /// <summary>
        /// Draws an icon with the respective background.
        /// </summary>
        /// <param name="File">The file to draw.</param>
        /// <param name="Position">The on-screen position.</param>
        public static void Icon(Bitmap Original, string Filename, Point Position)
        {
            // Draw the background
            UIRectangle Background = new UIRectangle(Position, GGO.Config.SquaredBackground, Colors.Backgrounds);
            Background.Draw();
            // And the image over it
            Image(Original, Filename, Position + GGO.Config.IconPosition, GGO.Config.IconSize);
        }

        /// <summary>
        /// Draws the information of a Game Entity.
        /// This can be a Ped or a Vehicle.
        /// </summary>
        /// <param name="GameEntity">The game entity.</param>
        /// <param name="Small">If a small information box should be drawn.</param>
        /// <param name="Count">If drawing the small boxes, the number of it.</param>
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
            if (GameEntity is Ped GamePed)
            {
                HealthNow = Function.Call<int>(Hash.GET_ENTITY_HEALTH, GamePed) - 100;
                HealthMax = Function.Call<int>(Hash.GET_PED_MAX_HEALTH, GamePed) - 100;
                
                BackgroundPosition = Small ? Calculations.GetSquadPosition(GGO.Config, Count, true) : GGO.Config.PlayerInformation;

                // Set the correct ped name
                if (GGO.Config.Name == "default" && GamePed.IsPlayer)
                {
                    EntityName = Game.Player.Name;
                }
                else if (GamePed.IsPlayer)
                {
                    EntityName = GGO.Config.Name;
                }
                else if (GGO.Config.Raw["names"][GamePed.Model.Hash.ToString()] != null)
                {
                    EntityName = (string)GGO.Config.Raw["names"][GamePed.Model.Hash.ToString()];
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
        /// <param name="Weapon">The player weapon.</param>
        /// <param name="Style">The weapon carry style.</param>
        public static void WeaponInfo(Weapon PlayerWeapon, WeaponStyle Style)
        {
            // Check if the player is using a secondary weapon
            bool Sidearm = Style == WeaponStyle.Sidearm;

            // Store the weapon name
            string Name = Weapon.GetDisplayNameFromHash(PlayerWeapon.Hash).Replace("WTT_", string.Empty);

            // Store the information for the primary or secondary weapon
            Point BackgroundLocation = Sidearm ? GGO.Config.SecondaryBackground : GGO.Config.PrimaryBackground;
            Point AmmoLocation = Sidearm ? GGO.Config.SecondaryAmmo : GGO.Config.PrimaryAmmo;
            Point WeaponLocation = Sidearm ? GGO.Config.SecondaryWeapon : GGO.Config.PrimaryWeapon;

            // Draw the background and ammo quantity
            UIRectangle AmmoBackground = new UIRectangle(BackgroundLocation, GGO.Config.SquaredBackground, Colors.Backgrounds);
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
            UIRectangle WeaponBackground = new UIRectangle(WeaponLocation, GGO.Config.WeaponBackground, Colors.Backgrounds);
            WeaponBackground.Draw();
            Image(WeaponBitmap, "Weapon" + Name, WeaponLocation + GGO.Config.WeaponPosition, GGO.Config.WeaponSize);
        }

        /// <summary>
        /// Draws a dead marker over the ped head.
        /// </summary>
        /// <param name="GamePed">The ped where the dead marker should be drawn.</param>
        public static void DeadMarker(Ped GamePed)
        {
            // Get the coordinates for the head
            Vector3 HeadCoord = GamePed.GetBoneCoord(Bone.SKEL_Head);
            // Get the On Screen position for the head
            Point ScreenPos = UI.WorldToScreen(HeadCoord);

            // Get distance ratio by Ln(Distance + Sqrt(e)), then calculate size of marker using intercept thereom.
            double Ratio = Math.Log(Vector3.Distance(Game.Player.Character.Position, HeadCoord) + 1.65);
            // Calculate the marker size based on the distance between player and dead ped
            Size MarkerSize = new Size((int)(GGO.Config.DeadMarker.Width / Ratio), (int)(GGO.Config.DeadMarker.Height / Ratio));
            // Offset the marker by half width to center, and full height to put on top.
            ScreenPos.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the marker on screen
            Image(Resources.DeadMarker, nameof(Resources.DeadMarker), ScreenPos, MarkerSize);
        }
    }
}
