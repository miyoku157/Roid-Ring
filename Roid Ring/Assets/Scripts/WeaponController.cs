using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform turretTr;
    public GameObject projectile;

    Vector3 mouse_pos;
    Vector3 object_pos;
    float angle;
    
    void Start ()
    {

	}
	
	void Update ()
    {
        /*var mousePos = Input.mousePosition;
        Debug.Log(mousePos);
        //mousePos.y = Camera.main.transform.position.y;
        var mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log(mouseWorld);
        var offset = new Vector3(mousePos.x - mouseWorld.x, 0, mousePos.z - mouseWorld.z);
        var angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
        transform.GetChild(0).localRotation = Quaternion.Euler((angle), 0, 0);

        Debug.DrawLine(Camera.main.transform.position, transform.GetChild(0).position, Color.red, 0.5f);
        Debug.DrawLine(Camera.main.transform.position, mouseWorld, Color.blue, 0.5f);*/

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject temp = Instantiate(projectile);
            temp.transform.position = transform.position;
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * 150);
        }
    }
}
