using System;

namespace GGO.API
{
    /// <summary>
    /// Class used as a base for all of the inventory items.
    /// </summary>
    public class Item
    {
        #region Events

        /// <summary>
        /// Event triggered when the user clicks the specific item.
        /// </summary>
        public event EventHandler OnClick;

        #endregion

        #region Functions

        /// <summary>
        /// If the item data is available and should be shown.
        /// </summary>
        public virtual bool IsAvailable()
        {
            return true;
        }

        /// <summary>
        /// Gets the quantity of the inventory item.
        /// </summary>
        public virtual string GetQuantity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the item icon.
        /// </summary>
        public virtual string GetIcon()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Triggers the OnClick event.
        /// </summary>
        public void Clicked()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
