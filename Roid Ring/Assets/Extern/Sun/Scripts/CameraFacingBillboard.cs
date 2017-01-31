using UnityEngine;


public class CameraFacingBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform, transform.up);
        }
    }
}