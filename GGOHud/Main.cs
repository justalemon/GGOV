using GTA;

namespace GGOHud
{
    public class GGOHud : Script
    {
        private Configuration Config = new Configuration("scripts\\GGOHud.ini", "GGOHud");

        public GGOHud()
        {
            if (Config.Debug)
            {
                UI.Notify("GGOHud has been enabled.");
            }
        }
    }
}
