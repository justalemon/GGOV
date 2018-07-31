using GGOHud;
using GTA;
using GTA.Native;
using System;
using System.Drawing;

public class GGOHudScript : Script
{
    /// <summary>
    /// Load our Script Settings from the SHVDN folder.
    /// </summary>
    public static ScriptSettings Config = ScriptSettings.Load("scripts\\GGOHud.ini");
    /// <summary>
    /// Store the player name that shows on top of the health bar.
    /// </summary>
    public static string CharacterName = Game.Player.Name;

    public GGOHudScript()
    {
        // Register the event
        Tick += OnTick;
        
        // Change the player name if the user has changed it
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void OnTick(object Sender, EventArgs Event)
    {
        // Store our dummy location information here
        Point PrimaryAmmoDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"), GUI.SizeFromConfig("AmmoDummy"));
        Point SecondaryAmmoDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"), GUI.SizeFromConfig("AmmoDummy"));
        Point PrimaryIconDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconDummy"));
        Point SecondaryIconDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconDummy"));
        // As well our images
        string WeaponImage = GUI.GetWeapon();
        string PlayerIcon = GUI.GetIcon(GUI.Icon.Player);
        string PrimaryIcon = GUI.GetIcon(GUI.Icon.Primary);
        string SecondaryIcon = GUI.GetIcon(GUI.Icon.Secondary);

        // Draw our player/character name
        Draw.Text(CharacterName, GUI.PointFromConfig("PlayerName"), 0.325f, false);
        // Draw the player icon
        Draw.Texture(PlayerIcon, GUI.PointFromConfig("IconGenericX", "IconPlayerY"), GUI.SizeFromConfig("IconSize"));
        // Backgrounds
        // In order: Player Icon, Primary Icon, Secondary Icon, Player Info, Ammo Primary, Ammo Secondary
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPlayerY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("PlayerBackground"), GUI.SizeFromConfig("PlayerBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);

        // Draw dummies if the weapon is banned
        if (Weapons.CurrentType() == Weapons.Type.Banned)
        {
            Draw.Dummy(PrimaryAmmoDummy);
            Draw.Dummy(SecondaryAmmoDummy);
            Draw.Dummy(PrimaryIconDummy);
            Draw.Dummy(SecondaryIconDummy);
        }
        // Pimary weapon information
        else if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"));
            Draw.Dummy(SecondaryAmmoDummy);
            Draw.Texture(PrimaryIcon, GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconSize"));
            Draw.Dummy(SecondaryIconDummy);
            Draw.Texture(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponPrimaryY"), GUI.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImagePrimaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        // Secondary/Sidearm information
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Dummy(PrimaryAmmoDummy);
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"));
            Draw.Texture(SecondaryIcon, GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconSize"));
            Draw.Dummy(PrimaryIconDummy);
            Draw.Texture(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponSecondaryY"), GUI.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImageSecondaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }

        // Calculate the bar size
        int Health = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
        int MaxHealth = Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Game.Player.Character) - 100;
        int HealthPercentage = Convert.ToInt32(((float)Health / MaxHealth) * 100f);
        float Size = (GUI.SizeFromConfig("HealthBar").Width / 100f) * HealthPercentage;

        // Store the size of the bar in a new object
        Size BarSize = GUI.SizeFromConfig("HealthBar");
        BarSize.Width = Convert.ToInt32(Size);
        
        // First, draw the dividers so they are in the background
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerOneX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerTwoX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerThreeX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerFourX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Healthy);

        // Then, draw the bar by itself
        Draw.Rectangle(GUI.PointFromConfig("HealthBar"), BarSize, Colors.FromHealth(MaxHealth, HealthPercentage));

        // Disable the radar if the user want to
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
