using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class PlayerHealth : PlayerField
    {
        public override bool DataShouldBeShown()
        {
            throw new NotImplementedException();
        }

        public override float GetCurrentValue()
        {
            return Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Health;
        }

        public override string GetIconName()
        {
            return "Alive";
        }

        public override float GetMaxValue()
        {
            return Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
        }

        public override string GetName()
        {
            return Game.Player.Name;
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
