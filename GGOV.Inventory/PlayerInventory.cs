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

            // Draw the UI Elements
            background.Draw();
            top.Draw();
            playerColor.Draw();
            playerName.Draw();
            playerGender.Draw();
            textStatus.Draw();
            textStrage.Draw();
        }

        #endregion
    }
}
