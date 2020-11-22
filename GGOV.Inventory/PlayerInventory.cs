using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using System.Drawing;
using Font = GTA.UI.Font;

namespace GGO
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
        }

        #endregion
    }
}
