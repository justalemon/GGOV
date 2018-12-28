using GTA;
using GTA.Native;
using System.Linq;

namespace GGO
{
    /// <summary>
    /// The style of the weapon that the player currently has.
    /// </summary>
    public enum WeaponStyle
    {
        Banned = -1,
        Main = 0,
        Sidearm = 1,
        Melee = 2,
        Double = 3
    }

    /// <summary>
    /// Extensions used to get more features from existing classes.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static uint[] BannedWeapons = new uint[] { 1595662460u, 1548507267u, 2685387236u, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), SMG.
        /// </summary>
        public static uint[] SecondaryWeapons = new uint[] { 416676503u, 3337201093u };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type).
        /// </summary>
        public static uint[] MeleeWeapons = new uint[] { 3566412244u };

        /// <summary>
        /// If the entity is part of the mission.
        /// </summary>
        /// <returns>True if the entity is part of the mission, False otherwise.</returns>
        public static bool IsMissionEntity(this Entity GameEntity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, GameEntity);
        }

        /// <summary>
        /// Checks if the specified relationship ID is for a friendly ped.
        /// </summary>
        /// <param name="Relationship">The relationship ID.</param>
        /// <returns>True if the ped is friendly, False otherwise.</returns>
        public static bool IsFriendly(this Ped GamePed)
        {
            return (int)Game.Player.Character.GetRelationshipWithPed(GamePed) <= 2 && (int)GamePed.GetRelationshipWithPed(Game.Player.Character) <= 2;
        }

        /// <summary>
        /// Checks for the weapon type.
        /// </summary>
        /// <param name="ID">The Weapon Type ID.</param>
        /// <returns>The style of weapon.</returns>
        public static WeaponStyle GetStyle(this WeaponCollection Collection)
        {
            // Return the first match, in order
            // From dangerous to normal
            if (BannedWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Banned;
            else if (MeleeWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Melee;
            else if (SecondaryWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Sidearm;
            else
                return WeaponStyle.Main;
        }
    }
}
