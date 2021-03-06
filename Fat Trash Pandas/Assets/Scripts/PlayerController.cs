﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton reference
    public static PlayerController instance;

    // Action to be called with result (set by caller when listen enabled)
    System.Action<Vector2> callAction;

    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<PlayerController>();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit tileHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out tileHit))
            {
                callAction(HexMap.instance.worldToHex(tileHit.transform.position));
            }
        }
    }

    // Waits for user to click on tile, then calls action function with hex coordinate of tile clicked
    public void startListening(System.Action<Vector2> action)
    {
        callAction = action;
        this.enabled = true;
    }

    public void stopListening()
    {
        callAction = null;
        this.enabled = false;
    }
}
