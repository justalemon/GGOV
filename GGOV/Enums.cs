namespace GGO
{
    /// <summary>
    /// The usage for the selected weapon.
    /// </summary>
    public enum Usage
    {
        Banned = -1,
        Main = 0,
        Sidearm = 1,
        Item = 2,
        Double = 3
    }

    /// <summary>
    /// Gender to be shown on the inventory.
    /// </summary>
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    /// <summary>
    /// The type of position that GetSpecificPosition should return.
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// Character icon for the squad members.
        /// </summary>
        SquadIcon = 0,
        /// <summary>
        /// Information for the squad members.
        /// </summary>
        SquadInfo = 1,
        /// <summary>
        /// Icon for player related information.
        /// </summary>
        PlayerIcon = 2,
        /// <summary>
        /// Information related to the player.
        /// </summary>
        PlayerInfo = 3,
        /// <summary>
        /// Ammo count for the current weapon.
        /// </summary>
        PlayerAmmo = 4,
        /// <summary>
        /// Weapon image.
        /// </summary>
        PlayerWeapon = 5
    }

    /// <summary>
    /// The section of the field.
    /// </summary>
    public enum FieldSection
    {
        Squad = 0,
        Player = 1
    }
}
