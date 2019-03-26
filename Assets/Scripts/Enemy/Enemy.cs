using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _enemySpeed;
    [SerializeField] private int _enemyHitPoints;
    [SerializeField] private int _enemyScore;
    [SerializeField] private int _enemyAttack;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private MeshRenderer _enemyBodyRenderer;

    public float EnemySpeed{ get { return _enemySpeed; } }

    public void InitializeEnemy(EnemyInfo info)
    {
        name = info.enemyName;
        GameObject meshObject = Instantiate(info.enemyMeshObject) as GameObject;
        meshObject.transform.parent = transform;
        meshObject.transform.localRotation= Quaternion.identity;
        meshObject.transform.localPosition = Vector3.zero;
        _enemySpeed = info.enemySpeed;
        _enemyHitPoints = info.enemyHitPoints;
        _enemyScore = info.enemyScore;
        _enemyAttack = info.enemyAttack;
        _enemyColor = info.enemyColor;
        _enemyBodyRenderer = meshObject.GetComponent<MeshRenderer>();
    }

    public void UpdateEnemyColor()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        _enemyBodyRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", _enemyColor);
        _enemyBodyRenderer.SetPropertyBlock(block);

    }

    public void TakeDamage(int damage)
    {
        _enemyHitPoints -= damage;

        if (_enemyHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
