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
        float direction = transform.root.localScale.x > 0 ? 1f : -1f;

        // Rotate the bullet if facing left
        Quaternion bulletRotation = direction > 0 ? Quaternion.identity : Quaternion.Euler(0f, 0f, 180f);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

        #region Old Code to instantiate the bullet at the player position instead of firepoint position
        //this references the bullet object and the firepoint posistion and rotation of referencing the player
        //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        #endregion
    }
}
