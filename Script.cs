using GGOHud;
using GGOHud.Properties;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class ScriptHUD : Script
{
    /// <summary>
    /// Load our Script Settings from the SHVDN folder.
    /// </summary>
    public static ScriptSettings Config = ScriptSettings.Load("scripts\\GGOHud.ini");
    /// <summary>
    /// Store the player name that shows on top of the health bar.
    /// </summary>
    public static string CharacterName = Game.Player.Name;
    /// <summary>
    /// Dummy location for the primary ammo.
    /// </summary>
    public static Point PrimaryAmmoDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"), GUI.SizeFromConfig("AmmoDummy"));
    /// <summary>
    /// Dummy location for the secondary ammo.
    /// </summary>
    public static Point SecondaryAmmoDummy = Point.Add(GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"), GUI.SizeFromConfig("AmmoDummy"));
    /// <summary>
    /// Dummy location for the primary icon.
    /// </summary>
    public static Point PrimaryIconDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconDummy"));
    /// <summary>
    /// Dummy location for the secondary icon.
    /// </summary>
    public static Point SecondaryIconDummy = Point.Add(GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconDummy"));
    public static Dictionary<string, string> Images = new Dictionary<string, string>
    {
        { "PlayerIcon", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "PrimaryIcon", Tools.ResourceToFile(Resources.ImageGun) },
        { "SecondaryIcon", Tools.ResourceToFile(Resources.ImageGun) },
        { "SquadIconOne", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIconTwo", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIconThree", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIconFour", Tools.ResourceToFile(Resources.ImagePlayer) },
    }; 

    public ScriptHUD()
    {
        // Register the event
        Tick += OnTick;
        
        // Change the player name if the user has changed it
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
    }

    public static void OnTick(object Sender, EventArgs Event)
    {
        // If the game is stil loading or is paused, do not draw the elements on screen
        if (Game.IsLoading || Game.IsPaused)
            return;

        // Temporary variables to check that everything is good
        bool PrimaryDummy = true;
        bool SecondaryDummy = true;
        bool DrawPrimary = false;
        bool DrawSecondary = false;

        // Store our current weapon to show
        string WeaponImage = GUI.GetWeapon();

        // Draw our player/character name
        Draw.Text(CharacterName, GUI.PointFromConfig("PlayerName"), 0.325f, false);
        // Draw the player icon
        Draw.Image(Images["PlayerIcon"], GUI.PointFromConfig("IconGenericX", "IconPlayerY"), GUI.SizeFromConfig("IconSize"), true);
        // Backgrounds
        // In order: Player Icon, Primary Icon, Secondary Icon, Player Info, Ammo Primary, Ammo Secondary
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPlayerY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("BackgroundGenericX", "BackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("PlayerBackground"), GUI.SizeFromConfig("PlayerBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundSecondaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        
        if (Weapons.CurrentWeaponType == Weapons.Type.Main)
        {
            PrimaryDummy = false;
            DrawPrimary = true;
        }
        else if (Weapons.CurrentWeaponType == Weapons.Type.Sidearm)
        {
            SecondaryDummy = false;
            DrawSecondary = true;
        }
        else if (Weapons.CurrentWeaponType == Weapons.Type.Double)
        {
            PrimaryDummy = false;
            SecondaryDummy = false;
            DrawPrimary = true;
            DrawSecondary = true;
        }

        if (PrimaryDummy)
        {
            Draw.Dummy(PrimaryAmmoDummy);
            Draw.Dummy(PrimaryIconDummy);
        }
        if (SecondaryDummy)
        {
            Draw.Dummy(SecondaryAmmoDummy);
            Draw.Dummy(SecondaryIconDummy);
        }
        if (DrawPrimary)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"));
            Draw.Image(Images["PrimaryIcon"], GUI.PointFromConfig("IconGenericX", "IconPrimaryY"), GUI.SizeFromConfig("IconSize"), true);
            Draw.Image(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponPrimaryY"), GUI.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImagePrimaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        if (DrawSecondary)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), GUI.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"));
            Draw.Image(Images["SecondaryIcon"], GUI.PointFromConfig("IconGenericX", "IconSecondaryY"), GUI.SizeFromConfig("IconSize"), true);
            Draw.Image(WeaponImage, GUI.PointFromConfig("WeaponGenericX", "WeaponSecondaryY"), GUI.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(GUI.PointFromConfig("WeaponImageGenericX", "WeaponImageSecondaryY"), GUI.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        
        // Draw the dividers so they are in the background
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerOneX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerTwoX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerThreeX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(GUI.PointFromConfig("HealthDividerFourX", "HealthDividerY"), GUI.SizeFromConfig("HealthDivider"), Colors.Divider);

        // To finish, let's draw the health bar
        Draw.HealthBar(GUI.PointFromConfig("HealthBar"), GUI.SizeFromConfig("HealthBar"), Game.Player.Character);

        // Disable the radar if the user want to
        if (Config.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }
    }
}
