using GTA;
using GTA.Native;
using System;

public class GGOHudScript : Script
{
    public static ScriptSettings Config = ScriptSettings.Load("script\\GGOHud.ini");

    public GGOHudScript()
    {
        Tick += DrawOnTick;

        Function.Call(Hash.DISPLAY_HUD, Config.GetValue("GGOHud", "DisableGameUI", true));
    }

    public static void DrawOnTick(object Sender, EventArgs Event)
    {

    }
}
