using GTA;
using LemonUI.Elements;
using System.Drawing;

namespace GGO
{
    /// <summary>
    /// The information of a player weapon.
    /// </summary>
    public class Weapon : Field
    {
        #region Fields

        internal ScaledRectangle weaponBackground = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(175, 0, 0, 0)
        };
        internal ScaledText ammo = new ScaledText(PointF.Empty, "")
        {
            Alignment = GTA.UI.Alignment.Center
        };

        #endregion

        #region Properties

        /// <summary>
        /// The current ammo in the clip.
        /// </summary>
        public virtual int AmmoCount => Game.Player.Character.Weapons.Current.AmmoInClip;
        /// <summary>
        /// The hash of the current weapon.
        /// </summary>
        public virtual int WeaponHash => Game.Player.Character.Weapons.Current.Model.Hash;

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

            infoBackground.Size = new SizeF(60, 50);

            ammo.Position = new PointF(infoBackground.Position.X + (infoBackground.Size.Width * 0.5f), infoBackground.Position.Y + 3);

            weaponBackground.Position = new PointF(infoBackground.Position.X + infoBackground.Size.Width + 5, position.Y);
            weaponBackground.Size = new SizeF(230 - infoBackground.Size.Width - 5, 50);
        }
        /// <summary>
        /// Draws the current weapon being used by the player.
        /// </summary>
        public override void Process()
        {
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
            weaponBackground.Draw();
            ammo.Draw();
        }

        #endregion
    }
}
