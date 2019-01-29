using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class PlayerVehicle : Field
    {
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

        public override bool IsDataAvailable()
        {
            return Game.Player.Character.CurrentVehicle != null;
        }
    }
}
