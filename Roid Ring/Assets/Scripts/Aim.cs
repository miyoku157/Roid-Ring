using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Texture2D crosshair;
    Rect position;
    
	void Start ()
    {
        Cursor.visible = false;
	}
	void Update ()
    {
        position = new Rect(Input.mousePosition.x - (crosshair.width / 2), -(Input.mousePosition.y - crosshair.height), crosshair.width, crosshair.height);
	}

    private void OnGUI()
    {
        GUI.DrawTexture(position, crosshair);
    }
}
