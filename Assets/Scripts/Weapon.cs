using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        //this references the bullet object and the firepoint posistion and rotation of referencing the player
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
