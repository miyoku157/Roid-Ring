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
        Vector2 pos = ScreenToGUIPoint(new Vector2(Input.mousePosition.x - (crosshair.width / 2), (Input.mousePosition.y + crosshair.height/2)));
        position = new Rect(pos.x,pos.y, crosshair.width, crosshair.height);
	}

    private void OnGUI()
    {
        GUI.DrawTexture(position, crosshair);
    }

    private Vector2 ScreenToGUIPoint(Vector2 input)
    {
        return new Vector2(input.x,Screen.height-input.y);
    }
}
