using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject projectile;
    public GameObject testCube;

    Vector3 mouse_pos, object_pos;
    Vector2 offset;
    float angle, parentAngle;
    
    void Start ()
    {
        if (transform.root.GetComponent<SpaceshipController>().isLocalPlayer)
        {
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

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject temp = Instantiate(projectile);
            temp.transform.position = transform.GetChild(0).position;
            temp.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 300);
            Destroy(temp, 5f);
        }
    }
}
