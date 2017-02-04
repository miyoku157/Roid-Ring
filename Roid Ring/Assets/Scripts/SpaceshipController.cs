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

    public GameObject shieldEffect;
    public Transform shieldCoreTransform;
    public GameObject projectile;
    public List<GameObject> sternThrusters, portThrusters, starboardThrusters, bowThrusters;

    private float thrusterStartingSpeed = 0.03f;
    private float thrusterKillingSpeed = 0.1f;
    private int thrustMultiplier = 20;
    private Vector3 shieldOffset;
    private Rigidbody rb;
    private Camera cam;
    private ShieldBehavior shieldScript;
    private float sternThrustVisual, portThrustVisual, starboardThrustVisual, bowThrustVisual;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cam = transform.GetChild(2).GetComponent<Camera>();
        GameObject shield = Instantiate(shieldEffect);
        shieldScript = shield.GetComponent<ShieldBehavior>();
        shieldScript.setPos(transform.rotation, shieldCoreTransform.position);

        if (isLocalPlayer)
        {
            shield.transform.GetChild(0).gameObject.layer = 13;
            cam.enabled = true;
        }
        else
        {
            shield.transform.GetChild(0).gameObject.layer = 17;
            cam.enabled = false;
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, -1.55f, transform.position.z);
        shieldScript.setPos(transform.rotation, shieldCoreTransform.position);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);

        //Debug.Log(bowThrustVisual);

        foreach (GameObject go in bowThrusters)
        {
            ParticleSystem.MainModule ps = go.GetComponent<ParticleSystem>().main;
            ps.startSpeed = -15 * bowThrustVisual;
        }
    }

    void FixedUpdate()
    {
        if(!isLocalPlayer) return;

        if(isClient)
        {
            int vertThrust, horizThrust;
            vertThrust = horizThrust = 0;

            if (Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    vertThrust = shipForwardThrust;
                    bowThrustVisual += thrusterStartingSpeed;
                    sternThrustVisual -= thrusterKillingSpeed;
                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    vertThrust = shipBackwardThrust;
                    sternThrustVisual += thrusterStartingSpeed;
                    bowThrustVisual -= thrusterKillingSpeed;
                }

                CmdMovevertic(vertThrust);
            }
            else
            {
                bowThrustVisual -= thrusterKillingSpeed;
                sternThrustVisual -= thrusterKillingSpeed;
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    horizThrust = shipForwardThrust;
                    portThrustVisual += thrusterStartingSpeed;
                    starboardThrustVisual -= thrusterKillingSpeed;
                }

                if (Input.GetAxis("Horizontal") < 0)
                {
                    horizThrust = shipBackwardThrust;
                    starboardThrustVisual += thrusterStartingSpeed;
                    portThrustVisual -= thrusterKillingSpeed;
                }

                CmdMovehoriz(horizThrust);
            }
            else
            {
                portThrustVisual -= thrusterKillingSpeed;
                starboardThrustVisual -= thrusterKillingSpeed;
            }

            checkThrustVisuals();
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

    void checkThrustVisuals()
    {
        bowThrustVisual = bowThrustVisual > 1 ? 1 : bowThrustVisual;
        bowThrustVisual = bowThrustVisual < 0 ? 0 : bowThrustVisual;

    }

   [ClientRpc]
    public void RpcMovevertic(int vertThrust)
    {
        rb.AddForce(transform.forward * Input.GetAxis("Vertical") * vertThrust * thrustMultiplier);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipForwardSpeed / 3.6f);
    }
    [ClientRpc]
    public void RpcMovehoriz(int horizThrust)
    {
        rb.AddForce(transform.right * Input.GetAxis("Horizontal") * horizThrust * thrustMultiplier);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipSideSpeed / 3.6f);
    }

    [Command]
    public void CmdMovevertic(int vertThrust)
    {
        rb.AddForce(transform.forward * Input.GetAxis("Vertical") * vertThrust * thrustMultiplier);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipForwardSpeed / 3.6f);
        RpcMovevertic(vertThrust);
    }
    [Command]
    public void CmdMovehoriz(int horizThrust)
    {
        rb.AddForce(transform.right * Input.GetAxis("Horizontal") * horizThrust * thrustMultiplier);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, shipSideSpeed / 3.6f);
        RpcMovehoriz(horizThrust);
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject temp = Instantiate(projectile);
        temp.layer = 13;
        temp.transform.position = transform.GetChild(1).GetChild(0).position;
        temp.transform.rotation = Quaternion.LookRotation(transform.GetChild(1).GetChild(0).forward);
        NetworkServer.Spawn(temp);
        Destroy(temp, 5.0f);
    }
}