using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using System.Collections.Generic;
using System.Drawing;
using Font = GTA.UI.Font;

namespace GGO.Inventory
{
    /// <summary>
    /// The Inventory UI Item.
    /// </summary>
    public class PlayerInventory : IRecalculable, IProcessable
    {
        #region Fields

        private const float healthWidth = 258;
        private const float healthHeight = 23;
        private const float healthCorner = 1;

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

        private readonly ScaledText healthText = new ScaledText(PointF.Empty, "Life", 0.225f);
        private readonly ScaledRectangle healthBar = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerTop = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerBottom = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerLeft = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        private readonly ScaledRectangle healthCornerRight = new ScaledRectangle(PointF.Empty, SizeF.Empty);

        private readonly ScaledText weaponText = new ScaledText(PointF.Empty, "Arms", 0.225f);
        private readonly List<List<ScaledRectangle>> weaponCorners = new List<List<ScaledRectangle>>();

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

            const float inventorySpaceHeight = 50;
            float baseY = background.Position.Y + 252;

            const float itemWidth = 107;
            const float itemWidthFromX = 35;
            itemsText.Position = new PointF(background.Position.X + itemWidthFromX, baseY - 23);
            for (int iy = 0; iy < 6; iy++)
            {
                float itemBaseY = baseY + (iy * inventorySpaceHeight) + (iy * 14);

                for (int ix = 0; ix < 3; ix++)
                {
                    int i = (iy * 3) + ix;
                    float itemBaseX = background.Position.X + itemWidthFromX + ((itemWidth + 4) * ix);

                    // Top
                    itemsCorners[i][0].Size = new SizeF(itemWidth, healthCorner * 2);
                    itemsCorners[i][0].Position = new PointF(itemBaseX, itemBaseY);
                    // Bottom
                    itemsCorners[i][1].Size = new SizeF(itemWidth, healthCorner);
                    itemsCorners[i][1].Position = new PointF(itemBaseX, itemBaseY + inventorySpaceHeight);
                    // Left
                    itemsCorners[i][2].Size = new SizeF(healthCorner, inventorySpaceHeight);
                    itemsCorners[i][2].Position = new PointF(itemBaseX, itemBaseY);
                    // Right
                    itemsCorners[i][3].Size = new SizeF(healthCorner * 2, inventorySpaceHeight);
                    itemsCorners[i][3].Position = new PointF(itemBaseX + itemWidth - healthCorner, itemBaseY);
                    // Center
                    itemsCorners[i][4].Size = new SizeF(healthCorner * 2, inventorySpaceHeight - (8 * 2));
                    itemsCorners[i][4].Position = new PointF(itemBaseX + (itemWidth * 0.5f) + (itemsCorners[i][4].Size.Width * 0.5f), itemBaseY + (inventorySpaceHeight * 0.5f) - (itemsCorners[i][4].Size.Height * 0.5f));
                }
            }

            const float weaponWidth = 143;
            float weaponBaseX = background.Position.X + background.Size.Width - weaponWidth - 40;
            weaponText.Position = new PointF(weaponBaseX, baseY - 23);
            for (int i = 0; i < 6; i++)
            {
                float weaponBaseY = baseY + (i * inventorySpaceHeight) + (i * 14);

                // Top
                weaponCorners[i][0].Size = new SizeF(weaponWidth, healthCorner * 2);
                weaponCorners[i][0].Position = new PointF(weaponBaseX, weaponBaseY);
                // Bottom
                weaponCorners[i][1].Size = new SizeF(weaponWidth, healthCorner);
                weaponCorners[i][1].Position = new PointF(weaponBaseX, weaponBaseY + inventorySpaceHeight);
                // Left
                weaponCorners[i][2].Size = new SizeF(healthCorner, inventorySpaceHeight);
                weaponCorners[i][2].Position = new PointF(weaponBaseX, weaponBaseY);
                // Right
                weaponCorners[i][3].Size = new SizeF(healthCorner, inventorySpaceHeight);
                weaponCorners[i][3].Position = new PointF(weaponBaseX + weaponWidth - healthCorner, weaponBaseY);
            }
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

            // Draw the UI Elements
            background.Draw();
            top.Draw();
            playerColor.Draw();
            playerName.Draw();
            playerGender.Draw();
            textStatus.Draw();
            textStrage.Draw();
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

            weaponText.Draw();
            foreach (List<ScaledRectangle> corners in weaponCorners)
            {
                foreach (ScaledRectangle corner in corners)
                {
                    corner.Draw();
                }
            }
        }

        #endregion
    }
}
