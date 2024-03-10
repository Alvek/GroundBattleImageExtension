# **DW2 notification filtering mod**

Mod will allow modders to add separate troop ground images.

# **Mod installation**

1.  Copy mod content to DW2\mods\ folder
2. Add parameter to game with one of the folowing methods:
  - In Steam library open DW2 properties and add launch parameters: --low-level-inject mods\GroundBattleImageExtension\GroundBattleImageExtension.dll!GroundBattleImageExtension.Preloader.Init
  - Create shortcut to exe and add parameters to Target like this (replace path to your DW2 folder): "D:\Games\Distant Worlds 2\DistantWorlds2.exe" --low-level-inject "d:\Games\Distant Worlds 2\mods\GroundBattleImageExtension\GroundBattleImageExtension.dll!GroundBattleImageExtension.Preloader.Init"
  - Create .bat file that runs the game, something like this (replace path to your DW2 folder):"D:\Games\Distant Worlds 2\DistantWorlds2.exe" --low-level-inject "d:\Games\Distant Worlds 2\mods\GroundBattleImageExtension\GroundBattleImageExtension.dll!GroundBattleImageExtension.Preloader.Init"
3.  Adjust GroundImages.xml to use your  images (TroopDefinitonId and GroundImageFileName)
4.  Run game using method you created in step 2 (run steam or use shortcut\bat)

# Issues

DW2 doesn't support code mods from workshop or mods folder fully, so you can't enabled them via Modifications menu in game (enabling\disabling does nothing for code mods) right now.
Only way to enable code mod is --low-level-inject command, you need to use it every time you want mod enabled during game.
