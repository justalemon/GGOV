using GTA;
using LemonUI;
using LemonUI.Extensions;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// The set of fields used for the player information.
    /// </summary>
    public sealed class PlayerFields : IProcessable, IRecalculable
    {
        #region Properties

        /// <summary>
        /// If the player fields should be visible or not.
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// The health of the player.
        /// </summary>
        public PedHealth Health { get; } = new PedHealth(Game.Player.Character, true);

        #endregion

        #region Constructor

        internal PlayerFields()
        {
            Recalculate();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the player fields.
        /// </summary>
        public void Recalculate()
        {
            PointF position = new PointF(1f.ToXAbsolute() - 388, 848);
            Health.Recalculate(position);
        }
        /// <summary>
        /// Processes the player fields.
        /// </summary>
        public void Process()
        {
            Health.Process();
        }

        #endregion
    }
}
