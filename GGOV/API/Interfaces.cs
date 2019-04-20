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

    /// <summary>
    /// Base Interface for all of the fields.
    /// </summary>
    public interface IField : IBase
    {
        
    }

    #endregion

    #region Items

    /// <summary>
    /// Interface used for inventory items.
    /// </summary>
    public interface IItem : IBase
    {
        /// <summary>
        /// Event triggered when the user clicks the specific item.
        /// </summary>
        event EventHandler OnClick;
        /// <summary>
        /// Quantity of the inventory item.
        /// </summary>
        string Quantity { get; }
    }

    #endregion
}
