using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;


public enum TileType
{
    Floor,
    Wall,
    //Water,
    //Lava,
}


public enum GenerationSteps
{
    RandomAssign,
    FillFromFile,
    Smooth,
    RemoveSmallRooms,
    ConnectRooms
}

public class MapGenerator : MonoBehaviour
{
    struct Region
    {
        public TileType tileTypeInRegion;
        public Vector2Int[] tileCoordinates;
        public int numTilesInRegion;
    }

    public int numTilesInRow;
    public int numTilesInColumn;
    public bool useRandomSeed;
    public string seed;
    public int minRegionSize = 50;
    public int passageWayRadius = 2;
    public NavMeshSurface navMeshSurface;

    public GameObject tilePrefab;
    public Material caveWall;
    public Material caveFloor;

    [Range(0, 100)] public int randomFillPercent;

    private System.Random random;
    private TileType[,] map;
    private GenerationSteps[] mapGenerationSteps;
    private int numSteps;
    private GameObject[,] tiles;
    private Room[] roomsAboveThreshold;
    private int numRoomsAboveThreshold;
    private Vector2Int[] floorTiles;
    private int numFloorTiles;
    private int numNavMesesBuilt;

    public Vector2Int[] FloorTiles
    {
        get { return floorTiles; }
    }

    public int NumFloorTiles
    {
        get { return numFloorTiles; }
    }

    #region Monobehaviors
    private void Awake()
    {
        if (useRandomSeed)
        {
            seed = System.DateTime.Now.Ticks.ToString();
        }
        random = new System.Random(seed.GetHashCode());
        CreateTileObjects();
        map = new TileType[numTilesInRow, numTilesInColumn];

        mapGenerationSteps = new GenerationSteps[100];

        if (!ParseFile())
        {
            numSteps = 10;
            mapGenerationSteps[0] = GenerationSteps.RandomAssign;
            mapGenerationSteps[1] = GenerationSteps.Smooth;
            mapGenerationSteps[2] = GenerationSteps.Smooth;
            mapGenerationSteps[3] = GenerationSteps.Smooth;
            mapGenerationSteps[4] = GenerationSteps.Smooth;
            mapGenerationSteps[5] = GenerationSteps.Smooth;
            mapGenerationSteps[6] = GenerationSteps.RemoveSmallRooms;
            mapGenerationSteps[7] = GenerationSteps.Smooth;
            mapGenerationSteps[8] = GenerationSteps.ConnectRooms;
            mapGenerationSteps[9] = GenerationSteps.Smooth;
        }

        GenerateMap();
    }

