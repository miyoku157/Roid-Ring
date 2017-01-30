using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health;
    public bool isPlayer;

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Shot")
        {
            TakeDamage(10);
            Destroy(coll.gameObject);
        }
    }

    public int getHealth()
    {
        return health;
    }
}