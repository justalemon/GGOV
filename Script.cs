using GGOHud;
using GGOHud.Tools;
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
    public static ScriptSettings ScriptConfig = ScriptSettings.Load("scripts\\GGOHud.ini");
    /// <summary>
    /// Dummy location for the primary ammo.
    /// </summary>
    public static Point PrimaryAmmoDummy = Point.Add(Config.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"), Config.SizeFromConfig("AmmoDummy"));
    /// <summary>
    /// Dummy location for the secondary ammo.
    /// </summary>
    public static Point SecondaryAmmoDummy = Point.Add(Config.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"), Config.SizeFromConfig("AmmoDummy"));
    /// <summary>
    /// Dummy location for the primary icon.
    /// </summary>
    public static Point PrimaryIconDummy = Point.Add(Config.PointFromConfig("IconGenericX", "IconPrimaryY"), Config.SizeFromConfig("IconDummy"));
    /// <summary>
    /// Dummy location for the secondary icon.
    /// </summary>
    public static Point SecondaryIconDummy = Point.Add(Config.PointFromConfig("IconGenericX", "IconSecondaryY"), Config.SizeFromConfig("IconDummy"));

    public ScriptHUD()
    {
        // Register the event
        Tick += OnTick;
        Aborted += OnAbort;
    }

    public static void OnAbort(object Sender, EventArgs Event)
    {
        // Delete our folder with images once the script is disabled or reloaded
        Directory.Delete(Path.Combine(Path.GetTempPath(), "GGOHud"), true);
    }

    public static void OnTick(object Sender, EventArgs Event)
    {
        // If the game is stil loading or is paused, do not draw the elements on screen
        if (Game.IsLoading || Game.IsPaused || !Game.Player.Character.IsAlive)
            return;

        // Temporary variables to check that everything is good
        bool PrimaryDummy = true;
        bool SecondaryDummy = true;
        bool DrawPrimary = false;
        bool DrawSecondary = false;

        // Draw our player/character name
        Draw.Text(Names.Player(), Config.PointFromConfig("PlayerName"), 0.325f, false);
        // Draw the player icon
        Draw.Image(Data.Icons["PlayerIcon"], Config.PointFromConfig("IconGenericX", "IconPlayerY"), Config.SizeFromConfig("IconSize"));
        // Backgrounds
        // Player icon
        Draw.Rectangle(Config.PointFromConfig("IconGenericX", "IconPlayerY") + Config.SizeFromConfig("IconBGOffset"), Config.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Primary icon
        Draw.Rectangle(Config.PointFromConfig("IconGenericX", "IconPrimaryY") + Config.SizeFromConfig("IconBGOffset"), Config.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Secondary icon
        Draw.Rectangle(Config.PointFromConfig("IconGenericX", "IconSecondaryY") + Config.SizeFromConfig("IconBGOffset"), Config.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Player information
        Draw.Rectangle(Config.PointFromConfig("PlayerBackground"), Config.SizeFromConfig("PlayerBackground"), Colors.Background);
        // Primary ammo
        Draw.Rectangle(Config.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundPrimaryY"), Config.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Secondary ammo
        Draw.Rectangle(Config.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundSecondaryY"), Config.SizeFromConfig("SquaredBackground"), Colors.Background);
        
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
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Config.PointFromConfig("AmmoGenericX", "AmmoPrimaryY"));
            Draw.Image(Data.Icons["PrimaryIcon"], Config.PointFromConfig("IconGenericX", "IconPrimaryY"), Config.SizeFromConfig("IconSize"), true);
            Draw.Image(Images.WeaponImage, Config.PointFromConfig("WeaponGenericX", "WeaponPrimaryY"), Config.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(Config.PointFromConfig("WeaponImageGenericX", "WeaponImagePrimaryY"), Config.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        if (DrawSecondary)
        {
            Draw.Text(Game.Player.Character.Weapons.Current.AmmoInClip.ToString(), Config.PointFromConfig("AmmoGenericX", "AmmoSecondaryY"));
            Draw.Image(Data.Icons["SecondaryIcon"], Config.PointFromConfig("IconGenericX", "IconSecondaryY"), Config.SizeFromConfig("IconSize"), true);
            Draw.Image(Images.WeaponImage, Config.PointFromConfig("WeaponGenericX", "WeaponSecondaryY"), Config.SizeFromConfig("WeaponImage"), true);
            Draw.Rectangle(Config.PointFromConfig("WeaponImageGenericX", "WeaponImageSecondaryY"), Config.SizeFromConfig("WeaponBackground"), Colors.Background);
        }
        
        // Draw the dividers so they are in the background
        Draw.Rectangle(Config.PointFromConfig("HealthDividerOneX", "HealthDividerY"), Config.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(Config.PointFromConfig("HealthDividerTwoX", "HealthDividerY"), Config.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(Config.PointFromConfig("HealthDividerThreeX", "HealthDividerY"), Config.SizeFromConfig("HealthDivider"), Colors.Divider);
        Draw.Rectangle(Config.PointFromConfig("HealthDividerFourX", "HealthDividerY"), Config.SizeFromConfig("HealthDivider"), Colors.Divider);

        // To finish, let's draw the health bar
        Draw.HealthBar(Config.PointFromConfig("HealthBar"), Config.SizeFromConfig("HealthBar"), Game.Player.Character);

        // Disable the radar if the user want to
        if (ScriptConfig.GetValue("GGOHud", "DisableRadarAndHUD", true))
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        }

        // Squad Information
        int Count = 1; // We start with one, the player
        List<Ped> FriendlyPeds = new List<Ped>
        {
            Game.Player.Character
        };

        // Add the peds on the list if they are part of the mision and the ped does not hate the player.
        // Also check that we are not adding the player again.
        foreach (Ped ThePed in World.GetNearbyPeds(Game.Player.Character.Position, 50f))
        {
            if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, ThePed) &&
                Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, ThePed, Game.Player.Character) != 5 &&
                !Function.Call<bool>(Hash.IS_PED_A_PLAYER, ThePed))
            {
                FriendlyPeds.Add(ThePed);
            }
        }

        foreach (Ped Friendly in FriendlyPeds)
        {
            if (Count > 5)
            {
                return;
            }
            
            Size Offset = Config.SizeFromConfig("SquadOffset");
            Size Square = Config.SizeFromConfig("SquaredBackground");
            Point GeneralPosition = Config.PointFromConfig("IconSquadX", "IconSquadFirstY") + Offset;
            Point InfoPosition = new Point(GeneralPosition.X + Square.Width + Offset.Width, GeneralPosition.Y * Count);

            Draw.Image(Data.Icons["SquadIcon" + Count.ToString()], new Point(GeneralPosition.X, GeneralPosition.Y * Count), Config.SizeFromConfig("IconSize"), true);
            Draw.Rectangle(new Point(GeneralPosition.X + Config.SizeFromConfig("IconBGOffset").Width, GeneralPosition.Y * Count), Square, Colors.Background);
            Draw.Rectangle(InfoPosition, Config.SizeFromConfig("SquadBackground"), Colors.Background);

            Draw.HealthBar(InfoPosition + Config.SizeFromConfig("SquadHealthPos"), Config.SizeFromConfig("SquadHealthBar"), Friendly);
            Draw.Text(Names.Ped(Friendly), InfoPosition + Config.SizeFromConfig("SquadNameOffset"), 0.3f, false);

            Count += 1;
        }
    }
}
