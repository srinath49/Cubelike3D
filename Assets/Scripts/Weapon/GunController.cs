using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun playerGun;

    public void Shoot()
    {
        playerGun.Shoot();
    }

}
