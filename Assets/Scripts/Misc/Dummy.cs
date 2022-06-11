using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] float health = 100f;
   
    public void takeDamage(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            destroyDummy();
        }
    }

    public void destroyDummy()
    {
        Destroy(gameObject);
    }


}
