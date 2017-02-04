using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Damageable : NetworkBehaviour
{
    public int maxHull, maxShield;
    public float shieldRechargeRate = 1;
    public bool isPlayer;

    private int hull, shield;
    private int temp;

    void Start()
    {
        shield = maxShield;
        hull = maxHull;

        if(maxShield > 0)
        {
            StartCoroutine("ShieldRecharge");
        }
    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        temp = 0;
        shield -= amount;
            
        if(shield < 0)
        {
            temp = -shield;
            shield = 0;
            hull -= temp;
        }

        if(hull < 0)
        {
            Destroy(this.gameObject);
        }
    }

    [Command]
    private void CmdKnockBack(GameObject coll)
    {
        Vector3 forceVec = -coll.gameObject.GetComponent<Rigidbody>().velocity * 100;
        coll.gameObject.GetComponent<Rigidbody>().AddForce(forceVec, ForceMode.Impulse);
        RpcKnockBack(coll);
    }

    [ClientRpc]
    private void RpcKnockBack(GameObject coll)
    {
        Vector3 forceVec = -coll.gameObject.GetComponent<Rigidbody>().velocity * 100;
        coll.gameObject.GetComponent<Rigidbody>().AddForce(forceVec, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Shot")
        {
            CmdTakeDamage(10);
            Destroy(coll.gameObject);
        }
        else if(coll.gameObject.tag == "Player")
        {
            CmdKnockBack(coll.gameObject);
        }
    }

    IEnumerator ShieldRecharge()
    {
        while (true)
        {
            if (shield < maxShield)
            {
                shield += 1;
            }

            yield return new WaitForSeconds(1 / shieldRechargeRate / 6.66f);
        }
    }

    public int getHull()
    {
        return hull;
    }

    public int getShield()
    {
        return shield;
    }
}