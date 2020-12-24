using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI.Elements;
using System.Drawing;

namespace GGO.HUD
{
    /// <summary>
    /// The information of a player weapon.
    /// </summary>
    public abstract class Weapon : Field
    {
        #region Fields

        public bool firstTime = true;
        private bool shiftedImage = false;
        private WeaponHash lastHash = 0;
        private WeaponType lastType = 0;

        private readonly ScaledText noneIcon = new ScaledText(PointF.Empty, "-", 0.4f)
        {
            Alignment = Alignment.Center
        };
        private readonly ScaledText noneAmmo = new ScaledText(PointF.Empty, "-", 0.4f)
        {
            Alignment = Alignment.Center
        };
        internal ScaledRectangle weaponBackground = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(175, 0, 0, 0)
        };
        internal ScaledText ammo = new ScaledText(PointF.Empty, "")
        {
            Alignment = Alignment.Center
        };
        internal ScaledTexture weapon = new ScaledTexture("ggo_weapons", "");

        #endregion

        #region Properties

        /// <summary>
        /// The current ammo in the clip.
        /// </summary>
        public virtual int AmmoCount
        {
            get
            {
                // If the player has the Weapon equipped, get the current ammo in the clip
                if (Game.Player.Character.Weapons.Current.Hash == Hash)
                {
                    return Game.Player.Character.Weapons.Current.AmmoInClip;
                }
                // If he does not, return the ammo of the clip (because GTA auto reloads)
                else
                {
                    return Function.Call<int>(GTA.Native.Hash.GET_MAX_AMMO_IN_CLIP, Game.Player.Character, Hash, true);
                }
            }
        }
        /// <summary>
        /// The hash of the current weapon.
        /// </summary>
        public virtual WeaponHash Hash => Game.Player.Character.Weapons.Current.Hash;
        /// <summary>
        /// If the current weapon hash is valid for this type of weapon.
        /// </summary>
        public virtual bool IsWeaponValid => Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, Hash, false);

        #endregion

        #region Constructor

        internal Weapon()
        {
            Icon.Texture = "icon_weapon";
        }

        #endregion

        #region Functions

        /// <summary>
        /// Shifts the image slightly to the left if the weapon is Melee, or restores it to default.
        /// </summary>
        internal void ShiftImage(bool shift, bool force = false)
        {
            // If is the first time, the image has not been shifted to the correct position or is being forced
            if (firstTime || shiftedImage != shift || force)
            {
                // Set first time to false just in case
                firstTime = false;

                // Then, set the size of the background
                weaponBackground.Size = new SizeF(230 - infoBackground.Size.Width - 5 - (shift ? 45 : 0), 50);
                // And the position of the weapon itself
                weapon.Position = new PointF(weaponBackground.Position.X + (weaponBackground.Size.Width * 0.5f) - (165 * 0.5f), weaponBackground.Position.Y);
            }
        }
        /// <summary>
        /// Recalculates the position of the weapon information.
        /// </summary>
        /// <param name="position">The new position of the weapon information.</param>
        public override void Recalculate(PointF position)
        {
            base.Recalculate(position);

            noneIcon.Position = new PointF(position.X + (50 * 0.5f), position.Y + 6);
            noneAmmo.Position = new PointF(position.X + + 50 + 5 + (60 * 0.5f), position.Y + 6);

            infoBackground.Size = new SizeF(60, 50);

            ammo.Position = new PointF(infoBackground.Position.X + (infoBackground.Size.Width * 0.5f), infoBackground.Position.Y + 3);

            weaponBackground.Position = new PointF(infoBackground.Position.X + infoBackground.Size.Width + 5, position.Y);

            ShiftImage(Tools.GetWeaponType(lastHash) == WeaponType.Melee, true);
            weapon.Size = new SizeF(165, 50);
        }
        /// <summary>
        /// Draws the current weapon being used by the player.
        /// </summary>
        public override void Process()
        {
            // If the last hash is not the same as the current one, update the position of the weapon and texture
            WeaponHash currentHash = Hash;
            WeaponType currentType = Tools.GetWeaponType(currentHash);
            if (lastHash != currentHash && IsWeaponValid)
            {
                weapon.Texture = ((int)currentHash).ToString();

                switch (Tools.GetWeaponType(currentHash))
                {
                    case WeaponType.Primary:
                    case WeaponType.Secondary:
                        ShiftImage(false);
                        break;
                    case WeaponType.Melee:
                        ShiftImage(true);
                        break;
                }

                lastHash = currentHash;
                lastType = currentType;
            }

            // Update the current ammo count
            ammo.Text = AmmoCount.ToString();
            // And set the correct font size
            if (AmmoCount < 1000)
            {
                ammo.Scale = 0.55f;
            }
            else
            {
                ammo.Scale = 0.4f;
            }
            // And draw all of the elements
            base.Process();
            infoBackground.Draw();
            if (lastType == WeaponType.Primary || lastType == WeaponType.Secondary)
            {
                weaponBackground.Draw();
                Icon?.Draw();
                ammo.Draw();
                weapon.Draw();
            }
            else
            {
                noneIcon.Draw();
                noneAmmo.Draw();
            }
        }

        #endregion
    }
}
