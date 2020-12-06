using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Control variables
    public float camRotSpeed; // Speed of camera rotation
    public GameObject cameraAnchor; // Anchor object for camera rotation

    // Start is called before the first frame update
    void Start()
    {
        cameraAnchor.transform.position = HexMap.instance.getMapCenter();
        Camera.main.transform.parent = cameraAnchor.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
                default:
                    Debug.Log(tile);
                    break;
            }
        }
        */

        // Rotate camera if right mouse down
        if (Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camRotSpeed, Vector3.up);
            cameraAnchor.transform.Rotate(0, Input.GetAxis("Mouse X") * camRotSpeed, 0);
        }
    }
}
