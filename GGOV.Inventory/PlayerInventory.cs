using GTA;
using LemonUI;
using PlayerCompanion;
using System;

namespace GGO
{
    /// <summary>
    /// The Inventory UI Item.
    /// </summary>
    public class PlayerInventory : IRecalculable, IProcessable
    {
        #region Properties

        /// <summary>
        /// If the inventory is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Constructor

        internal PlayerInventory()
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Recalculates the position of the menu.
        /// </summary>
        public void Recalculate()
        {
        }
        /// <summary>
        /// Processes the inventory.
        /// </summary>
        public void Process()
        {

        }

        #endregion
    }
}
