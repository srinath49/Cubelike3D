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

Enemy Modifiers:
Modifiers are xml files that describe similar set of information to that of the enemy xml files, but each modifier applies to every enemy type. The values described in these files modify the exsiting values of the enemy.

Ex: Lets say we have EnemyA and Modifier1 as follows:
EnemyA has speed = 3 and attack = 2. 
Modifier1 has speed = -1 and attack = 1.
Then Enemy of type EnemyA will have same values as described by EnemyA.xml
However, Enemy of type EnemyA_Modifier1 will have speed = 2 (3-1) and attack = 3 (2 + 1).

Enemy Spawner.
For each Enemy.xml file found in StreamingAssets/Enemies, the game creates an EnemySpawner object, which spawns enemies every n secods as mentioned by the Enemy.xml file it corresponds to. The spawner creates 1 profile each for the Enemy type it corresponds to plus every modifier.

Ex: 
A.xml and B.xml are 2 enemy types in StreamingAssets/Enemies
1.xml, 2.xml, and 3.xml are 3 modifiers in StreamingAssets/EnemyModifiers

THe game creates 2 enemy spawners: Spawner_A and Spawner_B.
Each of these spawners: Spawner_A and Spawner_B, are capable of Spawning 4 unique enemies.
Spawner_A can spawn: Enemy_A, Enemy_A_1, Enemy_A_2, Enemy_A_3
Spawner_B can spawn: Enemy_B, Enemy_B_1, Enemy_B_2, Enemy_B_3
Where enemies: Enemy_X_1, Enemy_X_2, and Enemy_X_3 are a modified version (or mutation) of enemy Enemy_X

The user can easily add more modifiers and enemy types to the game by adding the corresponding xml files to their respecting folders.
