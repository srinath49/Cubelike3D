using UnityEngine;

public struct EnemyModifier
{
    public string nameModifier;
    public Color colorModifier;
    public float speedModifier;
    public int hitPointsModifier;
    public int scoreModifier;
    public int attackModifier;

    public EnemyModifier(string name, Color color, float speed, int hitPoints, int score, int attack)
    {
        nameModifier = name;
        colorModifier = color;
        speedModifier = speed;
        hitPointsModifier = hitPoints;
        scoreModifier = score;
        attackModifier = attack;
    }
}
