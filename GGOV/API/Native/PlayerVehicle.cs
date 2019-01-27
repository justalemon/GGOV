using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class PlayerVehicle : Field
    {
        public override bool DataShouldBeShown()
        {
            throw new NotImplementedException();
        }

        public override float GetCurrentValue()
        {
            return Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character.CurrentVehicle);
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Health;
        }

        public override string GetFirstText()
        {
            return Game.Player.Character.CurrentVehicle.FriendlyName;
        }

        public override string GetIconName()
        {
            return "Vehicle";
        }

        public override float GetMaxValue()
        {
            return 1000;
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
            return Game.Player.Character.CurrentVehicle != null;
        }
    }
}
