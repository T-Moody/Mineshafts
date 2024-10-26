## Notes

Use the mod on a server to sync configs.  
In case of questions you may find me on the [OdinPlus](https://discord.gg/s7tUavejWR) or the [Valheim modding](https://discord.gg/MXqWrn532w) discord servers.  
You may also file an issue on [GitHub](https://github.com/GoldenJude/Mineshafts/issues).  

## Features

#### Underground mining
- place down a mineshaft entrance, grab your pickaxe and hack away at the cold stone walls to uncover the riches hiding deep in the earth  
- (upon entering a mine you may dig around using your pickaxe and eventually find ores and other materials)  
- building within mines is enabled  
- it is possible to link up separate mineshafts by digging towards them  
- the dug up area will stay behind even when destroying a mineshaft entrance, feel free to reposition entrances  
- placing an entrance inside a mine will link it up back to the surface  

#### Abandoned mineshafts
- explore the remains of past mining operations and fight the entrapped miners  
- abandoned mineshafts generate throughout the world and contain loot aswell as exposed ore veins  

## Configuration
Configuration for the mod is done within the ``GoldenJude_Mineshafts.cfg`` file created upon game launch.  
All configs reload during runtime.  

#### General configuration  
``stone_health`` = configures the damage required to break underground rock  
``min_pickaxe_tier`` = minimum pickaxe tier needed to begin mining  

#### Building piece recipe  
``items`` = array of items followed by their required amounts for this building piece  
``recover`` = defines if required items should be recovered upon destruction

#### Abandoned mineshaft configuration  
``quantity`` = amount of abandoned mineshafts to be generated throughout the world  
``min_spacing`` = minimum spacing between abandoned mineshafts (if an abandoned mineshaft is spawned another one cannot spawn within this distance of it)  
``rooms`` = amount of rooms to be generated in an abandoned mineshaft  

#### Drops
``biomes`` = array of possible bioms this drop may occur in  
possible biomes: Global, None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands  
``min_pickaxe_tier`` = minimum pickaxe tier required for this drop to occur  
``prefab`` = prefab spawned when this drop is rolled (can be any prefab from the game, creatures for example)  
``drop min``/``max`` = amount of drops  
to define a drop the header must start with ``drop_`` followed by anything, for example ``[drop_my_drop-epic_ore-mistlands-32]``  

## Screenshots  

![pic1](https://cdn.discordapp.com/attachments/818400376255545395/974630185217441822/unknown.png)  
![pic2](https://cdn.discordapp.com/attachments/818400376255545395/974622304350896128/unknown.png)  
![pic3](https://cdn.discordapp.com/attachments/818400376255545395/974622683339816980/unknown.png)  
![pic4](https://cdn.discordapp.com/attachments/818400376255545395/974629195416236092/unknown.png)  

## Changelog   
- **1.0.8**  
fixed pink assets and Ashland weather issue.
- **1.0.7**  
fixed error regarding tool tiers  
updated serversync  
- **1.0.6**  
updated for mistlands  
**overhauled mining drops**:  
veins replaced by drops system  
any broken rock now has a chance to drop one or more materials  
drops can spawn any prefab such as creatures  
- **1.0.5**  
fixed configs issues when editing through r2modman, marked prefabs as static, updated serversync  
- **1.0.4**  
fixed required pickaxe tiers being too high in default config  
- **1.0.3**  
fixed min pickaxe tier config for veins not applying  
- **1.0.2.**  
fixed vein chance calculation resulting in infinite strips of veins  
- **1.0.1**  
fixed an error regarding piece configs  
- **1.0.0**  
initial upload  

## Prefab names  

mineshaft entrance - MS_Entrance  
abandoned mineshaft location - MS_D_AbandonedMineshaft  
mine chunk/tile - MS_MineTile  
wood planks 2x2 - MS_woodwall_2x2  
damaged wood planks 2x2 - MS_woodwall_2x2_damaged  
wood planks 2x4 - MS_woodwall_2x4  
damaged wood planks 2x4 - MS_woodwall_2x4_damaged  
wooden stairs - MS_woodstairs  
damaged wooden stairs - MS_woodstairs_damaged  
tier 1 chest - MS_chest_T1  
tier 2 chest - MS_chest_T2  
underground wall on hit effect - MS_FX_Tile_Hit  
underground wall on destroy effect - MS_FX_Tile_Destroyed  
bone pile skeleton spawner - MS_Bonepile  
