using GGO.Extensions;
using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class ItemWeapon : Item
    {
        private WeaponHash StoredHash;

        public ItemWeapon(WeaponHash Hash)
        {
            StoredHash = Hash;
            OnClick += OnClickGiveWeapon;
        }

        public override string GetQuantity()
        {
            if (Game.Player.Character.Weapons.HasWeapon(StoredHash))
            {
                return Game.Player.Character.Weapons[StoredHash].GetCorrectAmmo();
            }
            else
            {
                return "0";
            }
        }

        public override string GetIcon()
        {
            return Enum.GetName(typeof(WeaponHash), StoredHash);
        }

        public void OnClickGiveWeapon(object Sender, EventArgs Args)
        {
            Tools.SelectOrGive(StoredHash);
        }
    }
}
