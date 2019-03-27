# Cubelike3D
Simple Extensible 3D Rougelike  game made in Unity for NextGames interview.

Controls:
Touch/Click and hold on screen to move player in that direction. Player will face and shoot in that direction.

Map Generation:
The map is generated at startup using a simple Cellular Automata methodology. Once the map is generated, isolated regions are ditected using a floodfill algorithm and tiny regions are removed, whle the rest of the regions are connected together by creating passageways.

Gameplay:
Enemies keep spawning at regular intervals. Enemies continously move towards the player.
Player must face the direction of the enemy in order to shoot them down.
This creates a balancing act for the player between, moving in the direction of enemies to kill them vs moving away from the enemies
to save themselves. 

Enemies: 
Enemy types are specified via xml files inside the StreamingAssets/Enemies directory. Each XML file is describes exactly 1 enemy type. 
Each enemy xml files describes the enemy's mesh(0 - 4), hitpoints, attack, speed, spawning rate etc.


Adding Enemy Xml files to this folder will spawn new enemies in game.
EnemyModifiers descripe mutations that will apply to all enemies, and the spawner will automatically spawn enemies with these mutations occasionally.
