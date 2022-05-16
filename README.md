## Notes

Use the mod on a server to sync configs.  
In case of questions you may find me on the [OdinPlus](https://discord.gg/s7tUavejWR) or the [Valheim modding](https://discord.gg/MXqWrn532w) discord servers.  
You may also file an issue on [GitHub](https://github.com/GoldenJude/Mineshafts/issues).  

## Features

#### Underground mining
- place down a mineshaft entrance, grab your pickaxe and uncover the riches hiding deep in the earth  
- upon entering a mine you may dig around using your pickaxe and eventually find ores  
- building within mines is allowed  
- it is possible to link up separate mineshafts by digging towards them  

#### Abandoned mineshafts
- explore the remains of past mining operations and fight the entrapped miners  
- abandoned mineshafts generate throughout the world and contain loot aswell as exposed ore veins  

## Configuration
Configuration for the mod is done within the ``GoldenJude_Mineshafts.cfg`` file created upon game launch.  
All configs reload during runtime.  

#### General configuration  
``wall_health`` = configures the health of underground rock  
``min_pickaxe_tier`` = minimum pickaxe tier needed to mine generic underground rock  
``vein_chance`` = percent chance that a vein will appear  
``default_drop`` = default drop when mining underground rock  
``default_drop_min``/``max`` = amount of dropped default material  

#### Building piece recipe  
``items`` = array of items followed by their required amounts for this building piece  
``recover`` = defines if required items should be recovered upon destruction

#### Abandoned mineshaft configuration  
``spawn_chance`` = percent chance that a mineshaft will spawn if possible  
``quantity`` = amount of abandoned mineshafts to be generated throughout the world  
``min_spacing`` = minimum spacing between abandoned mineshafts (if an abandoned mineshaft is spawned another one cannot spawn within this distance of it)  
``rooms`` = amount of rooms to be generated in an abandoned mineshaft  

#### Veins
``biomes`` = array of possible bioms this vein may spawn in  
possible biomes: Global, None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands  
``weight`` = likelinnes of a vein occuring over others in the same biome  
``min_pickaxe_tier`` = minimum pickaxe tier required to mine this vein  
``color`` = color of the vein model defined in hex color code  
``emission_color`` = color of the glow on the vein model, black - no glow  
``metallic`` = adds a metallic shine to the vein  
``shine`` = defines the smoothness/shine of the vein model  
``drop`` = item dropped by this vein  
``drop min``/``max`` = amount of dropped material  
to define a vein config the header must start with ``vein_`` followed by anything, for example ``[vein_my_vein-mistlands-32]``  

## Screenshots  

![pic1](https://cdn.discordapp.com/attachments/818400376255545395/974630185217441822/unknown.png)  
![pic2](https://cdn.discordapp.com/attachments/818400376255545395/974622304350896128/unknown.png)  
![pic3](https://cdn.discordapp.com/attachments/818400376255545395/974622683339816980/unknown.png)  
![pic4](https://cdn.discordapp.com/attachments/818400376255545395/974629195416236092/unknown.png)  

## Changelog   
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
