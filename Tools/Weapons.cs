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
                if (Data.BannedWeapons.Contains(WeaponType))
                    return Type.Banned;
                else if (Data.MeleeWeapons.Contains(WeaponType))
                    return Type.Melee;
                else if (IsPlayerDualWielding)
                    return Type.Double;
                else if (Data.SecondaryWeapons.Contains(WeaponType))
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
                foreach (List<string> AnimSet in Data.DualWieldAnims)
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
