using System;
using System.Collections.Generic;
using UnityEngine;

class Room : IComparable<Room>
{
    public Vector2Int[] tilesInThisRoom;
    public List<Vector2Int> edgeTiles;
    public List<Room> connectedRooms;
    public int roomSize;
    public bool isMainRoom;
    public bool isAccessibleFromMainRoom;

    public Room() { }

    public Room(Vector2Int[] tilesInRoom, int numTilesInRegion, TileType[,] map, GameObject[,] tiles)
    {
        tilesInThisRoom = tilesInRoom;
        roomSize = numTilesInRegion;
        connectedRooms = new List<Room>();
        edgeTiles = new List<Vector2Int>();

        for (int tileIndex = 0; tileIndex < roomSize; tileIndex++)
        {
            Vector2Int tile = tilesInThisRoom[tileIndex];
            for (int x = tile.x - 1; x <= tile.x + 1; x++)
            {
                for (int y = tile.y - 1; y <= tile.y + 1; y++)
                {
                    if (map[x, y] == TileType.Wall && !edgeTiles.Contains(tile))
                    {
                        edgeTiles.Add(tile);
                    }
                }
            }
        }
    }

    public bool IsConnectedToRoom(Room otherRoom)
    {
        return (connectedRooms != null && connectedRooms.Contains(otherRoom));
    }

    public int CompareTo(Room otherRoom)
    {
        return otherRoom.roomSize.CompareTo(roomSize);
    }

    public void SetAccessibleFromMainRoom()
    {
        if (!isAccessibleFromMainRoom)
        {
            isAccessibleFromMainRoom = true;

            foreach (var connectedRoom in connectedRooms)
            {
                connectedRoom.SetAccessibleFromMainRoom();
            }
        }
    }

    public static void ConnectRooms(Room roomOne, Room roomTwo)
    {
        roomOne.connectedRooms.Add(roomTwo);
        roomTwo.connectedRooms.Add(roomOne);
        if (roomOne.isAccessibleFromMainRoom)
        {
            roomTwo.SetAccessibleFromMainRoom();
        }
        if (roomTwo.isAccessibleFromMainRoom)
        {
            roomOne.SetAccessibleFromMainRoom();
        }
    }
}
