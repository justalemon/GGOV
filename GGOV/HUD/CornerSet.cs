using LemonUI.Elements;
using System.Drawing;

namespace GGO.HUD
{
    /// <summary>
    /// A Set of Corners for the inventory Items and Weapons.
    /// </summary>
    public class CornerSet
    {
        #region Properties

        /// <summary>
        /// The Top Corner.
        /// </summary>
        public ScaledRectangle Top { get; } = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Bottom Corner.
        /// </summary>
        public ScaledRectangle Bottom { get; } = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Left Corner.
        /// </summary>
        public ScaledRectangle Left { get; } = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// The Right Corner.
        /// </summary>
        public ScaledRectangle Right { get; } = new ScaledRectangle(PointF.Empty, SizeF.Empty);
        /// <summary>
        /// An extra Corner or Section.
        /// </summary>
        public ScaledRectangle Extra { get; } = new ScaledRectangle(PointF.Empty, SizeF.Empty);

        #endregion
    }
}
