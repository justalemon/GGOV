using System;

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
    /// Abstract class used for showing all of the player data.
    /// </summary>
    public abstract class Field
    {
        #region Generic

        /// <summary>
        /// Gets the type of field to be shown on the player section.
        /// </summary>
        public virtual FieldType GetFieldType()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// If the information field should be shown during the next game tick.
        /// </summary>
        public virtual bool ShouldBeShown()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the name for the icon.
        /// </summary>
        public virtual string GetIconName()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the current value for the health bar or ammo count.
        /// </summary>
        public virtual float GetCurrentValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Health Only

        /// <summary>
        /// Gets the contents of the first text field.
        /// It only needs to be implemented on Health.
        /// </summary>
        public virtual string GetFirstText()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the contents of the first text field.
        /// If this does not returns empty or a whitespace, the Health bar gets removed.
        /// </summary>
        public virtual string GetSecondText()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the maximum value for health bar.
        /// It only needs to be implemented on Health.
        /// </summary>
        public virtual float GetMaxValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Weapon Only

        /// <summary>
        /// Gets the name for the image of the weapon. This (obviously) only needs to be implemented on Weapon.
        /// </summary>
        public virtual string GetWeaponImage()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// If the weapon data (ammo and the respective image) should be shown.
        /// </summary>
        public virtual bool DataShouldBeShown()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
