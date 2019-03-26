using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[]    _enemies;
    private bool            _isSpawnerActive;
    private bool            _initialized;
    private int             _minScoreToActivate;
    private float           _enemySpawnRateSeconds;
    private float           _lastSpawnTime;
    private System.Random   _randomSpawner;
    private GameManager     _gameManager;
    private PlayerManager   _playerManager;
    private int             _enemyLayer;

    private void Start()
    {
        _isSpawnerActive = false;
    }

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }

        if (!_isSpawnerActive)
        {
            if (_playerManager.GetScore() >= _minScoreToActivate)
            {
                Activate();
                return;
            }
        }

        if (_isSpawnerActive && Time.time - _lastSpawnTime >= _enemySpawnRateSeconds)
        {
            Spawn();
        }
    }

    public void Initialize(GameObject enemyPrefab, EnemyInfo enemyInfo, EnemyModifier[] modifiers, GameManager gameManager, PlayerManager playerManager)
    {
        _gameManager = gameManager;
        _playerManager = playerManager;
        _enemies = new GameObject[modifiers.Length+1];
        _minScoreToActivate = enemyInfo.minScoreBeforeSpawning;
        _enemySpawnRateSeconds = enemyInfo.spawnRate;
        _enemyLayer = enemyPrefab.layer;

        GameObject newEnemyObject = Instantiate(enemyPrefab);
        Enemy newEnemy = newEnemyObject.AddComponent<Enemy>();
        newEnemy.InitializeEnemy(enemyInfo);
        _enemies[0] = newEnemyObject;

        for (int modifierIndex = 0; modifierIndex < modifiers.Length; modifierIndex++)
        {
            EnemyModifier currentModifier = modifiers[modifierIndex];
            EnemyInfo currentInfo = new EnemyInfo(enemyInfo, currentModifier);

            newEnemyObject = Instantiate(enemyPrefab);
            newEnemy = newEnemyObject.AddComponent<Enemy>();
            newEnemy.InitializeEnemy(currentInfo);

            int enemyIndex = modifierIndex + 1;
            _enemies[enemyIndex] = newEnemyObject;
        }

        _randomSpawner = new System.Random(enemyInfo.enemyName.GetHashCode());
        _initialized = true;
    }

    private void Activate()
    {
        _isSpawnerActive = true;
        _lastSpawnTime = Time.deltaTime;
    }

    private void Spawn()
    {
        int enemyRandomizer = _randomSpawner.Next(0, 10);
        GameObject newEnemy;
        if (enemyRandomizer < 6)
        {
            newEnemy = Instantiate(_enemies[0]);
        }
        else
        {
            int mutantRandomizer = _randomSpawner.Next(1, _enemies.Length);
            newEnemy = Instantiate(_enemies[mutantRandomizer]);
        }
        newEnemy.gameObject.layer = _enemyLayer;
        newEnemy.SetActive(true);
        Vector2Int enemySpawnPoint = _gameManager.GetRandomFloorCoordinates();
        newEnemy.transform.position = new Vector3(enemySpawnPoint.x, 1.5f, enemySpawnPoint.y);
        newEnemy.GetComponent<EnemyController>().Activate(newEnemy.GetComponent<Enemy>(), _playerManager.GetPlayerTransform());
        _lastSpawnTime = Time.time;
    }
}
