using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControl : MonoBehaviour
{
    // Variables
    public float camRotSpeed; // Speed of camera rotation
    public float camZoomSpeed;
    public float camTranslateSpeed;
    public GameObject cameraAnchor; // Anchor object for camera rotation

    void Start()
    {
        cameraAnchor.transform.position = HexMap.instance.getMapCenter();
        Camera.main.transform.parent = cameraAnchor.transform;
    }

    void Update()
    {
        Vector3 anchorPos = cameraAnchor.transform.position;
        Vector3 camPos = Camera.main.transform.position;

        // Rotate camera if right mouse down
        if (Input.GetMouseButton(1))
        {
            cameraAnchor.transform.Rotate(0, Input.GetAxis("Mouse X") * camRotSpeed * Time.deltaTime, 0);
        }

        // Move rotation point with middle mouse button
        if (Input.GetMouseButton(0) && !PlayerController.instance.enabled) 
        {
            anchorPos -= cameraAnchor.transform.forward * Input.GetAxis("Mouse Y") * camTranslateSpeed * Time.deltaTime;
            anchorPos -= cameraAnchor.transform.right * Input.GetAxis("Mouse X") * camTranslateSpeed * Time.deltaTime;
        }

        // Zoom
        camPos += Camera.main.transform.forward * Input.GetAxis("Mouse ScrollWheel") * camZoomSpeed * Time.deltaTime;

        // Apply new positions
        anchorPos.x = Mathf.Clamp(anchorPos.x, 0, HexMap.instance.bounds.x);
        anchorPos.z = Mathf.Clamp(anchorPos.z, 0, HexMap.instance.bounds.z);
        if(camPos.y > 2.5f && camPos.y < HexMap.instance.bounds.y)
        {
            Camera.main.transform.position = camPos;
        }
        cameraAnchor.transform.position = anchorPos;
    }
}
