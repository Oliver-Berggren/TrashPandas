using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit tileHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out tileHit))
        {
            Vector2 tile = HexMap.instance.worldToHex(tileHit.transform.position);
            int type = HexMap.instance.getTileType(tile);
            switch(type) // none: 0, empty: 1, dump: 2, den: 3, gas: 4, hole: 5, trash: 6
            {
                case 1:
                    Debug.Log("empty");
                    break;
                case 2:
                    Debug.Log("dump");
                    break;
                case 3:
                    Debug.Log("den");
                    break;
                case 4:
                    Debug.Log("gas");
                    break;
                case 5:
                    Debug.Log("hole");
                    break;
                case 6:
                    Debug.Log("trash");
                    break;
            }
        }
    }
}
