using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    private bool isActive;
    private Transform targetTransform;
    private Enemy _enemy;
    [SerializeField] private NavMeshAgent pathFinder;
    private float _lastUpdateTargetPath; 

    public void Activate(Enemy enemy, Transform playerTransform)
    {
        _enemy = enemy;
        _enemy.UpdateEnemyColor();
        pathFinder.speed = _enemy.EnemySpeed;
        targetTransform = playerTransform;
        StartCoroutine(WaitBeforeFollowingPlayer());
    }

    void Start()
    {
        isActive = false;
        pathFinder = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isActive)
        {
            float previousRotationZ = transform.eulerAngles.z;
            transform.LookAt(targetTransform);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, previousRotationZ);
        }

        if (targetTransform != null && Time.time - _lastUpdateTargetPath > 1.0f && pathFinder.isOnNavMesh)
        {
            StartCoroutine(UpdateTarget());
            _lastUpdateTargetPath = Time.time;
        }
    }

    IEnumerator UpdateTarget()
    {
        //yield return null;
        Vector3 targetPosition = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        pathFinder.SetDestination(targetPosition);
        yield return null;
    }

    IEnumerator WaitBeforeFollowingPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        isActive = true;
        pathFinder.enabled = true;
    }
}
