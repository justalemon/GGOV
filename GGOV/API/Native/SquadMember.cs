using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class SquadMember : IHealth
    {
        public bool Visible => true;

        public string Icon => InternalPed.IsAlive ? "Alive" : "Dead";

        public string Title
        {
            get
            {
                if (InternalPed.IsPlayer)
                {
                    return Game.Player.Name;
                }
                else if (GGO.Names.ContainsKey(InternalPed.Model.GetHashCode().ToString()))
                {
                    return GGO.Names[InternalPed.Model.GetHashCode().ToString()];
                }
                else
                {
                    return InternalPed.Model.Hash.ToString();
                }
            }
        }

        public float Current => Function.Call<int>(Hash.GET_ENTITY_HEALTH, InternalPed) - 100;

        public float Maximum => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, InternalPed) - 100;

        public Ped InternalPed { get; }

        public SquadMember(Ped BasePed)
        {
            InternalPed = BasePed;
        }
    }
}
