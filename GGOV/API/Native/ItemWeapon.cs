using GGO.Extensions;
using GTA;
using GTA.Native;
using System;

namespace GGO.API.Native
{
    public class ItemWeapon : IItem
    {
        public bool Visible => true;
   
        public string Icon => Enum.GetName(typeof(WeaponHash), Hash);

        public event EventHandler OnClick;

        public string Quantity
        {
            get
            {
                if (Game.Player.Character.Weapons.HasWeapon(Hash))
                {
                    return Game.Player.Character.Weapons[Hash].GetCorrectAmmo();
                }
                else
                {
                    return "0";
                }
            }
        }

        private WeaponHash Hash;

        public ItemWeapon(WeaponHash hash)
        {
            Hash = hash;
            OnClick += OnClickGiveWeapon;
        }

        public void OnClickGiveWeapon(object sender, EventArgs args)
        {
            Tools.SelectOrGive(Hash);
        }
    }
}
