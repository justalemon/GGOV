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
        UIText CharacterNameHUD = new UIText(CharacterName, Coords.CalculatePoint(80.1f, 77.5f), 0.325f);
        CharacterNameHUD.Draw();

        UIText BulletsHUD = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Coords.CalculatePoint(91.75f, 77.6f), 0.475f);
        BulletsHUD.Draw();
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        UIRectangle CharacterBackground = new UIRectangle(Coords.CalculatePoint(80, 77), Coords.CalculateSize(5, 15), Color.Black);
        CharacterBackground.Draw();

        UIRectangle BulletsBackground = new UIRectangle(Coords.CalculatePoint(91.2f, 77), Coords.CalculateSize(5, 3.5f), Color.Black);
        BulletsBackground.Draw();
    }

    public static void ChangeOnTick(object Sender, EventArgs Event)
    {
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
