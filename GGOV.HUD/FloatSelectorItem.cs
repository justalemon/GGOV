using GTA;
using LemonUI.Menus;

namespace GGO
{
    /// <summary>
    /// Item used to select a Float value.
    /// </summary>
    public class FloatSelectorItem : NativeDynamicItem<float>
    {
        #region Constructor

        public FloatSelectorItem(string title, string description) : base(title, description, 0)
        {
            ItemChanged += FloatSelectorItem_ItemChanged;
        }

        #endregion

        #region Events

        private void FloatSelectorItem_ItemChanged(object sender, ItemChangedEventArgs<float> e)
        {
            bool ten = Game.IsControlPressed(Control.FrontendX);
            bool dec = Game.IsControlPressed(Control.FrontendY);

            if (e.Direction == Direction.Left)
            {
                if (ten)
                {
                    e.Object -= 10;
                }
                else if (dec)
                {
                    e.Object -= 0.1f;
                }
                else
                {
                    e.Object -= 1;
                }
            }
            else if (e.Direction == Direction.Right)
            {
                if (ten)
                {
                    e.Object += 10;
                }
                else if (dec)
                {
                    e.Object += 0.1f;
                }
                else
                {
                    e.Object += 1;
                }
            }
        }

        #endregion
    }
}
