using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : MonoBehaviour
{
    public GameObject projectile;
    public GameObject testCube;
    SpaceshipController sc;
    public int projectileSpeed = 600;
    public int energyRecoverySpeed = 1;
    public float firingCooldpwn = 0.20f;

    Vector3 mouse_pos, object_pos;
    Vector2 offset;
    private float angle, parentAngle, timeToCooldown;
    private int energy;
    
    void Start ()
    {
        sc = transform.root.GetComponent<SpaceshipController>();
        if (sc.isLocalPlayer)
        {
            testCube = GameObject.Find("Cube");
            StartCoroutine("EnergyRecovery");
            enabled = true;
        }
        else
        {
            enabled = false;
        }
    }
	
	void Update ()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 25;
        object_pos = Camera.main.WorldToScreenPoint(transform.position);

        // Test cube movement
        if(testCube != null)
            testCube.transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        offset = new Vector2(mouse_pos.x - object_pos.x, mouse_pos.y - object_pos.y);
        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        parentAngle = transform.root.eulerAngles.y;

        transform.localRotation = Quaternion.Euler(-90, -180, -angle + 90 - parentAngle);

        if(Input.GetKey(KeyCode.Mouse0) && energy > 5 && timeToCooldown <= 0)
        {
            sc.CmdSpawn();
            energy -= 5;
            Destroy(temp, 3f);
            timeToCooldown = firingCooldpwn;
        }
        else
        {
            timeToCooldown -= Time.deltaTime;
        }
    }

    IEnumerator EnergyRecovery()
    {
        while(true)
        {
            if(energy < 100)
            {
                energy += 1;
            }

            yield return new WaitForSeconds(1 / energyRecoverySpeed / 6.66f);
        }
    }

    public int getEnergy()
    {
        return energy;
    }
}
