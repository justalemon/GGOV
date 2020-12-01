using GTA;
using GTA.Native;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Font = GTA.UI.Font;

namespace GGO.Inventory
{
    /// <summary>
    /// The Inventory UI Item.
    /// </summary>
    public class PlayerInventory : IRecalculable, IProcessable
    {
        #region Fields

        private static readonly List<WeaponHash> weapons = ((WeaponHash[])Enum.GetValues(typeof(WeaponHash))).Where(x => Tools.GetWeaponType(x) == WeaponType.Primary || Tools.GetWeaponType(x) == WeaponType.Secondary).ToList();

        private const float genericHeight = 50;
        private const float healthHeight = 23;
        private const float healthWidth = 258;
        private const float healthCorner = 1;
        private const float weaponWidth = 143;

        private Ped lastPed = null;
        private readonly ScaledRectangle background = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(130, 90, 90, 90)
        };
        private readonly ScaledRectangle top = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(200, 47, 52, 62)
        };

        private readonly ScaledRectangle playerColor = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(250, 33, 145, 198)
        };
        private readonly ScaledText playerName = new ScaledText(PointF.Empty, Game.Player.Name, 0.625f, Font.Monospace);
        private readonly ScaledTexture playerGender = new ScaledTexture("ggo", "");

        private readonly ScaledText textStatus = new ScaledText(PointF.Empty, "Status", 0.325f);
        private readonly ScaledText textStrage = new ScaledText(PointF.Empty, "Strage", 0.325f);
        private readonly ScaledTexture settingsIcon = new ScaledTexture("ggo", "icon_settings");

        private readonly ScaledText healthText = new ScaledText(PointF.Empty, "Life", 0.225f);
        private readonly ScaledRectangle healthBar = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerTop = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerBottom = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerLeft = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerRight = new ScaledRectangle(PointF.Empty, SizeF.Empty);

        private readonly ScaledText weaponText = new ScaledText(PointF.Empty, "Arms", 0.225f);
        private readonly List<List<ScaledRectangle>> weaponCorners = new List<List<ScaledRectangle>>();
        private readonly Dictionary<WeaponHash, ScaledTexture> weaponImages = new Dictionary<WeaponHash, ScaledTexture>();
        private readonly Dictionary<WeaponHash, ScaledTexture> weaponVisible = new Dictionary<WeaponHash, ScaledTexture>();
        private SizeF weaponAreaSize = SizeF.Empty;
        private int weaponIndex = 0;

        private readonly ScaledText itemsText = new ScaledText(PointF.Empty, "Items", 0.225f);
        private readonly List<List<ScaledRectangle>> itemsCorners = new List<List<ScaledRectangle>>();

        #endregion

        #region Properties

        /// <summary>
        /// If the inventory is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Constructor

        internal PlayerInventory()
        {
            for (int i = 0; i < 6; i++)
            {
                List<ScaledRectangle> list = new List<ScaledRectangle>
                {
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty)
                };
                weaponCorners.Add(list);
            }

            for (int i = 0; i < (6 * 3); i++)
            {
                List<ScaledRectangle> list = new List<ScaledRectangle>
                {
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty),
                    new ScaledRectangle(PointF.Empty, SizeF.Empty)
                };
                itemsCorners.Add(list);
            }

            Recalculate();
            UpdateWeapons();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the menu.
        /// </summary>
        public void Recalculate()
        {
            // Get the current resolution for calculations
            float width = 1f.ToXAbsolute();
            const float height = 1080;

            // And set the positions
            background.Size = new SizeF(575, 710);
            background.Position = new PointF((width * 0.5f) - (background.Size.Width * 0.5f), (height * 0.5f) - (background.Size.Height * 0.5f));

            top.Size = new SizeF(background.Size.Width, 155);
            top.Position = background.Position;

            playerColor.Size = new SizeF(255, 60);
            playerColor.Position = new PointF(background.Position.X - 23, background.Position.Y + 30);
            playerName.Position = new PointF(playerColor.Position.X + 92, playerColor.Position.Y + 7);

            playerGender.Size = new SizeF(50, 50);
            playerGender.Position = new PointF(playerColor.Position.X + 40, playerColor.Position.Y + 5);

            float statusY = background.Position.Y + 173;
            textStatus.Position = new PointF(background.Position.X + 197, statusY);
            textStrage.Position = new PointF(background.Position.X + 337, statusY);
            settingsIcon.Size = new SizeF(20, 20);
            settingsIcon.Position = new PointF(background.Position.X + background.Size.Width - 25 - settingsIcon.Size.Width, statusY + 3);

            healthText.Position = new PointF(background.Position.X + 265, background.Position.Y + 37);
            healthBar.Position = new PointF(background.Position.X + 295, background.Position.Y + 37);

            healthCornerTop.Size = new SizeF(healthWidth, healthCorner);
            healthCornerTop.Position = healthBar.Position;
            healthCornerBottom.Size = new SizeF(healthWidth, healthCorner * 2);
            healthCornerBottom.Position = new PointF(healthBar.Position.X, healthBar.Position.Y + healthHeight - healthCorner);
            healthCornerLeft.Size = new SizeF(healthCorner, healthHeight);
            healthCornerLeft.Position = healthBar.Position;
            healthCornerRight.Size = new SizeF(healthCorner, healthHeight);
            healthCornerRight.Position = new PointF(healthBar.Position.X + healthWidth - healthCorner, healthBar.Position.Y);

            float baseY = background.Position.Y + 252;

            const float itemWidth = 107;
            const float itemWidthFromX = 35;
            itemsText.Position = new PointF(background.Position.X + itemWidthFromX, baseY - 23);
            for (int iy = 0; iy < 6; iy++)
            {
                float itemBaseY = baseY + (iy * genericHeight) + (iy * 14);

                for (int ix = 0; ix < 3; ix++)
                {
                    int i = (iy * 3) + ix;
                    float itemBaseX = background.Position.X + itemWidthFromX + ((itemWidth + 4) * ix);

                    // Top
                    itemsCorners[i][0].Size = new SizeF(itemWidth, healthCorner * 2);
                    itemsCorners[i][0].Position = new PointF(itemBaseX, itemBaseY);
                    // Bottom
                    itemsCorners[i][1].Size = new SizeF(itemWidth, healthCorner);
                    itemsCorners[i][1].Position = new PointF(itemBaseX, itemBaseY + genericHeight);
                    // Left
                    itemsCorners[i][2].Size = new SizeF(healthCorner, genericHeight);
                    itemsCorners[i][2].Position = new PointF(itemBaseX, itemBaseY);
                    // Right
                    itemsCorners[i][3].Size = new SizeF(healthCorner * 2, genericHeight);
                    itemsCorners[i][3].Position = new PointF(itemBaseX + itemWidth - healthCorner, itemBaseY);
                    // Center
                    itemsCorners[i][4].Size = new SizeF(healthCorner * 2, genericHeight - (8 * 2));
                    itemsCorners[i][4].Position = new PointF(itemBaseX + (itemWidth * 0.5f) + (itemsCorners[i][4].Size.Width * 0.5f), itemBaseY + (genericHeight * 0.5f) - (itemsCorners[i][4].Size.Height * 0.5f));
                }
            }

            float weaponBaseX = background.Position.X + background.Size.Width - weaponWidth - 40;
            weaponText.Position = new PointF(weaponBaseX, baseY - 23);
            for (int i = 0; i < 6; i++)
            {
                float weaponBaseY = baseY + (i * genericHeight) + (i * 14);

                // Top
                weaponCorners[i][0].Size = new SizeF(weaponWidth, healthCorner * 2);
                weaponCorners[i][0].Position = new PointF(weaponBaseX, weaponBaseY);
                // Bottom
                weaponCorners[i][1].Size = new SizeF(weaponWidth, healthCorner);
                weaponCorners[i][1].Position = new PointF(weaponBaseX, weaponBaseY + genericHeight);
                // Left
                weaponCorners[i][2].Size = new SizeF(healthCorner, genericHeight);
                weaponCorners[i][2].Position = new PointF(weaponBaseX, weaponBaseY);
                // Right
                weaponCorners[i][3].Size = new SizeF(healthCorner, genericHeight);
                weaponCorners[i][3].Position = new PointF(weaponBaseX + weaponWidth - healthCorner, weaponBaseY);
            }

            weaponAreaSize = new SizeF(weaponWidth, (genericHeight * 6) +  (14 * 5));
        }
        /// <summary>
        /// Processes the inventory.
        /// </summary>
        public void Process()
        {
            // If the last ped is not the same as the current one, update the player gender
            if (lastPed != Game.Player.Character)
            {
                switch (Game.Player.Character.Gender)
                {
                    case Gender.Male:
                        playerGender.Texture = "gender_m";
                        break;
                    case Gender.Female:
                        playerGender.Texture = "gender_f";
                        break;
                    default:
                        playerGender.Texture = "gender_u";
                        break;
                }
                lastPed = Game.Player.Character;
            }

            // If is not visible, return
            if (!Visible)
            {
                return;
            }

            // Show the cursor during this frame
            Function.Call(Hash._SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME);

            // Disable the firing controls
            Game.DisableControlThisFrame(Control.Attack);
            Game.DisableControlThisFrame(Control.Aim);
            Game.DisableControlThisFrame(Control.Attack2);
            // And the HUD Reticle
            Hud.HideComponentThisFrame(HudComponent.Reticle);

            // Update the value of the health bar
            float percentage = (Game.Player.Character.HealthFloat - 100) / (Game.Player.Character.MaxHealthFloat - 100);
            if (percentage < 0)
            {
                percentage = 0;
            }
            else if (percentage > 1)
            {
                percentage = 1;
            }
            else if (float.IsNaN(percentage))
            {
                percentage = 0;
            }
            healthBar.Size = new SizeF(healthWidth * percentage, healthHeight);

            // Make sure that the weapon images are up to date
            bool weaponUpdateRequired = false;
            foreach (WeaponHash weaponHash in weapons)
            {
                // Check if the player has the weapon
                bool has = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, weaponHash, false);

                // If the player does not has the weapon and is on the list, or he does but it is, remove it
                if ((!has && weaponImages.ContainsKey(weaponHash)) || (has && !weaponImages.ContainsKey(weaponHash)))
                {
                    weaponUpdateRequired = true;
                    break;
                }
            }
            // If they are not, update them
            if (weaponUpdateRequired)
            {
                UpdateWeapons();
            }

            // Draw the UI Elements
            background.Draw();
            top.Draw();
            playerColor.Draw();
            playerName.Draw();
            playerGender.Draw();
            textStatus.Draw();
            textStrage.Draw();
            settingsIcon.Draw();
            healthText.Draw();
            healthCornerTop.Draw();
            healthCornerBottom.Draw();
            healthCornerLeft.Draw();
            healthCornerRight.Draw();
            healthBar.Draw();

            itemsText.Draw();
            foreach (List<ScaledRectangle> itemCorner in itemsCorners)
            {
                foreach (ScaledRectangle iCorner in itemCorner)
                {
                    iCorner.Draw();
                }
            }

            // Corners of the weapon spaces
            weaponText.Draw();
            foreach (List<ScaledRectangle> corners in weaponCorners)
            {
                foreach (ScaledRectangle corner in corners)
                {
                    corner.Draw();
                }
            }
            // And the weapon images themselves
            foreach (KeyValuePair<WeaponHash, ScaledTexture> weapon in weaponVisible)
            {
                weapon.Value.Draw();
            }

            // Check if the user clicked any of the weapons
            // If he did, switch to it and update the weapons on the screen
            foreach (KeyValuePair<WeaponHash, ScaledTexture> weapon in weaponVisible)
            {
                if (Game.IsControlJustPressed(Control.CursorAccept) && Screen.IsCursorInArea(weapon.Value.Position, weapon.Value.Size))
                {
                    Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weapon.Key, true);
                    UpdateVisibleWeapons();
                    break;
                }
            }

            // If the player moved the mouse wheel up or down when the weapons are selected, move
            bool isMouseOnWeapons = Screen.IsCursorInArea(weaponCorners[0][0].Position, weaponAreaSize);
            if (Game.IsControlJustPressed(Control.PhoneScrollForward) && isMouseOnWeapons)
            {
                weaponIndex++;
                UpdateVisibleWeapons();
            }
            else if (Game.IsControlJustPressed(Control.PhoneScrollBackward) && isMouseOnWeapons)
            {
                weaponIndex--;
                UpdateVisibleWeapons();
            }

            // If the settings button was pressed, open the settings menu
            if (Game.IsControlJustPressed(Control.CursorAccept) && Screen.IsCursorInArea(settingsIcon.Position, settingsIcon.Size))
            {
                GGO.menu.Open();
            }
        }
        /// <summary>
        /// Updates the weapon on the list.
        /// </summary>
        private void UpdateWeapons()
        {
            // Remove all of the existing weapon activators
            weaponImages.Clear();
            // Iterate the weapons and add the images for the weapons that the player has
            foreach (WeaponHash weaponHash in weapons)
            {
                if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, weaponHash, false))
                {
                    weaponImages.Add(weaponHash, new ScaledTexture("ggo_weapons", $"{(int)weaponHash}"));
                }
            }

            // And recalculate the position of the visible weapons
            UpdateVisibleWeapons();
        }
        /// <summary>
        /// Recalculates the position of the Weapons shown on the screen.
        /// </summary>
        private void UpdateVisibleWeapons()
        {
            // Clear the list of existing weapons
            weaponVisible.Clear();

            // If there are no weapons, return
            if (weaponImages.Count == 0)
            {
                return;
            }

            // If the index of the weapons is over the number of weapon images, set the maximum probable index
            if (weaponImages.Count > 6 && weaponIndex + 6 > weaponImages.Count)
            {
                weaponIndex = weaponImages.Count - 6;
            }
            else if (weaponImages.Count < 6 || weaponIndex < 0)
            {
                weaponIndex = 0;
            }

            // Then, get the weapons and change their positions on screen
            for (int i = 0; i < 6 && i + weaponIndex < weaponImages.Count; i++)
            {
                KeyValuePair<WeaponHash, ScaledTexture> image = weaponImages.ElementAt(i + weaponIndex);

                image.Value.Position = weaponCorners[i][0].Position;
                image.Value.Size = new SizeF(weaponWidth, genericHeight);
                weaponImages[image.Key] = image.Value;

                weaponVisible.Add(image.Key, image.Value);
            }
        }

        #endregion
    }
}
