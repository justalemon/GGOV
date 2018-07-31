using GGOHud;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

public class GGOHudScript : Script
{
    // Settings
    public static ScriptSettings Config = ScriptSettings.Load("scripts\\GGOHud.ini");
    // Character Name on the Right Side
    public static string CharacterName = Game.Player.Name;

    public GGOHudScript()
    {
        // Register the events
        // TODO: See if having multiple events slow the script down
        Tick += DrawTextOnTick;
        Tick += DrawShapesOnTick;
        Tick += DrawImagesOnTick;
        Tick += DrawHealthOnTick;
        Tick += ChangeOnTick;
        
        // If the Character Name on the config has changed, use it
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void DrawTextOnTick(object Sender, EventArgs Event)
    {
        Point PrimaryDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"), GUI.SizeFromConfig("AmmoDummy"));
        Point SecondaryDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"), GUI.SizeFromConfig("AmmoDummy"));

        // Player name - Example: LLENN
        Draw.Text(CharacterName, GUI.PointFromConfig("PlayerName"), 0.325f, false);

        // Draw dummies if the weapon should not be shown
        if (Weapons.CurrentType() == Weapons.Type.Banned)
        {
            Draw.Dummy(PrimaryDummy);
            Draw.Dummy(SecondaryDummy);
        }
        // Ammo for primary weapons - Example: 30 (for the Bullpup Rifle)
        else if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"));
            Draw.Dummy(SecondaryDummy);
        }
        // Ammo for sidearms - Example: 9 (for the .50 Pistol)
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Dummy(PrimaryDummy);
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"));
        }
    }

    public static void DrawImagesOnTick(object Sender, EventArgs Event)
    {
        // Store the locations of the Icons here
        string WeaponImage = GUI.GetWeapon();
        string PlayerIcon = GUI.GetIcon(GUI.Icon.Player);
        string PrimaryIcon = GUI.GetIcon(GUI.Icon.Primary);
        string SecondaryIcon = GUI.GetIcon(GUI.Icon.Secondary);
        Point PrimaryDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconDummy"));
        Point SecondaryDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconDummy"));

        // Player Icon
        Draw.Texture(PlayerIcon, GUI.PointFromConfig("IconGenericX", "IconPlayerY"), GUI.SizeFromConfig("IconSize"));

        // Weapon information
        // In order: Icon, Dummy, Weapon
        if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Texture(PrimaryIcon, GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconSize"));
            Draw.Dummy(SecondaryDummy);
            Draw.Texture(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponPrimaryY"), GUI.SizeFromConfig("WeaponImage"), true);
        }
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Texture(SecondaryIcon, GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconSize"));
            Draw.Dummy(PrimaryDummy);
            Draw.Texture(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponSecondaryY"), GUI.SizeFromConfig("WeaponImage"), true);
        }
        else
        {
            Draw.Dummy(PrimaryDummy);
            Draw.Dummy(SecondaryDummy);
        }
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        // Icons
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPlayerY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        
        // Player Info
        Draw.Rectangle(GUI.PointFromConfig("PlayerBackground"), GUI.SizeFromConfig("PlayerBackground"), Colors.Background);
        
        // Ammo
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Weapons
        if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImagePrimaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImageSecondaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
    }

    public static void DrawHealthOnTick(object Sender, EventArgs Event)
    {
        // Calculate the bar size
        int Health = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
        int MaxHealth = Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Game.Player.Character) - 100;
        int HealthPercentage = Convert.ToInt32(((float)Health / MaxHealth) * 100f);
        float Size = (GUI.SizeFromConfig("HealthBar").Width / 100f) * HealthPercentage;

        // Store the changes on a new object
        Size BarSize = GUI.SizeFromConfig("HealthBar");
        BarSize.Width = Convert.ToInt32(Size);
        
        // First, draw the dividers so they are in the background
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerOneX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerTwoX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerThreeX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerFourX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);

        // Then, draw the bar by itself
        Draw.Rectangle(GUI.PointFromConfig("HealthBar"), BarSize, Colors.FromHealth(MaxHealth, HealthPercentage));
    }

    public static void ChangeOnTick(object Sender, EventArgs Event)
    {
        // Disable the radar if is requested
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
