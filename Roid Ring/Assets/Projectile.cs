using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
	void Start ()
    {

        if (isClient)
        {
            CmdFire();
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        Destroy(this.gameObject);
    }

    [Command]
    void CmdFire()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 300);
        RpcFire();
    }
    [ClientRpc]
    void RpcFire()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 300);
    }
}
