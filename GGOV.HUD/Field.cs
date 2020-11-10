using LemonUI;
using LemonUI.Elements;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// Represents a field shown on the top left or bottom right section of the screen.
    /// </summary>
    public abstract class Field : IProcessable
    {
        #region Fields

        internal ScaledRectangle iconBackground = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(175, 0, 0, 0)
        };
        internal ScaledRectangle infoBackground = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(175, 0, 0, 0)
        };

        #endregion

        #region Properties

        /// <summary>
        /// If the Field is visible on the screen or not.
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// The icon of the Field.
        /// </summary>
        public ScaledTexture Icon { get; set; } = new ScaledTexture("ggo", "icon_alive");

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the field.
        /// </summary>
        public virtual void Recalculate(PointF position)
        {
            iconBackground.Position = position;
            iconBackground.Size = new SizeF(50, 50);

            if (Icon != null)
            {
                Icon.Position = new PointF(position.X + 3, position.Y + 3);
                Icon.Size = new SizeF(44, 44);
            }

            infoBackground.Position = new PointF(position.X + 50 + 5, position.Y);
        }
        /// <summary>
        /// Processes the information of the Field.
        /// </summary>
        public virtual void Process()
        {
            iconBackground.Draw();
        }

        #endregion
    }
}
