using GGO.HUD;
using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using PlayerCompanion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Font = GTA.UI.Font;
using Screen = LemonUI.Screen;

namespace GGO.Inventory
{
    /// <summary>
    /// Represents an Item and the Text Label used for showing the count.
    /// </summary>
    internal class ItemPair
    {
        #region Properties

        /// <summary>
        /// The Item used to fetch the information from.
        /// </summary>
        internal Item Item { get; set; }
        /// <summary>
        /// The text used for showing the count.
        /// </summary>
        internal ScaledText Count { get; } = new ScaledText(PointF.Empty, "", 0.41f, Font.ChaletLondon) { Alignment = Alignment.Center };

        #endregion

        #region Constructor

        internal ItemPair(Item item)
        {
            Item = item;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Draws the Item Image and Count.
        /// </summary>
        internal void Draw()
        {
            if (Item is StackableItem stackable)
            {
                Count.Text = stackable.Count.ToString();
            }
            else
            {
                Count.Text = "1";
            }

            Item.Icon.Draw();
            Count.Draw();
        }

        #endregion
    }

    /// <summary>
    /// The Inventory UI Item.
    /// </summary>
    public class PlayerInventory : IRecalculable, IProcessable
    {
        #region Constants

        /// <summary>
        /// The Height of all of the Items and Weapons on the inventory.
        /// </summary>
        private const float genericHeight = 50;
        /// <summary>
        /// The height separation between all of the separators of the inventory.
        /// </summary>
        private const float genericHeightSeparation = 14;

        /// <summary>
        /// The Height of the Health bar.
        /// </summary>
        private const float healthHeight = 23;
        /// <summary>
        /// The Width of the Health bar.
        /// </summary>
        private const float healthWidth = 258;
        /// <summary>
        /// The Size of the corners of the Health Bar.
        /// </summary>
        private const float healthCorner = 1;

        /// <summary>
        /// The Start X position of the Inventory Items from the left side of the background.
        /// </summary>
        private const float itemXFromCorner = 35;
        /// <summary>
        /// The Width of the Inventory items.
        /// </summary>
        private const float itemWidth = 107;

        /// <summary>
        /// The Width of the Weapon Fields.
        /// </summary>
        private const float weaponWidth = 143;

        #endregion

        #region Fields

        /// <summary>
        /// The Weapons that the player can use in the Inventory.
        /// </summary>
        /// TODO: Read this from memory, either via SHVDN or Manually
        private static readonly List<WeaponHash> weapons = ((WeaponHash[])Enum.GetValues(typeof(WeaponHash))).Where(x => Tools.GetWeaponType(x) == WeaponType.Primary || Tools.GetWeaponType(x) == WeaponType.Secondary).ToList();

        /// <summary>
        /// The last know Ped of the player.
        /// </summary>
        private Ped lastPlayerPed = null;

        /// <summary>
        /// The background of the entire GGO Inventory.
        /// </summary>
        private readonly ScaledRectangle background = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(130, 90, 90, 90)
        };
        /// <summary>
        /// The detail on the Top of the Inventory.
        /// </summary>
        private readonly ScaledRectangle top = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(200, 47, 52, 62)
        };

