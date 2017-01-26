using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    Texture2D crosshair;
    Rect position;
	// Use this for initialization
	void Start () {
        crosshair=Resources.Load<Texture2D>("eotech_a65_popup");
	}
	
	// Update is called once per frame
	void Update () {
        position = new Rect(Input.mousePosition.x-(crosshair.width/2), -(Input.mousePosition.y- (crosshair.height / 2)), crosshair.width, crosshair.height);
	}

    private void OnGUI()
    {
        GUI.DrawTexture(position, crosshair);
    }
}
