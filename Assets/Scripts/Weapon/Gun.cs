using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzleTransform;
    public Projectile projectile;
    public float fireCoolDownSeconds = 0.05f;

    private float lastShotFiredAt = 0.0f;

    public void Shoot()
    {
        if (Time.time - lastShotFiredAt > fireCoolDownSeconds)
        {
            lastShotFiredAt = Time.time;
            Projectile newProjectile = Instantiate(projectile) as Projectile;
            newProjectile.transform.position = muzzleTransform.position;
            newProjectile.transform.rotation = Quaternion.LookRotation(muzzleTransform.forward, Vector3.up);
        }
    }
}
