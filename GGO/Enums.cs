namespace GGO
{
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
        PlayerInfo = 3
    }
}
