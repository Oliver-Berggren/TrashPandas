using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControl : MonoBehaviour
{
    // Variables
    public float camRotSpeed; // Speed of camera rotation
    public GameObject cameraAnchor; // Anchor object for camera rotation

    void Start()
    {
        cameraAnchor.transform.position = HexMap.instance.getMapCenter();
        Camera.main.transform.parent = cameraAnchor.transform;
    }

    void Update()
    {
        // Rotate camera if right mouse down
        if (Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camRotSpeed, Vector3.up);
            cameraAnchor.transform.Rotate(0, Input.GetAxis("Mouse X") * camRotSpeed, 0);
        }
    }
}
