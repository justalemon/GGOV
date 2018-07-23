using GGOHud;
using GTA;
using GTA.Native;
using System;
using System.IO;

public class GGOHudScript : Script
{
    public static ScriptSettings Config = ScriptSettings.Load("scripts\\GGOHud.ini");
    public static string CharacterName = Game.Player.Name;
    public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\GGOHud\\";

    public GGOHudScript()
    {
        Tick += DrawTextOnTick;
        Tick += DrawShapesOnTick;
        Tick += DrawImagesOnTick;
        Tick += ChangeOnTick;
        
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void DrawTextOnTick(object Sender, EventArgs Event)
    {
        // First Row
        // Player name
        // Example: LLENN
        UIText PlayerInfoName = new UIText(CharacterName, Coords.CalculatePoint(79.1f, 77.5f), 0.325f);
        PlayerInfoName.Draw();

        // Second Row
        // Ammo for primary weapons like Assault Rifles and Shotguns
        // Example: 30 (for the Bullpup Rifle)
        UIText PrimaryAmmo = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Coords.CalculatePoint(85, 77.6f), 0.475f);
        PrimaryAmmo.Draw();

        // Third Row
        // Ammo for secondary weapons like Pistols and Knifes
        // Example: 9 (for the .50 Pistol)
        UIText SecondaryAmmo = new UIText(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Coords.CalculatePoint(90.75f, 77.6f), 0.475f);
        SecondaryAmmo.Draw();
    }

    public static void DrawImagesOnTick(object Sender, EventArgs Event)
    {
        // Second Row
        // Picture for the primary weapon
        // Example: (see 0x3D2E1AE8.png for the Carabine)
        string WeaponImage = BaseDirectory + Game.Player.Character.Weapons.Current.Model.ToString() + ".png";
        if (File.Exists(WeaponImage))
        {
            UI.DrawTexture(WeaponImage, 1, 0, 200, Coords.CalculatePoint(83.75f, 81.5f), Coords.CalculateSize(6.75f, 9.50f));
        }
        else
        {
            UIText WeaponImageDummy = new UIText("-", Coords.CalculatePoint(85, 77.6f), 0.475f);
            WeaponImageDummy.Draw();
        }
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        // Left Icons
        // Player Info
        UIRectangle PlayerIconBG = new UIRectangle(Coords.CalculatePoint(79f, 73.2f), Coords.CalculateSize(5, 3.5f), Colors.Background);
        PlayerIconBG.Draw();
        // Primary Weapon
        UIRectangle PrimaryIconBG = new UIRectangle(Coords.CalculatePoint(84.6f, 73.2f), Coords.CalculateSize(5, 3.5f), Colors.Background);
        PrimaryIconBG.Draw();
        // Secondary Weapon
        UIRectangle SecondaryIconBG = new UIRectangle(Coords.CalculatePoint(90.2f, 73.2f), Coords.CalculateSize(5, 3.5f), Colors.Background);
        SecondaryIconBG.Draw();

        // First Row
        // Player Info
        UIRectangle PlayerInfoBG = new UIRectangle(Coords.CalculatePoint(79, 77), Coords.CalculateSize(5, 15), Colors.Background);
        PlayerInfoBG.Draw();

        // Second Row
        // Primary Ammo
        UIRectangle PrimaryAmmoBG = new UIRectangle(Coords.CalculatePoint(84.6f, 77), Coords.CalculateSize(5, 3.5f), Colors.Background);
        PrimaryAmmoBG.Draw();
        // Primary Weapon
        UIRectangle PrimaryGunBG = new UIRectangle(Coords.CalculatePoint(84.6f, 80.8f), Coords.CalculateSize(5, 11.1f), Colors.Background);
        PrimaryGunBG.Draw();

        // Third Row
        // Secondary Ammo
        UIRectangle SecondaryAmmoBG = new UIRectangle(Coords.CalculatePoint(90.2f, 77), Coords.CalculateSize(5, 3.5f), Colors.Background);
        SecondaryAmmoBG.Draw();
        // Secondary Weapon
        UIRectangle SecondaryGunBG = new UIRectangle(Coords.CalculatePoint(90.2f, 80.8f), Coords.CalculateSize(5, 11.1f), Colors.Background);
        SecondaryGunBG.Draw();
    }

    public static void ChangeOnTick(object Sender, EventArgs Event)
    {
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
