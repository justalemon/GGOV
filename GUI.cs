using GTA;
using System;
using System.Drawing;

namespace GGOHud
{
    class GUI
    {
        public static Size SizeFromConfig(string ConfigName)
        {
            float WidthOffset = GGOHudScript.Config.GetValue("Sizes", ConfigName + "W", 0f);
            float HeigthOffset = GGOHudScript.Config.GetValue("Sizes", ConfigName + "H", 0f);

            int Width = Convert.ToInt32((Game.ScreenResolution.Width / 100) * WidthOffset);
            int Heigth = Convert.ToInt32((Game.ScreenResolution.Height / 100) * HeigthOffset);

            return new Size(Width, Heigth);
        }

        public static Point PointFromConfig(string ConfigName)
        {
            return PointFromConfig(ConfigName + "X", ConfigName + "Y");
        }

        public static Point PointFromConfig(string XConfig, string YConfig)
        {
            float XOffset = GGOHudScript.Config.GetValue("Positions", XConfig, 0f);
            float YOffset = GGOHudScript.Config.GetValue("Positions", YConfig, 0f);

            int X = Convert.ToInt32((Game.ScreenResolution.Width / 100) * XOffset);
            int Y = Convert.ToInt32((Game.ScreenResolution.Height / 100) * YOffset);

            return new Point(X, Y);
        }
    }
}
