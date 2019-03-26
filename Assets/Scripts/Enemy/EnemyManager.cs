using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public string enemiesDirectory = "Enemies";
    public string enemyModifiersDirectory = "EnemyModifiers";
    public GameObject[] enemyMeshObjects;
    public Color defaultEnemyColor;
    public GameObject enemyPrefab;
    public PlayerManager _playerManager;
    public GameManager _gameManager;

    private EnemyInfo[] enemyInfos;
    private EnemyModifier[] enemyMutations;

    void Awake()
    {
        XMLLoader loader = new XMLLoader();
        XmlDocument[] enemies = loader.GetAllXMLDocumentsInDirectory(enemiesDirectory);
        enemyInfos = new EnemyInfo[enemies.Length];

        for (int enemyIndex = 0; enemyIndex < enemies.Length; enemyIndex++)
        {
            XmlNode parentNode      = enemies[enemyIndex].FirstChild;
            XmlNode nameNode        = parentNode.NextSibling.FirstChild;
            XmlNode attributesNode  = nameNode.NextSibling;
            XmlNode meshNode        = attributesNode.FirstChild;
            XmlNode speedNode       = meshNode.NextSibling;
            XmlNode hitPointsNode   = speedNode.NextSibling;
            XmlNode scoreNode       = hitPointsNode.NextSibling;
            XmlNode attackNode      = scoreNode.NextSibling;
            XmlNode minScoreNode    = attackNode.NextSibling;
            XmlNode spawnRateNode   = minScoreNode.NextSibling;

            int meshIndex;
            float speed;
            int hitPoints;
            int score;
            int attack;
            int minScore;
            float rate;

            int.TryParse(meshNode.InnerText, out meshIndex);
            if (meshIndex < 0 || meshIndex >= enemyMeshObjects.Length)
            { 
                meshIndex = 0;
            }

            float.TryParse(speedNode.InnerText, out speed);
            int.TryParse(hitPointsNode.InnerText, out hitPoints);
            int.TryParse(scoreNode.InnerText, out score);
            int.TryParse(attackNode.InnerText, out attack);
            int.TryParse(minScoreNode.InnerText, out minScore);
            float.TryParse(spawnRateNode.InnerText, out rate);
            EnemyInfo newEnemyInfo = new EnemyInfo(nameNode.InnerText, enemyMeshObjects[meshIndex], defaultEnemyColor, speed, hitPoints, score, attack, minScore, rate);
            enemyInfos[enemyIndex] = newEnemyInfo;
        }

        XmlDocument[] modifiers = loader.GetAllXMLDocumentsInDirectory(enemyModifiersDirectory);
        enemyMutations = new EnemyModifier[modifiers.Length];

        for (int modifierIndex = 0; modifierIndex < modifiers.Length; modifierIndex++)
        {
            XmlNode parentNode      = modifiers[modifierIndex].FirstChild;
            XmlNode nameNode        = parentNode.NextSibling.FirstChild;
            XmlNode attributesNode  = nameNode.NextSibling;
            XmlNode colorNode       = attributesNode.FirstChild;
            XmlNode speedNode       = colorNode.NextSibling;
            XmlNode hitPointsNode   = speedNode.NextSibling;
            XmlNode scoreNode       = hitPointsNode.NextSibling;
            XmlNode attackNode      = scoreNode.NextSibling;

            Color newColor;
            float speedModifier;
            int hitPointsModifier;
            int scoreModifier;
            int attackModifier;
            
            ColorUtility.TryParseHtmlString(colorNode.InnerText, out newColor);
            float.TryParse(speedNode.InnerText, out speedModifier);
            int.TryParse(hitPointsNode.InnerText, out hitPointsModifier);
            int.TryParse(scoreNode.InnerText, out scoreModifier);
            int.TryParse(attackNode.InnerText, out attackModifier);

            EnemyModifier newModifier = new EnemyModifier(nameNode.InnerText, newColor, speedModifier, hitPointsModifier, scoreModifier, attackModifier);
            enemyMutations[modifierIndex] = newModifier;
        }
    }

    public void Start()
    {
        
        CreateEnemySpawners();
    }

    public void CreateEnemySpawners()
    {
        for (int enemyIndex = 0; enemyIndex < enemyInfos.Length; enemyIndex++)
        {
            GameObject newSpawner = new GameObject();
            newSpawner.name = enemyInfos[enemyIndex].enemyName + "_Spawner";
            EnemySpawner spawnerScript = newSpawner.AddComponent<EnemySpawner>();
            spawnerScript.Initialize(enemyPrefab, enemyInfos[enemyIndex], enemyMutations, _gameManager, _playerManager);
        }
    }
}
