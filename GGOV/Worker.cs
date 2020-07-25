using GTA;
using LemonUI;
using System;

namespace GGO
{
    /// <summary>
    /// The SHVDN Script that handles the menun.
    /// </summary>
    public class Worker : Script
    {
        #region Private Fields

        /// <summary>
        /// The object pool handling the Inventory and HUD.
        /// </summary>
        private ObjectPool pool = new ObjectPool();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GGO Worker class.
        /// </summary>
        public Worker()
        {
            // Add the set of controls to the pool
            pool.Add(Inventory.CurrentInstance);
            // And subscribe the events that we need
            Tick += Worker_Tick;
        }

        #endregion

        #region Local Events

        private void Worker_Tick(object sender, EventArgs e)
        {
            // Process the current instance of the 
            pool.Process();
        }

        #endregion
    }
}
