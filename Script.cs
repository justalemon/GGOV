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
        // Player name - Example: LLENN
        Draw.Text(CharacterName, Position.PlayerName, 0.325f, false);

        // Draw dummies if the weapon should not be shown
        if (Weapons.CurrentType() == Weapons.Type.Banned)
        {
            Draw.Dummy(Position.PrimaryAmmoDummy);
            Draw.Dummy(Position.SecondaryAmmoDummy);
        }
        // Ammo for primary weapons - Example: 30 (for the Bullpup Rifle)
        else if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Position.PrimaryAmmo);
            Draw.Dummy(Position.SecondaryAmmoDummy);
        }
        // Ammo for sidearms - Example: 9 (for the .50 Pistol)
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Dummy(Position.PrimaryAmmoDummy);
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Position.SecondaryAmmo);
        }
    }

    public static void DrawImagesOnTick(object Sender, EventArgs Event)
    {
        // Store the locations of the Icons here
        string WeaponImage = Images.GetWeapon();
        string PlayerIcon = Images.GetIcon(Images.Icon.Player);
        string PrimaryIcon = Images.GetIcon(Images.Icon.Primary);
        string SecondaryIcon = Images.GetIcon(Images.Icon.Secondary);
        
        // Player Icon
        Draw.Texture(PlayerIcon, Position.PlayerIcon, Position.IconSize);

        // Weapon information
        // In order: Icon, Dummy, Weapon
        if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Texture(PrimaryIcon, Position.PrimaryIcon, Position.IconSize);
            Draw.Dummy(Position.SecondaryIconDummy);
            Draw.Texture(WeaponImage, Position.PrimaryImage, Position.WeaponSize, true);
        }
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Texture(SecondaryIcon, Position.SecondaryIcon, Position.IconSize);
            Draw.Dummy(Position.PrimaryIconDummy);
            Draw.Texture(WeaponImage, Position.SecondaryImage, Position.WeaponSize, true);
        }
        else
        {
            Draw.Dummy(Position.SecondaryIconDummy);
            Draw.Dummy(Position.PrimaryIconDummy);
        }
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        // Icons
        Draw.Rectangle(Position.PlayerIconBG, Position.SquaredBG, Colors.Background);
        Draw.Rectangle(Position.PrimaryIconBG, Position.SquaredBG, Colors.Background);
        Draw.Rectangle(Position.SecondaryIconBG, Position.SquaredBG, Colors.Background);
        
        // Player Info
        Draw.Rectangle(Position.PlayerInfoBG, Position.PlayerBGSize, Colors.Background);
        
        // Ammo
        Draw.Rectangle(Position.PrimaryAmmoBG, Position.SquaredBG, Colors.Background);
        Draw.Rectangle(Position.SecondaryAmmoBG, Position.SquaredBG, Colors.Background);
        // Weapons
        if (Weapons.CurrentType() == Weapons.Type.Main)
        {
            Draw.Rectangle(Position.PrimaryBG, Position.WeaponBG, Colors.Background);
        }
        else if (Weapons.CurrentType() == Weapons.Type.Sidearm)
        {
            Draw.Rectangle(Position.SecondaryBG, Position.WeaponBG, Colors.Background);
        }
    }

    public static void DrawHealthOnTick(object Sender, EventArgs Event)
    {
        // Calculate the bar size
        int Health = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character) - 100;
        int MaxHealth = Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Game.Player.Character) - 100;
        int HealthPercentage = Convert.ToInt32(((float)Health / MaxHealth) * 100f);
        float Size = (Position.HealthBarS.Width / 100f) * HealthPercentage;

        // Store the changes on a new object
        Size BarSize = Position.HealthBarS;
        BarSize.Width = Convert.ToInt32(Size);
        
        // First, draw the dividers so they are in the background
        Draw.Rectangle(Position.HealthDividerOne, Position.HealthBarDivider, Colors.Healthy);
        Draw.Rectangle(Position.HealthDividerTwo, Position.HealthBarDivider, Colors.Healthy);
        Draw.Rectangle(Position.HealthDividerThree, Position.HealthBarDivider, Colors.Healthy);
        Draw.Rectangle(Position.HealthDividerFour, Position.HealthBarDivider, Colors.Healthy);

        // Then, draw the bar by itself
        Draw.Rectangle(Position.HealthBar, BarSize, Colors.FromHealth(MaxHealth, HealthPercentage));
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
