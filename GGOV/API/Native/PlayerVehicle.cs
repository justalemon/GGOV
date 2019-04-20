using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class PlayerVehicle : IHealth
    {
        public bool Visible => Game.Player.Character.CurrentVehicle != null;

        public string Icon => "Vehicle";

        public string Title => Game.Player.Character.CurrentVehicle.FriendlyName;

        public float Current => Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character.CurrentVehicle);

        public float Maximum => 1000;
    }
}
