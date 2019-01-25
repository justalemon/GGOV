# GGOV

[![5mods](https://img.shields.io/badge/5mods-download-20BA4E.svg)](https://www.gta5-mods.com/scripts/ggo)
[![AppVeyor](https://img.shields.io/appveyor/ci/justalemon/ggov.svg?label=appveyor)](https://ci.appveyor.com/project/justalemon/ggov)
[![CodeFactor](https://www.codefactor.io/repository/github/justalemon/ggov/badge)](https://www.codefactor.io/repository/github/justalemon/ggov)
[![Discord](https://img.shields.io/badge/discord-join-7289DA.svg)](https://discord.gg/Cf6sspj)

GGOV is a mod for Grand Theft Auto V that aims to bring the Gun Gale Online experience into GTA V. It started as just the HUD but now we have the inventory fully working with the "laser if finger on trigger" comming soon.

![Preview](https://raw.githubusercontent.com/justalemon/GGOV/master/preview.png)

## Prerequisites

* GTA V 1.0.1493.0 (After Hours) or higher
* [ScriptHookV](http://www.dev-c.com/gtav/scripthookv/) 1.0.1493.0 or higher
* [ScriptHookVDotNet](https://github.com/crosire/scripthookvdotnet/releases) 2.10.7 or higher

## Install

* Drop all of the compressed files into your `scripts` folder.

## Configuration

For editing the configuration files, we recommend [Visual Studio Code](https://code.visualstudio.com).

### Enabling and Disabling the Hud or Inventory

* Open `GGO/Inventory.json` or `GGO/Hud.json`
* Change `enabled` to `false`

### Inventory Items and Weapons

Please note that the maximum of Weapons is 5 and for items is 13 (15 is possible, but the last 2 get hiden when a weapon is equiped).

* Get the hash of your desired Item or Weapons from [here](https://www.justalemon.ml/gtav/weapons/)
* Open `GGO/Inventory.json` with a text editor
* You are going to see two lists: `weapons` and `items`, add the new item or replace an existing one

## Changelog

### 0.1

* Initial Release

### 0.2

* NEW: Added support for [jedijosh920's Dual Wield](https://www.gta5-mods.com/scripts/dual-wield)
* NEW: Added squad information for characters during missions (thanks SirBackenbart, ikt and Dot. for the help!)
* FIX: The images now are hidden in the pause menu and during the game load
* FIX: Secondary weapon is now showing up correctly
* FIX: The icons are now loaded from the Assembly, allowing the same image to be drawn more than once
* FIX: The Health Bar does not go into negative during the "Wasted" screen is open (thanks SirBackenbart for the report!)

### 1.0

* NEW: Code and Configuration has been refactored (in other words, rewriten)
* NEW: Removed support for jedijosh920's Dual Wield

### 1.0.1

* FIX: The script no longer crashes while looking at a dead ped
* FIX: Some enemy peds no longer show up as friendly

### 1.1

* FIX: Added some speed tweaks, GGOV should no longer cut the framerate on high end systems
* FIX: The image of the icons is now correctly centered
* FIX: Now the processing of images should not slow down the game during intense fights
* NEW: Added configuration option to disable the dead markers
* NEW: Added debug logging (for development reasons)
* NEW: Added vehicle information over the player information, there is an option to disable it if you want the vanilla Gun Gale Online environment

### 1.2

* FIX: The friendly check is now done two-way, removing the "enemies show as friendly" problems
* NEW: Added configuration option to disable the squad members
* NEW: Added configuration option to disable the radar

### 1.2.1

* NEW: The mod now uses a single DLL file (without counting the JSON parser)
* NEW: Configuration file renamed from GGO.Shared.json to GGO.Hud.json
* FIX: The weapon image should be now correctly centered

### 2.0

* NEW: The Configuration system has been reworked, THE PREVIOUS CONFIGURATION FILES ARE NO LONGER COMPATIBLE!
* NEW: Added inventory system (used inventory from the Psycho LLENN episode as base)
* NEW: Updated Newtonsoft.Json from 11.0.2 to 12.0.1
* NEW: Added weapon images for The Arena Wars update
* NEW: The weapons images now use the Enum value instead of the internal name
* FIX: You no longer get a shit ton of peds inside of the nightclub (requires After Hours SP by jedijosh920)
* FIX: Now you don't get FPS drops because of the ped checks
* FIX: The image drawing time has been reduced
* FIX: We no longer copy the UI images while the script is running

### 2.1

* NEW: Added more default items to fill the empty spaces
* NEW: You can now hide your current weapon/change to your fists by clicking the current weapon
* NEW: Added option to automatically give the items and weapons when the script starts
* NEW: Added option to remove the weapons and items that are not part of the inventory
* FIX: The Flare Gun no longer shows ammo and magazines left
* FIX: We now show infinite ammo for melee weapons
