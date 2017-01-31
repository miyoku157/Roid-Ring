using System.Collections; using System.Collections.Generic; using UnityEngine;  public class ShieldBehavior : MonoBehaviour {     public void setPos(Quaternion _qu, Vector3 _vec)     {
        transform.position = _vec;
        transform.rotation = _qu;
    } }