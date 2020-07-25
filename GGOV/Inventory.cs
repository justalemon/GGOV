using LemonUI;

namespace GGO
{
    /// <summary>
    /// The Inventory of Gun Gale Online.
    /// </summary>
    public sealed class Inventory : IProcessable
    {
        #region Private Fields

        /// <summary>
        /// The current instance of the inventory.
        /// </summary>
        private static Inventory currentInstance = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current instance of the Inventory.
        /// </summary>
        public static Inventory CurrentInstance
        {
            get
            {
                if (currentInstance == null)
                {
                    currentInstance = new Inventory();
                }
                return currentInstance;
            }
        }
        /// <summary>
        /// If the inventory is currently visible or not.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        private Inventory()
        {
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Draws and Processes the controls of the inventory.
        /// </summary>
        public void Process()
        {
        }

        #endregion
    }
}
