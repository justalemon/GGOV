using GGOHud;
using GTA;
using GTA.Native;
using System;
using System.IO;

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
        Tick += ChangeOnTick;
        
        // If the Character Name on the config has changed, use it
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void DrawTextOnTick(object Sender, EventArgs Event)
    {
        // First Row - Player name - Example: LLENN
        Draw.Text(CharacterName, Position.PlayerName, 0.325f, false);

        // Draw dummies if the weapon should not be shown
        if (Checks.IsCurrentWeaponBanned())
        {
            Draw.Dummy(Position.PrimaryAmmoDummy);
            Draw.Dummy(Position.SecondaryAmmoDummy);
        }
        // Second Row - Ammo for primary weapons - Example: 30 (for the Bullpup Rifle)
        else if (!Checks.IsCurrentWeaponSidearm())
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Position.PrimaryAmmo);
            Draw.Dummy(Position.SecondaryAmmoDummy);
        }
        // Third Row - Ammo for sidearms - Example: 9 (for the .50 Pistol)
        else
        {
            Draw.Dummy(Position.PrimaryAmmoDummy);
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Position.SecondaryAmmo);
        }
    }

    public static void DrawImagesOnTick(object Sender, EventArgs Event)
    {
        // Store the locations of the Icons here
        string WeaponImage = Image.GetWeapon();
        string PlayerIcon = Image.GetIcon(Image.Icon.Player);
        string PrimaryIcon = Image.GetIcon(Image.Icon.Primary);
        string SecondaryIcon = Image.GetIcon(Image.Icon.Secondary);

        // Set of Icons
        // Player Icon
        if (File.Exists(PlayerIcon))
        {
            Draw.Texture(PlayerIcon, Position.PlayerIcon, Position.IconSize);
        }

        // Primary Icon
        if (File.Exists(PrimaryIcon) && !Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Texture(PrimaryIcon, Position.PrimaryIcon, Position.IconSize);
        }
        else
        {
            Draw.Dummy(Position.PrimaryIconDummy);
        }

        // Secondary Weapon
        if (File.Exists(SecondaryIcon) && Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Texture(SecondaryIcon, Position.SecondaryIcon, Position.IconSize);
        }
        else
        {
            Draw.Dummy(Position.SecondaryIconDummy);
        }

        // Second Row
        // Picture for the primary weapon - Example: (see GUN_CarbineRifle.png for the Carbine)
        if (File.Exists(WeaponImage) && !Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Texture(WeaponImage, Position.PrimaryImage, Position.WeaponSize);
        }

        // Third Row
        // Picture for the sidearm - Example: (see GUN_APPistol.png for the AP Pistol)
        if (File.Exists(WeaponImage) && Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Texture(WeaponImage, Position.SecondaryImage, Position.WeaponSize);
        }
    }

    public static void DrawShapesOnTick(object Sender, EventArgs Event)
    {
        // Left Icons
        // Player Info
        Draw.Rectangle(Position.PlayerIconBG, Position.SquaredBG, Colors.Background);
        // Primary Weapon
        Draw.Rectangle(Position.PrimaryIconBG, Position.SquaredBG, Colors.Background);
        // Secondary Weapon
        Draw.Rectangle(Position.SecondaryIconBG, Position.SquaredBG, Colors.Background);

        // First Row
        // Player Info
        Draw.Rectangle(Position.PlayerInfoBG, Position.PlayerBGSize, Colors.Background);

        // Second Row
        // Primary Ammo
        Draw.Rectangle(Position.PrimaryAmmoBG, Position.SquaredBG, Colors.Background);
        // Primary Weapon
        if (!Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Rectangle(Position.PrimaryBG, Position.WeaponBG, Colors.Background);
        }

        // Third Row
        // Secondary Ammo
        Draw.Rectangle(Position.SecondaryAmmoBG, Position.SquaredBG, Colors.Background);
        // Secondary Weapon
        if (Checks.IsCurrentWeaponSidearm() && !Checks.IsCurrentWeaponBanned())
        {
            Draw.Rectangle(Position.SecondaryBG, Position.WeaponBG, Colors.Background);
        }
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
