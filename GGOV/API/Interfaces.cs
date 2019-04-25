using System;

namespace GGO.API
{
    #region Base

    /// <summary>
    /// Base interface for the mod.
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// If the field or item should be shown during the next game tick.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Filename for the icon.
        /// </summary>
        string Icon { get; }
    }

    #endregion

    #region Fields

    /// <summary>
    /// Base interface for all of the fields.
    /// </summary>
    public interface IField : IBase
    {

    }

    /// <summary>
    /// Interface for fields that need a health bar.
    /// </summary>
    public interface IHealth : IField
    {
        /// <summary>
        /// Title of the field.
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Current health value.
        /// </summary>
        float Current { get; }
        /// <summary>
        /// Maximum health value.
        /// </summary>
        float Maximum { get; }
    }

    /// <summary>
    /// Interface for weapon fields that contains the ammo count and weapon image.
    /// </summary>
    public interface IWeapon : IField
    {
        /// <summary>
        /// Current ammo count.
        /// </summary>
        int Ammo { get; }
        /// <summary>
        /// The name for the weapon image.
        /// </summary>
        string Image { get; }
        /// <summary>
        /// If the weapon data (ammo and the respective image) should be shown.
        /// </summary>
        bool Available { get; }
    }

    /// <summary>
    /// Interface for text fields that only show a title and a piece of text.
    /// </summary>
    public interface IText : IField
    {
        /// <summary>
        /// Title of the field.
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Bottom text of the field.
        /// </summary>
        string Text { get; }
    }

    #endregion

    #region Items

    /// <summary>
    /// Class used for inventory items.
    /// </summary>
    public class Item : IBase
    {
        /// <summary>
        /// If the item should be visible on the inventory
        /// </summary>
        public virtual bool Visible { get; } = true;
        /// <summary>
        /// The name of the icon for the inventory.
        /// </summary>
        public virtual string Icon { get; } = "";
        /// <summary>
        /// Event triggered when the user clicks the specific item.
        /// </summary>
        public event EventHandler OnClick;
        /// <summary>
        /// Event triggered when the user right clicks the specific item.
        /// </summary>
        public event EventHandler OnRightClick;
        /// <summary>
        /// Event triggered when the user presses the mouse wheel the specific item.
        /// </summary>
        public event EventHandler OnMiddleClick;
        /// <summary>
        /// Quantity of the inventory item.
        /// </summary>
        public string Quantity { get; }
        /// <summary>
        /// Function called to simulate a click on the item.
        /// </summary>
        public void PerformClick()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Function called to simulate a click on the item.
        /// </summary>
        internal void PerformRightClick()
        {
            OnRightClick?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Function called to simulate a wheel click on the item.
        /// </summary>
        internal void PerformMiddleClick()
        {
            OnMiddleClick?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion
}
