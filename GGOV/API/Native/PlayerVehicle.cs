using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class PlayerVehicle : PlayerField
    {
        public override bool DataShouldBeShown()
        {
            throw new NotImplementedException();
        }

        public override float GetCurrentValue()
        {
            return Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character.CurrentVehicle) - 100;
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Health;
        }

        public override string GetIconName()
        {
            return "Vehicle";
        }

        public override float GetMaxValue()
        {
            return 1000;
        }

        public override string GetName()
        {
            return Game.Player.Character.CurrentVehicle.FriendlyName;
        }

        public override string GetWeaponImage()
        {
            throw new NotImplementedException();
        }

        public override bool ShouldBeShown()
        {
            return Game.Player.Character.CurrentVehicle != null;
        }
    }
}
