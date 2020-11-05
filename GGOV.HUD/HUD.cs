using GTA;
using LemonUI;
using System;

namespace GGO
{
    /// <summary>
    /// Script that handles the HUD Elements.
    /// </summary>
    public class HUD : Script
    {
        #region Private Fields

        private readonly ObjectPool pool = new ObjectPool();

        #endregion

        #region Constructor

        public HUD()
        {
            Tick += HUD_Tick;
        }

        #endregion

        #region 

        private void HUD_Tick(object sender, EventArgs e)
        {
            // Just process the HUD Elements
            pool.Process();
        }

        #endregion
    }
}
