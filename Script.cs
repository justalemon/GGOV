using GGOHud;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

public class GGOHudScript : Script
{
    public static ScriptSettings Config = ScriptSettings.Load("scripts\\GGOHud.ini");
    public static string CharacterName = Game.Player.Name;

    public GGOHudScript()
    {
        Tick += DrawTextOnTick;
        Tick += DrawShapesOnTick;
        Tick += ChangeOnTick;
        
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void DrawTextOnTick(object Sender, EventArgs Event)
    {
        UIText CharacterNameHUD = new UIText(CharacterName, Calculations.CalculatePoint(80.1f, 77.5f), 0.325f);
        CharacterNameHUD.Draw();
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        UIRectangle CharacterBackground = new UIRectangle(Calculations.CalculatePoint(80, 77), Calculations.CalculateSize(5, 15), Color.Black);
        CharacterBackground.Draw();
    }

    public static void ChangeOnTick(object Sender, EventArgs Event)
    {
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
