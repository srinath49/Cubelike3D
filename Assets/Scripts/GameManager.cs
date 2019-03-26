using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerManager _playerManager;

    private Vector2Int[] floorTilesInMap;
    private int numFloorTiles;
    private System.Random random;

    void Start()
    {
        random = new System.Random(gameObject.name.GetHashCode());
        floorTilesInMap = _mapGenerator.FloorTiles;
        numFloorTiles = _mapGenerator.NumFloorTiles;
    }

    public Vector2Int GetRandomFloorCoordinates()
    {
        int index = random.Next(0, numFloorTiles - 1);
        return floorTilesInMap[index];
    }
}
