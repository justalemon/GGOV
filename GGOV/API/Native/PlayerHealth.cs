using GTA;
using GTA.Native;

namespace GGO.API.Native
{
    public class PlayerHealth : Field
    {
        public override float GetCurrentValue()
        {
            return Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Health;
        }

        public override string GetFirstText()
        {
            return Game.Player.Name;
        }

        public override string GetIconName()
        {
            return "Alive";
        }

        public override float GetMaxValue()
        {
            return Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Game.Player.Character) - 100;
        }

        public override bool IsAvailable()
        {
            return true;
        }
    }
}