    private void Update()
    {
        if (numNavMesesBuilt >= numFloorTiles)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    #endregion
    
    #region Map Generation
    public void GenerateMap()
    {
        for (int generationStep = 0; generationStep < numSteps; generationStep++)
        {
            switch (mapGenerationSteps[generationStep])
            {
                case GenerationSteps.RandomAssign:
                    RandoomFillMap();
                    break;
                case GenerationSteps.Smooth:
                    SmoothStep();
                    break;
                case GenerationSteps.RemoveSmallRooms:
                    RemoveSmallRooms();
                    break;
                case GenerationSteps.ConnectRooms:
                    ConnectAllRooms();
                    break;
            }
        }
        UpdateTileObjects();
        BuildNavMesh();
    }

    private void RandoomFillMap()
    {
       for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                if (random.Next(0, 100) < randomFillPercent)
                {
                    map[x, y] = TileType.Wall;
                }
                else
                {
                    map[x, y] = TileType.Floor;
                }
            }
        }
    }

    private void SmoothStep()
    {
        for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                if (x == 0 || x == numTilesInRow - 1 || y == 0 || y == numTilesInColumn - 1)
                {
                    map[x, y] = TileType.Wall;
                    continue;
                }

                int numSorroundingWallTiles = GetNumberofSurroundingWalls(x, y);
                if (numSorroundingWallTiles > 4)
                {
                    map[x, y] = TileType.Wall;
                }
                else if (numSorroundingWallTiles < 4)
                {
                    map[x, y] = TileType.Floor;
                }
            }
        }
    }

    private void RemoveSmallRooms()
    {
        int numFoundRegions = 0;
        Region[] floorRegions = GetAllRegionsOfType(TileType.Floor, out numFoundRegions);

        roomsAboveThreshold = new Room[numFoundRegions];

        for (int regionIndex = 0; regionIndex < numFoundRegions; regionIndex++)
        {
            if (floorRegions[regionIndex].numTilesInRegion < minRegionSize)
            {
                for (int tileIndex = 0; tileIndex < floorRegions[regionIndex].numTilesInRegion; tileIndex++)
                {
                    Vector2Int tile = floorRegions[regionIndex].tileCoordinates[tileIndex];
                    map[tile.x, tile.y] = TileType.Wall;
                }
            }
            else
            {
                roomsAboveThreshold[numRoomsAboveThreshold] = new Room(floorRegions[regionIndex].tileCoordinates, floorRegions[regionIndex].numTilesInRegion, map, tiles);
                numRoomsAboveThreshold++;
            }
        }
    }

    private void ConnectAllRooms()
    {
        int roomIndexWithMostTiles = 0;
        int mostTilesInRoom = 0;

        for (int roomIndex = 0; roomIndex < numRoomsAboveThreshold; roomIndex++)
        {
            if (roomsAboveThreshold[roomIndex].tilesInThisRoom.Length > mostTilesInRoom)
            {
                mostTilesInRoom = roomsAboveThreshold[roomIndex].tilesInThisRoom.Length;
                roomIndexWithMostTiles = roomIndex;
            }
        }

        roomsAboveThreshold[roomIndexWithMostTiles].isMainRoom = true;
        roomsAboveThreshold[roomIndexWithMostTiles].isAccessibleFromMainRoom = true;

#if UNITY_EDITOR
        //foreach (var tile in roomsAboveThreshold[0].tilesInThisRoom)
        //{
        //    MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        //    tiles[tile.x, tile.y].GetComponent<MeshRenderer>().GetPropertyBlock(propertyBlock);
        //    propertyBlock.SetColor("_Color", Color.white);
        //    tiles[tile.x, tile.y].GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
        //}
#endif
        ConnectClosestRooms();
    }

    private Vector2Int[] GetConnectingTilesList(Vector2Int fromTile, Vector2Int toTile)
    {
        bool inverted = false;
        int x = fromTile.x;
        int y = fromTile.y;

        int dx = toTile.x - fromTile.x;
        int dy = toTile.y - fromTile.y;

        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Math.Abs(dx);
        int shortest = Math.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Math.Abs(dy);
            shortest = Math.Abs(dx);
            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;

        Vector2Int[] connectingTiles = new Vector2Int[longest];

        for (int i = 0; i < longest; i++)
        {
            connectingTiles[i] = new Vector2Int(x, y);

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }

                gradientAccumulation -= longest;
            }
        }

        return connectingTiles;
    }
    #endregion

    #region Rooms
    private void ConnectClosestRooms(bool forceAccessibilityFromMainRoom = false)
    {
        Room[] roomArrayOne = new Room[numRoomsAboveThreshold];
        Room[] roomArrayTwo = new Room[numRoomsAboveThreshold];
        int numRoomsInArrayOne = 0;
        int numRoomsInArrayTwo = 0;

        if (forceAccessibilityFromMainRoom)
        {
            for (int roomsIndex = 0; roomsIndex < roomsAboveThreshold.Length; roomsIndex++)
            {
                if (roomsAboveThreshold[roomsIndex].isAccessibleFromMainRoom)
                {
                    roomArrayTwo[numRoomsInArrayTwo] = roomsAboveThreshold[roomsIndex];
                    numRoomsInArrayTwo++;
                }
                else
                {
                    roomArrayOne[numRoomsInArrayOne] = roomsAboveThreshold[roomsIndex];
                    numRoomsInArrayOne++;
                }
            }
        }
        else
        {
            roomArrayOne = roomsAboveThreshold;
            roomArrayTwo = roomsAboveThreshold;
            numRoomsInArrayOne = numRoomsAboveThreshold;
            numRoomsInArrayTwo = numRoomsAboveThreshold;
        }

        int closestEdgeTileIndexA = 0;
        int closestEdgeTileIndexB = 0;
        Room closestRoomA = new Room();
        Room closestRoomB = new Room();
        bool foundPossibleConnection = false; 

        for (int roomOneIndex = 0; roomOneIndex < numRoomsInArrayOne; roomOneIndex++)
        {
            Room roomA = roomArrayOne[roomOneIndex];
            if (!forceAccessibilityFromMainRoom)
            {
                foundPossibleConnection = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            float closestDistance = 10000.0f;
            for (int roomTwoIndex = 0; roomTwoIndex < numRoomsInArrayTwo; roomTwoIndex++)
            {
                Room roomB = roomArrayTwo[roomTwoIndex];
                if (roomA == roomB || roomA.IsConnectedToRoom(roomB))
                {
                    continue;
                }
                
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Vector2Int tileA = roomA.edgeTiles[tileIndexA];
                        Vector2Int tileB = roomB.edgeTiles[tileIndexB];
                        float distanceBetweenTiles = Vector2Int.Distance(tileA, tileB);

                        if (distanceBetweenTiles < closestDistance ||!foundPossibleConnection)
                        {
                            foundPossibleConnection = true;
                            closestDistance = distanceBetweenTiles;
                            closestEdgeTileIndexA = tileIndexA;
                            closestEdgeTileIndexB = tileIndexB;
                            closestRoomA = roomA;
                            closestRoomB = roomB;
                        }
                    }
                }
            }

            if (foundPossibleConnection && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(closestRoomA, closestRoomB, closestEdgeTileIndexA, closestEdgeTileIndexB);
            }
        }

        if (foundPossibleConnection && forceAccessibilityFromMainRoom)
        {
            CreatePassage(closestRoomA, closestRoomB, closestEdgeTileIndexA, closestEdgeTileIndexB);
            ConnectClosestRooms(true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(true);
        }
    }

    private void CreatePassage(Room roomA, Room roomB, int edgeTileIndexA, int edgeTileIndexB)
    {
        Room.ConnectRooms(roomA, roomB);
        Vector2Int tileA = roomA.edgeTiles[edgeTileIndexA];
        Vector2Int tileB = roomB.edgeTiles[edgeTileIndexB];
        Vector2Int[] connectingTiles = GetConnectingTilesList(tileA, tileB);

        for (int connectingIndex = 0; connectingIndex < connectingTiles.Length; connectingIndex++)
        {
            CarvePassageWay(connectingTiles[connectingIndex]);
        }
    }

    private void CarvePassageWay(Vector2Int tileToCarveFrom)
    {
        int passageWayRadiusSqured = passageWayRadius * passageWayRadius;
        for (int x = -passageWayRadius; x <= passageWayRadius; x++)
        {
            for (int y = -passageWayRadius; y <= passageWayRadius; y++)
            {
                if (x * x + y * y < passageWayRadiusSqured)
                {
                    int carvePointX = tileToCarveFrom.x + x;
                    int carvePointY = tileToCarveFrom.y + y;
                    if (IsTileWithinMap(carvePointX, carvePointY))
                    {
                        map[carvePointX, carvePointY] = TileType.Floor;
                    }
                }
            }
        }
    }
    #endregion

    #region Support Methods
    private void CreateTileObjects()
    {
        tiles = new GameObject[numTilesInRow, numTilesInColumn];

        for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                tiles[x, y] = Instantiate(tilePrefab, new Vector3(x, 1.0f, y), Quaternion.identity);
            }
        }
    }

    private void UpdateTileObjects()
    {
        floorTiles = new Vector2Int[tiles.Length];
        numFloorTiles = 0;

        for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                TileType currentTileType = map[x, y];
                Vector3 tilePosition = new Vector3(x, (int)currentTileType, y);
                tiles[x, y].transform.position = tilePosition;
                tiles[x, y].GetComponent<MeshRenderer>().material = (currentTileType == TileType.Wall)?caveWall:caveFloor;
                if (currentTileType == TileType.Floor)
                {
                    floorTiles[numFloorTiles] = new Vector2Int(x, y);
                    numFloorTiles++;
                }
            }
        }
    }

    private void BuildNavMesh()
    {
        for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                if (map[x,y] == TileType.Floor)
                {
                    NavMeshSurface navSurface = tiles[x, y].AddComponent<NavMeshSurface>();
                    navSurface.agentTypeID = navMeshSurface.agentTypeID;
                }
                else
                {
                    NavMeshObstacle obstacle = tiles[x, y].AddComponent<NavMeshObstacle>();
                }
            }
        }

        numNavMesesBuilt = 0;
        for (int floorIndex = 0; floorIndex < numFloorTiles; floorIndex++)
        {
            Vector2Int currentFloorTile = floorTiles[floorIndex];
            NavMeshSurface currentSurface = 
                tiles[currentFloorTile.x, currentFloorTile.y].GetComponent<NavMeshSurface>();
            if (currentSurface != null)
            {
                StartCoroutine(BuildTileNavMesh(currentSurface));
            }
        }
    }

    private IEnumerator BuildTileNavMesh(NavMeshSurface navSurface)
    {
        yield return null;
        navSurface.BuildNavMesh();
        numNavMesesBuilt++;
    }

    private bool IsTileWithinMap(int x, int y)
    {
        return (x >= 0 && x < numTilesInRow && y >= 0 && y < numTilesInColumn);
    }

    private int GetNumberofSurroundingWalls(int xPos, int yPos)
    {
        int numSorroundingWalls = 0;

        for (int neighbourX = xPos - 1; neighbourX <= xPos + 1; neighbourX++)
        {
            for (int neighbourY = yPos - 1; neighbourY <= yPos + 1; neighbourY++)
            {
                if (neighbourX == xPos && neighbourY == yPos)
                {
                    continue;
                }

                if (IsTileWithinMap(neighbourX, neighbourY))
                {
                    if (map[neighbourX, neighbourY] == TileType.Wall)
                    {
                        numSorroundingWalls++;
                    }
                }
                else
                {
                    numSorroundingWalls++;
                }
            }
        }

        return numSorroundingWalls;
    }

    Vector2Int[] GetTilesInRegion(Vector2Int tileCoordinates, TileType typeOfTileInRegion, out int numTilesInRegion)
    {
        Vector2Int[] tilesInRegion = new Vector2Int[tiles.Length];
        int[,] tileLookedAtFlags = new int[numTilesInRow, numTilesInColumn];
        numTilesInRegion = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(tileCoordinates);
        tileLookedAtFlags[tileCoordinates.x, tileCoordinates.y] = 1;

        do
        {
            Vector2Int currentTile = queue.Dequeue();
            tilesInRegion[numTilesInRegion] = currentTile;
            numTilesInRegion++;

            for (int x = currentTile.x - 1; x <= currentTile.x + 1; x++)
            {
                for (int y = currentTile.y - 1; y <= currentTile.y + 1; y++)
                {
                    if (IsTileWithinMap(x, y))
                    {
                        if (tileLookedAtFlags[x, y] == 0 && map[x, y] == typeOfTileInRegion)
                        {
                            queue.Enqueue(new Vector2Int(x, y));
                            tileLookedAtFlags[x, y] = 1;
                        }
                    }
                }
            }
        } while (queue.Count > 0);

        return tilesInRegion;
    }

    Region[] GetAllRegionsOfType(TileType typeOfTilesInRegion, out int numFoundRegions)
    {
        Region[] foundRegions = new Region[15];
        int[,] tileLookedAtFlags = new int[numTilesInRow, numTilesInColumn];
        numFoundRegions = 0;

        for (int x = 0; x < numTilesInRow; x++)
        {
            for (int y = 0; y < numTilesInColumn; y++)
            {
                if (tileLookedAtFlags[x, y] == 0 && map[x, y] == typeOfTilesInRegion)
                {
                    Region newRegion = new Region();
                    newRegion.tileTypeInRegion = typeOfTilesInRegion;
                    newRegion.tileCoordinates = GetTilesInRegion(new Vector2Int(x, y), typeOfTilesInRegion, out newRegion.numTilesInRegion);
                    foundRegions[numFoundRegions] = newRegion;
                    numFoundRegions++;

                    for (int tileIndex = 0; tileIndex < newRegion.numTilesInRegion; tileIndex++)
                    {
                        Vector2Int tile = newRegion.tileCoordinates[tileIndex];
                        tileLookedAtFlags[tile.x, tile.y] = 1;
                    }
                }
            }
        }

        return foundRegions;
    }
    #endregion

    private bool ParseFile()
    {
        return false;
    }
}
