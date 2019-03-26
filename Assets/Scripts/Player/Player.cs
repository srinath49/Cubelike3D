using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour, IDamageable, IRepairable
{
    public float moveSpeed = 6.0f;
    private PlayerController playerController;
    private GunController gunController;
    private Camera playerViewCamera;
    private int _currentHitPoints;
    private int _currentMaxHitPoints;
    private int _baseHitPoints;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        playerViewCamera = Camera.main;
        playerViewCamera.GetComponent<FollowTranaform>().SetFollowTransform(transform);
        _baseHitPoints = 100;
        _currentMaxHitPoints = _baseHitPoints;
        _currentHitPoints = _baseHitPoints;
    }

    void Update()
    {
        Vector3 touchPosition = new Vector3();
        float currentMoveSpeed = 0.0f;
        if (Input.GetMouseButton(0))
        {
            Ray ray = playerViewCamera.ScreenPointToRay(Input.mousePosition);
            Plane worldPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance = 0.0f;

            if (worldPlane.Raycast(ray, out rayDistance))
            {
                touchPosition = ray.GetPoint(rayDistance);
            }

            currentMoveSpeed = moveSpeed;
        }

        playerController.MovePlayer(touchPosition, currentMoveSpeed);
        gunController.Shoot();
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;

        if (_currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int healPoints)
    {
        _currentHitPoints = Mathf.Max(_currentHitPoints + healPoints, _currentMaxHitPoints);
    }
}
