using GGO.Extensions;
using GTA;

namespace GGO.API.Native
{
    public class PlayerMain : PlayerWeapon
    {
        public override bool DataShouldBeShown()
        {
            return Game.Player.Character.Weapons.Current.GetStyle() == Usage.Main;
        }
    }
}
