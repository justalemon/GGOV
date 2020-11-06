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

        #region Properties

        /// <summary>
        /// The Squad members panel.
        /// </summary>
        public SquadMembers Squad { get; } = new SquadMembers();
        /// <summary>
        /// The fields with the information of the player.
        /// </summary>
        public PlayerFields Player { get; } = new PlayerFields();

        #endregion

        #region Constructor

        public HUD()
        {
            // Add the hud panels onto the pool
            pool.Add(Squad);
            pool.Add(Player);
            // And add the tick event
            Tick += HUD_Tick;
        }

        #endregion

        #region Events

        private void HUD_Tick(object sender, EventArgs e)
        {
            // Just process the HUD Elements
            pool.Process();
        }

        #endregion
    }
}
