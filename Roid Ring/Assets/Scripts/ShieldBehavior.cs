using System.Collections; using System.Collections.Generic; using UnityEngine;  public class ShieldBehavior : MonoBehaviour {     private Transform tr;     private bool isSet = false;     private Vector3 offset;  	void Update()     {         if(isSet)         {
            transform.position = tr.position + offset;
            transform.rotation = tr.rotation;
        } 	}      public void Init(Transform _tr, Vector3 _offset)     {
        tr = _tr;
        offset = _offset;
        isSet = true;
    }      public void setPos(Transform _tr)     {
        tr = _tr;
    } }