using GTA;
using System;
using System.Drawing;

namespace GGOHud.Tools
{
    class Config
    {
        /// <summary>
        /// Creates a Size from a set of config parameters.
        /// </summary>
        /// <param name="ConfigName">The name of the config parameters. Both of them will get appended with W & H.</param>
        /// <returns>The Size based on the current game resolution.</returns>
        public static Size SizeFromConfig(string ConfigName)
        {
            float WidthOffset = ScriptHUD.ScriptConfig.GetValue("Sizes", ConfigName + "W", 0f);
            float HeigthOffset = ScriptHUD.ScriptConfig.GetValue("Sizes", ConfigName + "H", 0f);

            int Width = Convert.ToInt32((Game.ScreenResolution.Width / 100) * WidthOffset);
            int Heigth = Convert.ToInt32((Game.ScreenResolution.Height / 100) * HeigthOffset);

            return new Size(Width, Heigth);
        }

        /// <summary>
        /// Creates a Point from a set of config parameters.
        /// </summary>
        /// <param name="ConfigName">The name of the config parameters. Both of them will get appended with X & Y.</param>
        /// <returns>The Position on screen based on the current game resolution.</returns>
        public static Point PointFromConfig(string ConfigName)
        {
            return PointFromConfig(ConfigName + "X", ConfigName + "Y");
        }

        /// <summary>
        /// Creates a Point from two parameters.
        /// </summary>
        /// <param name="XConfig">The X parameter.</param>
        /// <param name="YConfig">The Y parameter.</param>
        /// <returns>The Position on screen based on the current game resolution.</returns>
        public static Point PointFromConfig(string XConfig, string YConfig)
        {
            float XOffset = ScriptHUD.ScriptConfig.GetValue("Positions", XConfig, 0f);
            float YOffset = ScriptHUD.ScriptConfig.GetValue("Positions", YConfig, 0f);

            int X = Convert.ToInt32((Game.ScreenResolution.Width / 100) * XOffset);
            int Y = Convert.ToInt32((Game.ScreenResolution.Height / 100) * YOffset);

            return new Point(X, Y);
        }
    }
}
