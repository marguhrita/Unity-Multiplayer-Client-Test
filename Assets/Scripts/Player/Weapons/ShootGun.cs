using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour
{

    [SerializeField] Camera cam;

    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float cooldown = 1f;

    private bool canShoot = true;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {


        FindObjectOfType<AudioManager>().Play("Bark");
        canShoot = false;

        Invoke("resetShootCooldown", cooldown);

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit)) //Performs raycast on camera direction and outputs to "hit"
        {

            Dummy dummy = hit.transform.GetComponent<Dummy>();

            if (dummy != null)
            {
                dummy.takeDamage(damage);
            }

        }

    }

    void resetShootCooldown()
    {
        canShoot = true;
    }
}
