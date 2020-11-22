using GTA;
using LemonUI;
using System;

namespace GGO
{
    /// <summary>
    /// The Gun Gale Online Inventory.
    /// </summary>
    public class Inventory : Script
    {
        #region Fields

        private readonly ObjectPool pool = new ObjectPool();
        private readonly PlayerInventory inventory = new PlayerInventory();

        #endregion

        #region Constructors

        public Inventory()
        {
            pool.Add(inventory);
            Tick += Inventory_Tick;
        }

        #endregion

        #region Events

        private void Inventory_Tick(object sender, EventArgs e)
        {
            // If the user pressed the Inventory button
            Game.DisableControlThisFrame(Control.SelectWeapon);
            inventory.Visible = Game.IsControlPressed(Control.SelectWeapon);

            // Just process the menu pool
            pool.Process();
        }

        #endregion
    }
}
