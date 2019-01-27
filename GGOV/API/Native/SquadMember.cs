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

        public override bool DataShouldBeShown()
        {
            throw new NotImplementedException();
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
            else if (Hud.Names.ContainsKey(InternalPed.Model.GetHashCode().ToString()))
            {
                return Hud.Names[InternalPed.Model.GetHashCode().ToString()];
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

        public override string GetSecondText()
        {
            return string.Empty;
        }

        public override string GetWeaponImage()
        {
            throw new NotImplementedException();
        }

        public override bool ShouldBeShown()
        {
            return true;
        }
    }
}
