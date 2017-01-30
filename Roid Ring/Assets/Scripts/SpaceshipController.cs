using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceshipController : NetworkBehaviour
{
    public int shipForwardSpeed = 500;
    public int shipForwardThrust = 100;
    public int shipBackwardSpeed  = 200;
    public int shipBackwardThrust = 80;
    public int shipSideSpeed = 200;
    public int shipSideThrust = 80;
    public float killSpeed = 0.1f;

    public GameObject projectile;
    private int thrustMultiplier = 20;
    private Rigidbody rb;
    private Camera cam;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cam = transform.GetChild(2).GetComponent<Camera>();

        if(isLocalPlayer)
        {
            cam.enabled = true;
        }
        else
        {
            cam.enabled = false;
        }
    }

    void FixedUpdate()
    {
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        if(isLocalPlayer)
        {
            int vertThrust, horizThrust;
            vertThrust = horizThrust = 0;

            if(Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    vertThrust = shipForwardThrust;
                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    vertThrust = shipBackwardThrust;
                }

                rb.AddForce(transform.forward * Input.GetAxis("Vertical") * vertThrust * thrustMultiplier);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipForwardSpeed / 3.6f);
            }

            if(Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    horizThrust = shipForwardThrust;
                }

                if (Input.GetAxis("Horizontal") < 0)
                {
                    horizThrust = shipBackwardThrust;
                }

                rb.AddForce(transform.right * Input.GetAxis("Horizontal") * horizThrust * thrustMultiplier);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipSideSpeed / 3.6f);
            }

            rb.velocity *= killSpeed;

            if(Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(new Vector3(0, -1, 0));
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 1, 0));
            }
        }
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject temp = Instantiate(projectile);
        temp.transform.position = transform.GetChild(1).GetChild(0).position;
        temp.transform.rotation = Quaternion.LookRotation(transform.GetChild(1).GetChild(0).forward);
        NetworkServer.Spawn(temp);
        Destroy(temp, 5.0f);
    }
}