****************************************************************************************************************************************
# Cubelike3D
Simple Extensible 3D Rougelike  game made in Unity for NextGames interview.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Controls
Touch/Click and hold on screen to move player in that direction. Player will face and shoot in that direction.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Map Generation
The map is generated at startup using a simple Cellular Automata methodology. Once the map is generated, isolated regions are ditected using a floodfill algorithm and tiny regions are removed, whle the rest of the regions are connected together by creating passageways.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Gameplay
Enemies keep spawning at regular intervals. Enemies continously move towards the player.
Player must face the direction of the enemy in order to shoot them down.
This creates a balancing act for the player between, moving in the direction of enemies to kill them vs moving away from the enemies
to save themselves. 
****************************************************************************************************************************************


****************************************************************************************************************************************
# Enemies
Enemy types are specified via xml files inside the StreamingAssets/Enemies directory. Each XML file is describes exactly 1 enemy type. 
Each enemy xml files describes the enemy's mesh(0 - 4), hitpoints, attack, speed, spawning rate etc.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Enemy Modifiers
Modifiers are xml files that describe similar set of information to that of the enemy xml files, but each modifier applies to every enemy type. The values described in these files modify the exsiting values of the enemy.

Ex: Lets say we have EnemyA and Modifier1 as follows:
EnemyA has speed = 3 and attack = 2. 
Modifier1 has speed = -1 and attack = 1.
Then Enemy of type EnemyA will have same values as described by EnemyA.xml
However, Enemy of type EnemyA_Modifier1 will have speed = 2 (3-1) and attack = 3 (2 + 1).
****************************************************************************************************************************************


****************************************************************************************************************************************
# Enemy Spawner
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
****************************************************************************************************************************************


****************************************************************************************************************************************
# Game Design
#The following section describes the full design of the game including missing and partially implemented features.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Goals
The goal of the player is to kill as many enemies as possible and surviving for as long as possible. The player continously shoots in the direction he faces at any given time. The enemies cause the player a small amount of damage every second they are in contact with the player.

The player is awarded score points for each enemy they kill. The amount of score awarded depends on the type of enemy or enemy mutant they kill. The player is awarded additional score when they die, based on how long they survive.

The enemies have a chance to an item when they die, which when picked up, gives players additional health, additional score along with one of the following properties for few seconds:
1) Mutate player
2) Make player invinsible
3) Kill enemies on touch
4) Modified Ammo

While some of the mutation based drops may have negative effects, such as slow down player speed, etc, the score and health they provide should keep the player interested in them.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Map
The Game Map is generated at startup. The game loads XML files from StreamingAssets/Maps, where each xml file describes a map. The game randomly selects one from the list of files found and uses the map generation rules described in that file to generate the map.
The Map generation rules are described as steps. The generator, runs the algorithms associated with each of the rule/step described in the xml fil sequentially.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Enemies
Enemies in the game are the Evil Cubes and their shapely friends. That is to say all manner of primitive shapes. Each enemy type is described in their respective xml file. The xml file contains all the information of the enemy including speed, hitpoints, damage, etc.
Enemies inflict damage on player by staying in contact with the player. Every second they are in contact with the player, they will reduce the player's hitpoints by a value of their "attack". This damage ofcourse, depends on the player's state, i.e, if the player is in invinsible mode, due to an item, no damage will be inflicted on the player.

Enemies have 35% chance to drop items when they are killed. Each item gives players additional score points and minor health bonus. The items gives various boost for a limited amount of time to the player.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Mutations
Mutations or Modifiers are xml files that describe variations to the base attributes of the enemies. These mutations apply to every type of enemy. Each enemy spawner has about 60% chance to spawn a regular enemy, and 40% of picking one of the available mutations. A mutated enemy will mostly be similar to the base enemy type of which it is a mutation of, with slight changes to their speed, hitpoints, attack, etc. Each mutation has a specific color. A non mutated enemy will have a default color.

For example:
Enemy Type                        Enemy Type
{                                 {
    Name : Cubozoid                     Name : Cylindrilord
    Mesh : Cube                         Mesh : Cylinder
    Color: Blue                         Color: Blue
}                                 }

Mutation Type                     Mutation Type
{                                 {
    Name : Brute                      Name : Swift
    Color: Green                      Color: Yellow
}                                 }

The above enemy and mutation describes 6 possible enemies:
1) Blue Cube
2) Green Cube
3) Yellow Cube
4) Blue Cylider
5) Green Cylinder
6) Yelloe Cylinder

A Mutated enemy has a 45% chance to drop an item instead of 35%. When it does drop an item, it has a 50% chance to drop a regular item, and 50% chance to drop a special mutation item.

A mutation item is a special item that has significat health and score boost compared to a regular item, but in turn applies the mutation propeties to the player.
****************************************************************************************************************************************


****************************************************************************************************************************************
****************************************************************************************************************************************
 #                                                    REPORT
****************************************************************************************************************************************
****************************************************************************************************************************************

****************************************************************************************************************************************
# Implemented Features
1) Cellular Automata map generation
    * Generate Randomized tiles, then smooth them out to generate regions of similar tiles together.
    * Detect Isolated floor regions with flood fill algorithem 
    * Remove small isolated floor regions.
    * Connect isloated regions with pathways
2) Generate enemies from xml files. Easily add/remove enemy types.
3) Add mutations to enemy types with xml files
4) Enemy damage and death
5) Enemy pathfinding with using Unity navigation system.
****************************************************************************************************************************************

****************************************************************************************************************************************
# Missing/Incomplete Features
1) Generate map from xml files
2) Player damage/death system. Player is currently invincible
3) Items
4) Score
5) Move player with pathfinding
6) Replace unity pathfinding with A*
7) Add ability to specify path finding accuracy per enemy type and mutation type
****************************************************************************************************************************************

****************************************************************************************************************************************
# ToDo
1) Refactor the player and gun controller
2) Clean up other prototype code
3) Replace Lists with arrays if no List specific feature is required
4) Add object pooling system for bullets and enemies
5) Research and improve the way Navigation map is generated.
****************************************************************************************************************************************


****************************************************************************************************************************************
# Implementation Times
****************************************************************************************************************************************
The total development time of the implemented features was about 9 hours and 30 minutes, including some research. This is a little over the time set out for the NextGames test, but the project quickly became a hobby project for me, I almost forgot about turning in the test. I had a lot of fun with this project so far, and this will continue as a hobby project. Below is a break down of sections and a roughly how much each of these took.
****************************************************************************************************************************************
1) Map Generation       - 3 hrs
2) Enemy generation     - 2 hrs
3) Player Controller    - 30 mins
4) Shooting and Damage  - 1 hr
5) All research         - 2 hours
6) Code Refactoring     - 1 hr
****************************************************************************************************************************************
