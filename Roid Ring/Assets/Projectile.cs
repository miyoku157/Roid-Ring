using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    // Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * 300);
    }

    // Update is called once per frame
}
