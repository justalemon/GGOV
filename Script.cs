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
        { "SquadIcon1", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIcon2", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIcon3", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIcon4", Tools.ResourceToFile(Resources.ImagePlayer) },
        { "SquadIcon5", Tools.ResourceToFile(Resources.ImagePlayer) }
    };
    public static Dictionary<int, string> Names = new Dictionary<int, string>
    {

    };

    public ScriptHUD()
    {
        // Register the event
        Tick += OnTick;
        Aborted += OnAbort;
        
        // Change the player name if the user has changed it
        if (Config.GetValue("GGOHud", "CharacterName", "default") != "default")
            CharacterName = Config.GetValue("GGOHud", "CharacterName", "default");
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

        // Store our current weapon to show
        string WeaponImage = GUI.GetWeapon();

        // Draw our player/character name
        Draw.Text(CharacterName, GUI.PointFromConfig("PlayerName"), 0.325f, false);
        // Draw the player icon
        Draw.Image(Images["PlayerIcon"], GUI.PointFromConfig("IconGenericX", "IconPlayerY"), GUI.SizeFromConfig("IconSize"));
        // Backgrounds
        // Player icon
        Draw.Rectangle(GUI.PointFromConfig("IconGenericX", "IconPlayerY") + GUI.SizeFromConfig("IconBGOffset"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Primary icon
        Draw.Rectangle(GUI.PointFromConfig("IconGenericX", "IconPrimaryY") + GUI.SizeFromConfig("IconBGOffset"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Secondary icon
        Draw.Rectangle(GUI.PointFromConfig("IconGenericX", "IconSecondaryY") + GUI.SizeFromConfig("IconBGOffset"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Player information
        Draw.Rectangle(GUI.PointFromConfig("PlayerBackground"), GUI.SizeFromConfig("PlayerBackground"), Colors.Background);
        // Primary ammo
        Draw.Rectangle(GUI.PointFromConfig("AmmoBackgroundX", "AmmoBackgroundPrimaryY"), GUI.SizeFromConfig("SquaredBackground"), Colors.Background);
        // Secondary ammo
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

            string Name;

            if (Friendly.IsPlayer)
            {
                Name = CharacterName;
            }
            else if (Names.ContainsKey(Friendly.Model.Hash))
            {
                Name = Names[Friendly.Model.Hash];
            }
            else
            {
                Name = Friendly.Model.Hash.ToString();
            }
            
            Size Offset = GUI.SizeFromConfig("SquadOffset");
            Size Square = GUI.SizeFromConfig("SquaredBackground");
            Point GeneralPosition = GUI.PointFromConfig("IconSquadX", "IconSquadFirstY") + Offset;
            Point InfoPosition = new Point(GeneralPosition.X + Square.Width + Offset.Width, GeneralPosition.Y * Count);

            Draw.Image(Images["SquadIcon" + Count.ToString()], new Point(GeneralPosition.X, GeneralPosition.Y * Count), GUI.SizeFromConfig("IconSize"), true);
            Draw.Rectangle(new Point(GeneralPosition.X + GUI.SizeFromConfig("IconBGOffset").Width, GeneralPosition.Y * Count), Square, Colors.Background);
            Draw.Rectangle(InfoPosition, GUI.SizeFromConfig("SquadBackground"), Colors.Background);

            Draw.HealthBar(InfoPosition + GUI.SizeFromConfig("SquadHealthPos"), GUI.SizeFromConfig("SquadHealthBar"), Friendly);
            Draw.Text(Name, InfoPosition + GUI.SizeFromConfig("SquadNameOffset"), 0.3f, false);

            Count += 1;
        }
    }
}
