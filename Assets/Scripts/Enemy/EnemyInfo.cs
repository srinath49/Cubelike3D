using System;
using UnityEngine;

public struct EnemyInfo
{
    public string enemyName;
    public GameObject enemyMeshObject;
    public Color enemyColor;
    public float enemySpeed;
    public int enemyHitPoints;
    public int enemyScore;
    public int enemyAttack;
    public int minScoreBeforeSpawning;
    public float spawnRate;

    public EnemyInfo(string name, GameObject meshObject, Color color, float speed, int hitPoints, int score, int attack, int minScore, float rate)
    {
        enemyName               = name;
        enemyMeshObject         = meshObject;
        enemyColor              = color;
        enemySpeed              = speed;
        enemyHitPoints          = hitPoints;
        enemyScore              = score;
        enemyAttack             = attack;
        minScoreBeforeSpawning  = minScore;
        spawnRate               = rate;
    }

    public EnemyInfo(EnemyInfo otherInfo, EnemyModifier modifier)
    {
        enemyName               = otherInfo.enemyName + "_" + modifier.nameModifier;
        enemyMeshObject         = otherInfo.enemyMeshObject;
        enemyColor              = modifier.colorModifier;
        enemySpeed              = Mathf.Max((otherInfo.enemySpeed + modifier.speedModifier), 1);
        enemyHitPoints          = Math.Max(otherInfo.enemyHitPoints + modifier.hitPointsModifier , 1);
        enemyScore              = Math.Max(otherInfo.enemyScore + modifier.scoreModifier, 1);
        enemyAttack             = Math.Max(otherInfo.enemyAttack + modifier.attackModifier, 1);
        minScoreBeforeSpawning  = otherInfo.minScoreBeforeSpawning;
        spawnRate               = otherInfo.spawnRate;
    }
}
