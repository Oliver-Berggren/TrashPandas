﻿using System.Collections;
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
        // Rotate camera if right mouse down
        if (Input.GetMouseButton(1))
        {
            cameraAnchor.transform.Rotate(0, Input.GetAxis("Mouse X") * camRotSpeed * Time.deltaTime, 0);
        }
        // Move rotation point with middle mouse button
        if (Input.GetMouseButton(2)) 
        {
            Vector3 newPos = cameraAnchor.transform.position;
            newPos -= cameraAnchor.transform.forward * Input.GetAxis("Mouse Y") * camTranslateSpeed * Time.deltaTime;
            newPos -= cameraAnchor.transform.right * Input.GetAxis("Mouse X") * camTranslateSpeed * Time.deltaTime;
            cameraAnchor.transform.position = newPos;
        }
        // Zoom
        Camera.main.transform.position += Camera.main.transform.forward * Input.GetAxis("Mouse ScrollWheel") * camZoomSpeed * Time.deltaTime;
    }
}
