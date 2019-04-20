using GGO.Extensions;
using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class PlayerWeapon : IWeapon
    {
        public bool Visible => true;

        public string Icon => "Weapon";

        public int Ammo => Game.Player.Character.Weapons.Current.AmmoInClip;

        public string Image => Enum.GetName(typeof(WeaponHash), Game.Player.Character.Weapons.Current.Hash);

        public virtual bool Available => true;
    }

    public class PlayerPrimary : PlayerWeapon
    {
        public override bool Available => Game.Player.Character.Weapons.Current.GetStyle() == Usage.Main;
    }

    public class PlayerSecondary : PlayerWeapon
    {
        public override bool Available => Game.Player.Character.Weapons.Current.GetStyle() == Usage.Sidearm;
    }
}
