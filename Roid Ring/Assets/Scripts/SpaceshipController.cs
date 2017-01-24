using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public int shipForwardSpeed = 500;
    public int shipForwardThrust = 100;
    public int shipBackwardSpeed  = 200;
    public int shipBackwardThrust = 80;
    public float killSpeed = 0.1f;

    private int thrustMultiplier = 20;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        int genThrust = 0;

        if(Input.GetAxis("Vertical") != 0)
        {
            if(Input.GetAxis("Vertical") > 0)
            {
                genThrust = shipForwardThrust;
            }

            if(Input.GetAxis("Vertical") < 0)
            {
                genThrust = shipBackwardThrust;
            }

            rb.AddForce(Vector3.forward * Input.GetAxis("Vertical") * genThrust * thrustMultiplier);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipForwardSpeed / 3.6f); //.sqrMagnitude

        }  
        else
        {
            if(Input.GetAxis("Vertical") == 0)
            {
                rb.velocity *= killSpeed;
            }
        }
    }
}