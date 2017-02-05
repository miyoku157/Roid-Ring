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
    public List<GameObject> sternThrusters, portThrusters, starboardThrusters, bowThrusters, portRotationThrusters, starboardRotationThrusters;

    private float thrusterStartingSpeed = 0.03f;
    private float thrusterKillingSpeed = 0.1f;
    private int thrustMultiplier = 20;
    private Vector3 shieldOffset;
    private Rigidbody rb;
    private Camera cam;
    private ShieldBehavior shieldScript;
    private float sternThrustVisual, portThrustVisual, starboardThrustVisual, bowThrustVisual, portRotationVisual, starboardRotationVisual;
    private bool isRotating;
    private ParticleSystem ps;
    private ParticleSystem.MainModule psMain;
    private ParticleSystem.EmissionModule psEmission;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cam = transform.GetChild(2).GetComponent<Camera>();
        GameObject shield = Instantiate(shieldEffect);
        shieldScript = shield.GetComponent<ShieldBehavior>();
        shieldScript.setPos(transform.rotation, shieldCoreTransform.position);

        if(isLocalPlayer)
        {
            gameObject.layer = 13;
            shield.transform.GetChild(0).gameObject.layer = 13;
            cam.enabled = true;
        }
        else
        {
            gameObject.layer = 14;
            shield.transform.GetChild(0).gameObject.layer = 14;
            cam.enabled = false;
        }

        sternThrustVisual = portThrustVisual = starboardThrustVisual = bowThrustVisual = 0;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, -1.55f, transform.position.z);
        shieldScript.setPos(transform.rotation, shieldCoreTransform.position);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
        CmdEmitter();
    }

    void Emitter()
    {
        foreach(GameObject go in bowThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(bowThrustVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * bowThrustVisual;
            }
        }

        foreach(GameObject go in sternThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(sternThrustVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * sternThrustVisual;
            }
        }

        foreach(GameObject go in portThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(portThrustVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * portThrustVisual;
            }
        }

        foreach(GameObject go in starboardThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(starboardThrustVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * starboardThrustVisual;
            }
        }

        /*foreach(GameObject go in starboardRotationThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(starboardRotationVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * starboardRotationVisual;
            }
        }

        foreach(GameObject go in portRotationThrusters)
        {
            ps = go.GetComponent<ParticleSystem>();
            psMain = ps.main;
            psEmission = ps.emission;

            if(portRotationVisual < 0.1)
            {
                psEmission.enabled = false;
            }
            else
            {
                psEmission.enabled = true;
                psMain.startSpeed = -15 * portRotationVisual;
            }
        }*/
    }
    [Command]
    void CmdEmitter()
    {
        //Emitter();
        RpcEmitter();
    }
    [ClientRpc]
    void RpcEmitter()
    {
        Emitter();
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (isClient)
        {
            isRotating = false;
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
                portRotationVisual += thrusterStartingSpeed;
                starboardRotationVisual -= thrusterKillingSpeed;
                //isRotating = true;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 1, 0));
                starboardRotationVisual += thrusterStartingSpeed;
                portRotationVisual -= thrusterKillingSpeed;
                //isRotating = true;
            }
            else
            {
                portRotationVisual -= thrusterKillingSpeed;
                starboardRotationVisual -= thrusterKillingSpeed;
            }
        }
    }

    void checkThrustVisuals()
    {
        bowThrustVisual = bowThrustVisual > 1 ? 1 : bowThrustVisual;
        bowThrustVisual = bowThrustVisual < 0 ? 0 : bowThrustVisual;
        sternThrustVisual = sternThrustVisual > 1 ? 1 : sternThrustVisual;
        sternThrustVisual = sternThrustVisual < 0 ? 0 : sternThrustVisual;
        portThrustVisual = portThrustVisual > 1 ? 1 : portThrustVisual;
        portThrustVisual = portThrustVisual < 0 ? 0 : portThrustVisual;
        starboardThrustVisual = starboardThrustVisual > 1 ? 1 : starboardThrustVisual;
        starboardThrustVisual = starboardThrustVisual < 0 ? 0 : starboardThrustVisual;
        portRotationVisual = portRotationVisual > 1 ? 1 : portRotationVisual;
        portRotationVisual = portRotationVisual < 0 ? 0 : portRotationVisual;
        starboardRotationVisual = starboardRotationVisual > 1 ? 1 : starboardRotationVisual;
        starboardRotationVisual = starboardRotationVisual < 0 ? 0 : starboardRotationVisual;
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

            if (isLocalPlayer)
            {
                temp.layer = 13;
            }
            else
            {
                temp.layer = 14;
            }

            temp.transform.position = transform.GetChild(1).GetChild(0).position;
            temp.transform.rotation = Quaternion.LookRotation(transform.GetChild(1).GetChild(0).forward);
            NetworkServer.Spawn(temp);
        RpcSpawn(temp);
            Destroy(temp, 5.0f);
    }

    [ClientRpc]
    void RpcSpawn(GameObject temp)
    {
        if (isLocalPlayer)
        {
            temp.layer = 13;
        }
        else
        {
            temp.layer = 14;
        }

        temp.transform.position = transform.GetChild(1).GetChild(0).position;
        temp.transform.rotation = Quaternion.LookRotation(transform.GetChild(1).GetChild(0).forward);
    }
}