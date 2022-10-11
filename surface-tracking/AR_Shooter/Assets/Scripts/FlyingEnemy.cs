using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public int health = 3;
    public int damage = 1;

    public float moveSpeed;
    public float attackRange;

    void Update ()
    {
        // calculate the distance between us and the core
        float dist = Vector3.Distance(transform.position, ShooterCore.instance.transform.position);
        
        // if we're out of the attack range, move towards the core
        if(dist > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, ShooterCore.instance.transform.position, moveSpeed * Time.deltaTime);
        }
        // otherwise attack the core
        else
        {
            ShooterCore.instance.TakeDamage(damage);
            Destroy(gameObject);
        }

        // rotate to face the core
        transform.LookAt(ShooterCore.instance.transform);
    }

    // called when we're hit by a player's projectile
    public void TakeDamage ()
    {
        health--;

        if(health == 0)
            Destroy(gameObject);
    }
}