        /// <summary>
        /// The background of the Player Name and Gender.
        /// </summary>
        private readonly ScaledRectangle playerBackground = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(250, 33, 145, 198)
        };
        /// <summary>
        /// The name of the Player.
        /// </summary>
        private readonly ScaledText playerName = new ScaledText(PointF.Empty, Game.Player.Name, 0.625f, Font.Monospace);
        /// <summary>
        /// The Gender of the player, as reported by the game.
        /// </summary>
        private readonly ScaledTexture playerGender = new ScaledTexture("ggo", "");

        /// <summary>
        /// The Status text.
        /// </summary>
        private readonly ScaledText statusText = new ScaledText(PointF.Empty, "Status", 0.325f);
        /// <summary>
        /// The current Status of the player.
        /// This is always shown as "Strage" on SAO:AGGO.
        /// </summary>
        private readonly ScaledText statusCurrent = new ScaledText(PointF.Empty, "Strage", 0.325f);

        /// <summary>
        /// The icon used for opening the GGO Settings.
        /// </summary>
        private readonly ScaledTexture settingsIcon = new ScaledTexture("ggo", "icon_settings");

        /// <summary>
        /// The "Life" text to the left of the Health bar.
        /// </summary>
        private readonly ScaledText healthText = new ScaledText(PointF.Empty, "Life", 0.225f);
        /// <summary>
        /// The Health bar itself.
        /// </summary>
        private readonly ScaledRectangle healthBar = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Top corner of the Health bar.
        /// </summary>
        private readonly ScaledRectangle healthCornerTop = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Bottom corner of the Health bar.
        /// </summary>
        private readonly ScaledRectangle healthCornerBottom = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Left corner of the Health bar.
        /// </summary>
        private readonly ScaledRectangle healthCornerLeft = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Right corner of the Health bar.
        /// </summary>
        private readonly ScaledRectangle healthCornerRight = new ScaledRectangle(PointF.Empty, SizeF.Empty);

        /// <summary>
        /// The Text on top of the Weapons.
        /// </summary>
        private readonly ScaledText itemsText = new ScaledText(PointF.Empty, "Items", 0.225f);
        /// <summary>
        /// The Corners of the Inventory Items.
        /// </summary>
        private readonly List<CornerSet> itemsCorners = new List<CornerSet>();
        /// <summary>
        /// The ammo currently being used by the player.
        /// </summary>
        private readonly Dictionary<WeaponHash, Magazines> itemsAmmo = new Dictionary<WeaponHash, Magazines>();
        /// <summary>
        /// The items that are currently in use.
        /// </summary>
        private readonly List<ItemPair> itemsActive = new List<ItemPair>();
        /// <summary>
        /// The items visible on the screen.
        /// </summary>
        private readonly List<ItemPair> itemsVisible = new List<ItemPair>();
        /// <summary>
        /// The size of the area of the items.
        /// </summary>
        private SizeF itemsAreaSize = SizeF.Empty;
        /// <summary>
        /// The index of the inventory items.
        /// </summary>
        private int itemsIndex = 0;

        /// <summary>
        /// The Text on top of the Weapons.
        /// </summary>
        private readonly ScaledText weaponText = new ScaledText(PointF.Empty, "Arms", 0.225f);
        /// <summary>
        /// The corners of all of the weapon slots.
        /// </summary>
        private readonly List<CornerSet> weaponCorners = new List<CornerSet>();
        /// <summary>
        /// The known Weapon Images.
        /// </summary>
        private readonly Dictionary<WeaponHash, ScaledTexture> weaponImages = new Dictionary<WeaponHash, ScaledTexture>();
        /// <summary>
        /// The 6 (or less) Weapons shown on the Inventory.
        /// </summary>
        private readonly Dictionary<WeaponHash, ScaledTexture> weaponVisible = new Dictionary<WeaponHash, ScaledTexture>();
        /// <summary>
        /// The area of the Weapons inside of the inventory.
        /// </summary>
        private SizeF weaponAreaSize = SizeF.Empty;
        /// <summary>
        /// The current Index of the Weapons.
        /// </summary>
        private int weaponIndex = 0;

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
            foreach (WeaponHash hash in weapons)
            {
                switch (Tools.GetWeaponType(hash))
                {
                    case WeaponType.Primary:
                    case WeaponType.Secondary:
                        itemsAmmo.Add(hash, new Magazines(hash));
                        break;
                }
            }

            for (int i = 0; i < 6; i++)
            {
                weaponCorners.Add(new CornerSet());
            }

            for (int i = 0; i < (6 * 3); i++)
            {
                itemsCorners.Add(new CornerSet());
            }

            Recalculate();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the menu.
        /// </summary>
        public void Recalculate()
        {
            // Get the current width of the screen for the calculations
            // The height is always 1080
            float width = 1f.ToXAbsolute();

            // And set the positions
            background.Size = new SizeF(575, 710);
            background.Position = new PointF((width * 0.5f) - (background.Size.Width * 0.5f), (1080f * 0.5f) - (background.Size.Height * 0.5f));

            top.Size = new SizeF(background.Size.Width, 155);
            top.Position = background.Position;

            playerBackground.Size = new SizeF(255, 60);
            playerBackground.Position = new PointF(background.Position.X - 23, background.Position.Y + 30);
            playerName.Position = new PointF(playerBackground.Position.X + 92, playerBackground.Position.Y + 7);

            playerGender.Size = new SizeF(50, 50);
            playerGender.Position = new PointF(playerBackground.Position.X + 40, playerBackground.Position.Y + 5);

            float statusY = background.Position.Y + 173;
            statusText.Position = new PointF(background.Position.X + 197, statusY);
            statusCurrent.Position = new PointF(background.Position.X + 337, statusY);
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

            itemsText.Position = new PointF(background.Position.X + itemXFromCorner, baseY - 23);
            for (int iy = 0; iy < 6; iy++)
            {
                float itemBaseY = baseY + (iy * genericHeight) + (iy * genericHeightSeparation);

                for (int ix = 0; ix < 3; ix++)
                {
                    int i = (iy * 3) + ix;
                    float itemBaseX = background.Position.X + itemXFromCorner + ((itemWidth + 4) * ix);

                    // Top
                    itemsCorners[i].Top.Size = new SizeF(itemWidth, healthCorner * 2);
                    itemsCorners[i].Top.Position = new PointF(itemBaseX, itemBaseY);
                    // Bottom
                    itemsCorners[i].Bottom.Size = new SizeF(itemWidth, healthCorner);
                    itemsCorners[i].Bottom.Position = new PointF(itemBaseX, itemBaseY + genericHeight);
                    // Left
                    itemsCorners[i].Left.Size = new SizeF(healthCorner, genericHeight);
                    itemsCorners[i].Left.Position = new PointF(itemBaseX, itemBaseY);
                    // Right
                    itemsCorners[i].Right.Size = new SizeF(healthCorner * 2, genericHeight);
                    itemsCorners[i].Right.Position = new PointF(itemBaseX + itemWidth - healthCorner, itemBaseY);
                    // Center
                    itemsCorners[i].Extra.Size = new SizeF(healthCorner * 2, genericHeight - (8 * 2));
                    itemsCorners[i].Extra.Position = new PointF(itemBaseX + (itemWidth * 0.5f) + (itemsCorners[i].Extra.Size.Width * 0.5f), itemBaseY + (genericHeight * 0.5f) - (itemsCorners[i].Extra.Size.Height * 0.5f));
                }
            }

            float weaponBaseX = background.Position.X + background.Size.Width - weaponWidth - 40;
            weaponText.Position = new PointF(weaponBaseX, baseY - 23);
            for (int i = 0; i < 6; i++)
            {
                float weaponBaseY = baseY + (i * genericHeight) + (i * genericHeightSeparation);

                // Top
                weaponCorners[i].Top.Size = new SizeF(weaponWidth, healthCorner * 2);
                weaponCorners[i].Top.Position = new PointF(weaponBaseX, weaponBaseY);
                // Bottom
                weaponCorners[i].Bottom.Size = new SizeF(weaponWidth, healthCorner);
                weaponCorners[i].Bottom.Position = new PointF(weaponBaseX, weaponBaseY + genericHeight);
                // Left
                weaponCorners[i].Left.Size = new SizeF(healthCorner, genericHeight);
                weaponCorners[i].Left.Position = new PointF(weaponBaseX, weaponBaseY);
                // Right
                weaponCorners[i].Right.Size = new SizeF(healthCorner, genericHeight);
                weaponCorners[i].Right.Position = new PointF(weaponBaseX + weaponWidth - healthCorner, weaponBaseY);
            }

            itemsAreaSize = new SizeF((itemWidth * 3) + (4 * 2), (genericHeight * 6) + (genericHeightSeparation * 5));
            weaponAreaSize = new SizeF(weaponWidth, (genericHeight * 6) +  (genericHeightSeparation * 5));

            UpdateItems();
            UpdateWeapons();
        }
        /// <summary>
        /// Processes the inventory.
        /// </summary>
        public void Process()
        {
            // If the last ped is not the same as the current one, update the player gender
            if (lastPlayerPed != Game.Player.Character)
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
                lastPlayerPed = Game.Player.Character;
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
            Game.DisableControlThisFrame(Control.NextWeapon);
            Game.DisableControlThisFrame(Control.PrevWeapon);
            Game.DisableControlThisFrame(Control.VehicleSelectNextWeapon);
            Game.DisableControlThisFrame(Control.VehicleSelectPrevWeapon);
            Game.DisableControlThisFrame(Control.VehicleFlySelectNextWeapon);
            Game.DisableControlThisFrame(Control.WeaponWheelNext);
            Game.DisableControlThisFrame(Control.WeaponWheelPrev);
            Game.DisableControlThisFrame(Control.SelectNextWeapon);
            Game.DisableControlThisFrame(Control.SelectPrevWeapon);
            Game.DisableControlThisFrame(Control.SelectWeapon);
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
            playerBackground.Draw();
            playerName.Draw();
            playerGender.Draw();
            statusText.Draw();
            statusCurrent.Draw();
            settingsIcon.Draw();
            healthText.Draw();
            healthCornerTop.Draw();
            healthCornerBottom.Draw();
            healthCornerLeft.Draw();
            healthCornerRight.Draw();
            healthBar.Draw();

            // Make sure that all of the required items are present
            bool itemUpdateRequired = false;
            foreach (ItemPair pair in itemsVisible)
            {
                if (pair.Item is StackableItem stackable && stackable.Count == 0)
                {
                    itemUpdateRequired = true;
                    break;
                }
            }
            foreach (KeyValuePair<WeaponHash, Magazines> ammo in itemsAmmo)
            {
                if (ammo.Value.Count > 0 && !itemsActive.Where(x => x.Item == ammo.Value).Any()) // TODO: Look for a better solution
                {
                    itemUpdateRequired = true;
                    break;
                }
            }
            if (itemUpdateRequired)
            {
                UpdateItems();
            }

            itemsText.Draw();
            foreach (CornerSet corners in itemsCorners)
            {
                corners.Top.Draw();
                corners.Bottom.Draw();
                corners.Left.Draw();
                corners.Right.Draw();
                corners.Extra.Draw();
            }
            foreach (ItemPair pair in itemsVisible)
            {
                pair.Draw();
            }

            // Corners of the weapon spaces
            weaponText.Draw();
            foreach (CornerSet corners in weaponCorners)
            {
                corners.Top.Draw();
                corners.Bottom.Draw();
                corners.Left.Draw();
                corners.Right.Draw();
            }
            // And the weapon images themselves
            foreach (KeyValuePair<WeaponHash, ScaledTexture> weapon in weaponVisible)
            {
                weapon.Value.Draw();
            }

            // Iterate over the visible weapons
            foreach (KeyValuePair<WeaponHash, ScaledTexture> weapon in weaponVisible)
            {
                // If the user clicked in the weapon
                if (Game.IsControlJustPressed(Control.CursorAccept) && Screen.IsCursorInArea(weapon.Value.Position, weapon.Value.Size))
                {
                    // If the Equip Weapons feature is enabled
                    if (GGO.menu.EquipWeapons.Checked)
                    {
                        // Get the type of it
                        WeaponType type = Tools.GetWeaponType(weapon.Key);

                        // If the weapon is primary or secondary, set it
                        WeaponHash previous;
                        if (type == WeaponType.Primary)
                        {
                            previous = GGO.weaponPrimary;
                            GGO.weaponPrimary = weapon.Key;
                        }
                        else if (type == WeaponType.Secondary)
                        {
                            previous = GGO.weaponSecondary;
                            GGO.weaponSecondary = weapon.Key;
                        }
                        // If the weapon is not primary or secondary, warn the player
                        else
                        {
                            Notification.Show("~r~Error~s~: The weapon is not Primary nor Secondary. Please check your GGO Settings and try again.");
                            return;
                        }

                        // If the player has the previous weapon equiped or is unarmed, set the weapon as active
                        if (Game.Player.Character.Weapons.Current.Hash == previous || previous == WeaponHash.Unarmed || previous == 0)
                        {
                            Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weapon.Key, true);
                        }
                    }
                    // If is not
                    else
                    {
                        // Just set the weapon as active
                        Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weapon.Key, true);
                    }
                    // Update the weapons shown on the inventory and on the screen and break the iterator
                    UpdateWeapons();
                    break;
                }
            }

            // If the player moved the mouse wheel up or down when the weapons are selected, move
            bool upPressed = Game.IsControlJustPressed(Control.PhoneScrollBackward);
            bool downPressed = Game.IsControlJustPressed(Control.PhoneScrollForward);
            bool isMouseOnItems = Screen.IsCursorInArea(itemsCorners[0].Top.Position, itemsAreaSize);
            bool isMouseOnWeapons = Screen.IsCursorInArea(weaponCorners[0].Top.Position, weaponAreaSize);
            if (downPressed && isMouseOnItems)
            {
                itemsIndex += 3;
                UpdateVisibleItems();
            }
            else if (upPressed && isMouseOnItems)
            {
                itemsIndex -= 3;
                UpdateVisibleItems();
            }
            if (downPressed && isMouseOnWeapons)
            {
                weaponIndex++;
                UpdateVisibleWeapons();
            }
            else if (upPressed && isMouseOnWeapons)
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
        /// Recreates the list of custom weapons.
        /// </summary>
        internal void UpdateItems()
        {
            // Clear the existing items
            itemsActive.Clear();

            // Add the ammo items for the weapons owned by the player if the player has the weapon and the number of mags is not zero
            foreach (KeyValuePair<WeaponHash, Magazines> ammo in itemsAmmo)
            {
                if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, ammo.Key, false)&& ammo.Value.Count > 0)
                {
                    itemsActive.Add(new ItemPair(ammo.Value));
                }
            }

            // Finally, update the items on the screen
            UpdateVisibleItems();
        }
        /// <summary>
        /// Updates the items visible on the screen.
        /// </summary>
        internal void UpdateVisibleItems()
        {
            // Fix the index if is out of bounds
            int maxIndex = (int)Math.Ceiling((itemsActive.Count - (3 * 6f)) / 3);
            if (itemsIndex < 0 || itemsActive.Count <= (3 * 6))
            {
                itemsIndex = 0;
            }
            else if (itemsIndex > maxIndex)
            {
                itemsIndex = maxIndex;
            }

            // Clear the existing items
            itemsVisible.Clear();

            // Iterate the number of items and update their position
            for (int i = 0; i < (3 * 6); i++)
            {
                // If the index is out of bounds, break
                if (itemsIndex + i >= itemsActive.Count)
                {
                    break;
                }

                // Otherwise, set the positions based on the corners
                ItemPair pair = itemsActive[itemsIndex + i];

                SizeF size = new SizeF(32.5f, 32.5f);
                pair.Item.Icon.Size = size;

                PointF pos = itemsCorners[i].Top.Position;
                pair.Item.Icon.Position = new PointF(pos.X + (itemWidth * 0.25f) - (size.Width * 0.5f), pos.Y + (genericHeight * 0.5f) - (size.Height * 0.5f));
                pair.Count.Position = new PointF(pos.X + (itemWidth * 0.75f), pos.Y + (genericHeight * 0.5f) - (pair.Count.LineHeight * 0.6f));

                itemsVisible.Add(pair);
            }
        }
        /// <summary>
        /// Updates the weapon on the list.
        /// </summary>
        internal void UpdateWeapons()
        {
            // Remove all of the existing weapon activators
            weaponImages.Clear();
            // Iterate the weapons and add the images for the weapons that the player has
            foreach (WeaponHash weaponHash in weapons)
            {
                // If the player has the weapon, is not currently using it and is not equpped (if the setting is enabled)
                if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, weaponHash, false) &&
                    Game.Player.Character.Weapons.Current.Hash != weaponHash &&
                    weaponHash != GGO.weaponPrimary && weaponHash != GGO.weaponSecondary)
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

                image.Value.Position = weaponCorners[i].Top.Position;
                image.Value.Size = new SizeF(weaponWidth, genericHeight);
                weaponImages[image.Key] = image.Value;

                weaponVisible.Add(image.Key, image.Value);
            }
        }

        #endregion
    }
}
