using GTA;
using GTA.UI;
using LemonUI.Elements;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// The information of a player weapon.
    /// </summary>
    public abstract class Weapon : Field
    {
        #region Fields

        private int lastHash = 0;
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
        public virtual int AmmoCount => Game.Player.Character.Weapons.Current.AmmoInClip;
        /// <summary>
        /// The hash of the current weapon.
        /// </summary>
        public virtual int Hash => (int)Game.Player.Character.Weapons.Current.Hash;
        /// <summary>
        /// If the current weapon hash is valid for this type of weapon.
        /// </summary>
        public abstract bool IsWeaponValid { get; }

        #endregion

        #region Constructor

        internal Weapon()
        {
            Icon.Texture = "icon_weapon";
        }

        #endregion

        #region Functions

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
            weaponBackground.Size = new SizeF(230 - infoBackground.Size.Width - 5, 50);

            weapon.Position = new PointF(weaponBackground.Position.X + (weaponBackground.Size.Width * 0.5f) - (165 * 0.5f), weaponBackground.Position.Y);
            weapon.Size = new SizeF(165, 50);
        }
        /// <summary>
        /// Draws the current weapon being used by the player.
        /// </summary>
        public override void Process()
        {
            // If the last hash is not the same as the current one, update it
            if (lastHash != Hash)
            {
                lastHash = Hash;
                weapon.Texture = lastHash.ToString();
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
            if (IsWeaponValid)
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
