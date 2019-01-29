using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class PlayerWeapon : Field
    {
        public override float GetCurrentValue()
        {
            return Game.Player.Character.Weapons.Current.AmmoInClip;
        }

        public override FieldType GetFieldType()
        {
            return FieldType.Weapon;
        }

        public override string GetIconName()
        {
            return "Weapon";
        }

        public override string GetWeaponImage()
        {
            return Enum.GetName(typeof(WeaponHash), Game.Player.Character.Weapons.Current.Hash);
        }

        public override bool IsAvailable()
        {
            return true;
        }
    }
}
