using GTA;
using System;

public class GGOHudScript : Script
{
    public static ScriptSettings Config = ScriptSettings.Load("script\\GGOHud.ini");

    public GGOHudScript()
    {
        Tick += DrawOnTick;
    }

    public static void DrawOnTick(object Sender, EventArgs Event)
    {

    }
}
