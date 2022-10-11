using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
    {
        FlyingEnemy enemy = other.GetComponent<FlyingEnemy>();

        if(enemy != null)
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
    }
}