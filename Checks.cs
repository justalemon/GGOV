using GTA;

namespace GGOHud
{
    class Checks
    {
        public static bool IsCurrentWeaponBanned()
        {
            return Weapons.Hidden.Contains(Game.Player.Character.Weapons.Current.Hash);
        }

        public static bool IsCurrentWeaponSidearm()
        {
            return Weapons.Sidearms.Contains(Game.Player.Character.Weapons.Current.Hash);
        }
    }
}
