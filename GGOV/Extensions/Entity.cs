using GTA;
using GTA.Native;

namespace GGO.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// If the entity is part of the mission.
        /// </summary>
        /// <returns>True if the entity is part of the mission, False otherwise.</returns>
        public static bool IsMissionEntity(this Entity GameEntity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, GameEntity);
        }
    }
}
