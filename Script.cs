using GTA;
using GTA.Native;
using System;

public class GGOHudScript : Script
{
    public static ScriptSettings Config = ScriptSettings.Load("script\\GGOHud.ini");

    public GGOHudScript()
    {
        Tick += DrawOnTick;
        Tick += ChangeOnTick;
    }

    public static void DrawOnTick(object Sender, EventArgs Event)
    {
        
    }

    public static void ChangeOnTick(object Sender, EventArgs Event)
    {
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
