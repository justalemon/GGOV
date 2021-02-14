using GTA;
using GTA.Native;
using LemonUI.Elements;
using PlayerCompanion;
using System;

namespace GGO.Inventory
{
    /// <summary>
    /// Item used to show the Ammo of specific weapons.
    /// </summary>
    internal class AmmoCount : StackableItem
    {
        #region Fields

        private readonly bool canBeEquipped = false;
        private readonly WeaponHash weapon = default;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the Ammo Type.
        /// </summary>
        public override string Name => "Placeholder";
        /// <summary>
        /// The icon for the Ammo Type.
        /// </summary>
        public override ScaledTexture Icon { get; } = new ScaledTexture("", "");
        /// <summary>
        /// The monetary value of the Ammo.
        /// </summary>
        public override int Value => 0;
        /// <summary>
        /// The number of Magazines based on the Weapon and Ammo count.
        /// </summary>
        public int MagCount
        {
            get
            {
                int magSize = Function.Call<int>(Hash.GET_MAX_AMMO_IN_CLIP, Game.Player.Character, weapon, true);
                float count = (float)TotalAmmo / magSize;
                return (int)Math.Floor(count);
            }
        }
        /// <summary>
        /// The Total Ammo of the player.
        /// </summary>
        public int TotalAmmo
        {
            get => Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, Game.Player.Character, weapon);
            set
            {
            }
        }
        /// <summary>
        /// The Maximum ammount of Ammo.
        /// </summary>
        public override int Maximum => int.MaxValue;

        #endregion

        #region Constructor

        internal AmmoCount(WeaponHash hash)
        {
            weapon = hash;
            switch (Tools.GetWeaponType(hash))
            {
                case WeaponType.Gear:
                    Icon.Dictionary = "ggo_gear";
                    canBeEquipped = true;
                    break;
                default:
                    Icon.Dictionary = "ggo_ammo";
                    break;
            }
            Icon.Texture = ((int)hash).ToString();

            Used += AmmoCount_Used;
        }

        #endregion

        #region Events

        private void AmmoCount_Used(object sender, EventArgs e)
        {
            if (canBeEquipped)
            {
                Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, weapon, false);
            }
        }

        #endregion
    }
}
