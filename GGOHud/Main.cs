using GTA;
using System;
using System.Globalization;
using System.Threading;

namespace GGOHud
{
    public class GGOHud : Script
    {
        /// <summary>
        /// Class to get our configuration values.
        /// </summary>
        public Configuration Config = new Configuration("scripts\\GGOHud.ini", "GGOHud");

        public GGOHud()
        {
            // Patch our locale so we don't have the "coma vs dot" problem
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Add our OnTick event
            Tick += OnTick;

            // Show some debug messages if the user wants to
            if (Config.Debug)
            {
                UI.Notify("~p~GGOHud~s~: Loading...");
                UI.Notify("~p~GGOHud~s~: Icon image size is " + Config.IconImage.Width.ToString() + "w, " + Config.IconImage.Height.ToString() + "h");
                UI.Notify("~p~GGOHud~s~: Icon background size is " + Config.IconBackground.Width.ToString() + "w, " + Config.IconBackground.Height.ToString() + "h");
                UI.Notify("~p~GGOHud~s~: Icon image diff is " + Config.IconRelative.Width.ToString() + "w, " + Config.IconRelative.Height.ToString() + "h");
            }
        }

        private void OnTick(object Sender, EventArgs Args)
        {

        }
    }
}
