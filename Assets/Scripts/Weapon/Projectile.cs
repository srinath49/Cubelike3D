using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask layerMask;
    public float currentSpeed = 50;
    public int damage;
    public float lifeTimeSeconds;
    private float _spawnTime;
    private Vector3 movementDirection;

    public void SetDirection(Vector3 muzzleForward)
    {
        movementDirection = muzzleForward;
    }

    void Awake()
    {
        _spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - _spawnTime >= lifeTimeSeconds)
        {
            Destroy(gameObject);
            return;
        }

        float movementDelta = currentSpeed * Time.deltaTime;

        //Ray ray = new Ray(transform.position, movementDirection);
        //RaycastHit hit;
        //Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.red);
        //if (Physics.Raycast(ray, out hit, 2* movementDelta, layerMask, QueryTriggerInteraction.Collide))
        //{
        //    OnHit(hit);
        //}
        transform.Translate(Vector3.forward * movementDelta);
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
            Destroy(gameObject);
        }

        damageableObject = other.GetComponentInParent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnHit(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }

        damageableObject = hit.collider.GetComponentInParent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
