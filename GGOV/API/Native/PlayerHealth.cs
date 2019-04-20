using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class PlayerHealth : IHealth
    {
        public bool Visible => true;

        public string Icon => "Alive";

        public string Title => Game.Player.Name;

        public float Current => Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;

        public float Maximum => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
    }
}
