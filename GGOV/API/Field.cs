using System.Drawing;

namespace GGO.API
{
    /// <summary>
    /// Type of data field that is going to be shown.
    /// </summary>
    public enum FieldType
    {
        Health = 0,
        Weapon = 1,
        Text = 2
    }

    /// <summary>
    /// Interface for showing player data.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Type of field to be shown on the player section.
        /// </summary>
        FieldType Type { get; }
        /// <summary>
        /// If the information field should be shown during the next game tick.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Name for the icon.
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// Gets the current value for the health bar or ammo count.
        /// </summary>
        float Value { get; }
        /// <summary>
        /// Contents of the first text field.
        /// It only needs to be implemented on Health.
        /// </summary>
        string FirstText { get; }
        /// <summary>
        /// Contents of the second text field.
        /// If this does not returns empty or a whitespace, the Health bar gets removed.
        /// </summary>
        string SecondText { get; }
        /// <summary>
        /// The maximum value for health bar.
        /// It only needs to be implemented on Health.
        /// </summary>
        float MaxValue { get; }
        /// <summary>
        /// Gets the colour for the health bar.
        /// </summary>
        Color Color { get; set; }
        /// <summary>
        /// Gets the name for the image of the weapon. This (obviously) only needs to be implemented on Weapon.
        /// </summary>
        string WeaponImage { get; }
        /// <summary>
        /// If the weapon data (ammo and the respective image) should be shown.
        /// </summary>
        bool WeaponAvailable { get; }
    }
}
