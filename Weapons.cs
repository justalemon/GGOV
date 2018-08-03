using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace GGOHud
{
    class Weapons
    {
        /// <summary>
        /// The type of the weapon that the player currently has.
        /// </summary>
        public enum Type
        {
            Banned = -1,
            Main = 0,
            Sidearm = 1,
            Melee = 2,
            Double = 3
        }
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static readonly List<int> Banned = new List<int> { 1595662460, 1548507267, -1609580060, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), Stun Gun (Type), SMG.
        /// </summary>
        public static readonly List<int> Sidearms = new List<int> { 416676503, 690389602, -957766203 };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type), Boxer (Type).
        /// </summary>
        public static readonly List<int> Melee = new List<int> { -1609580060, -728555052 };
        /// <summary>
        /// Animations used by jedijosh920's Dual Wield mod.
        /// Note: I got used to 'list = [["a", "b"], ["1", "2"]]' in Python, so...
        /// </summary>
        public static List<List<string>> Anims = new List<List<string>> {
            new List<string> { "holster", "unholster_2h" },
            new List<string> { "holster", "melee@holster" },
            new List<string> { "anim@veh@armordillo@turret@base", "sit_aim_down" }
        };

        /// <summary>
        /// Checks the weapon that the player currently has equiped.
        /// </summary>
        /// <returns>The Type of weapon.</returns>
        public static Type CurrentWeaponType
        {
            get
            {
                // Store the hash of the current weapon
                int WeaponType = Function.Call<int>(Hash.GET_WEAPONTYPE_GROUP, Game.Player.Character.Weapons.Current.Hash.GetHashCode());

                // Return the first match, in order
                // From dangerous to normal
                if (Banned.Contains(WeaponType))
                    return Type.Banned;
                else if (Melee.Contains(WeaponType))
                    return Type.Melee;
                else if (IsPlayerDualWielding)
                    return Type.Double;
                else if (Sidearms.Contains(WeaponType))
                    return Type.Sidearm;
                else
                    return Type.Main;
            }
        }

        /// <summary>
        /// Checks if the player is using 2 guns with "Dual Wield" by jedijosh920.
        /// </summary>
        /// <returns>true if the player is "Dual Wielding", false otherwise.</returns>
        public static bool IsPlayerDualWielding
        {
            get
            {
                // For each set of animations
                foreach (List<string> AnimSet in Anims)
                {
                    // See if the animation is playing
                    bool ItIs = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, AnimSet[0], AnimSet[1], 3);

                    // If it is, we know that the player is using dual weapons
                    if (ItIs)
                        return true;
                }
                // If none of the animations are playing, just return
                return false;
            }
            
        }
    }
}
