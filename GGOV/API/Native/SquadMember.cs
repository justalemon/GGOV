using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class SquadMember : Field
    {
        public Ped InternalPed { get; }

        public SquadMember(Ped BasePed)
        {
            InternalPed = BasePed;
        }

        public override float GetCurrentValue()
        {
            return Function.Call<int>(Hash.GET_ENTITY_HEALTH, InternalPed) - 100;
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Health;
        }

        public override string GetFirstText()
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

        public override string GetIconName()
        {
            if (InternalPed.IsAlive)
            {
                return "Alive";
            }
            else
            {
                return "Dead";
            }
        }

        public override float GetMaxValue()
        {
            return Function.Call<int>(Hash.GET_PED_MAX_HEALTH, InternalPed) - 100;
        }
    }
}
