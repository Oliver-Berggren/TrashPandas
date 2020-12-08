using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : PlayerClass
{
    void Start()
    {
        hexLocation = HexMap.instance.getDen();
    }

    // Update is called once per frame
    void Update()
    {
        //if in move mode,
        //highlight moveable hexes
        //do the stuff in the playercontroller.cs thing
        //move()
        //subtract that from steps if they click on a valid hex
    }

    public bool near_tunnel;
    public bool near_human;
    public bool near_den;
    public int poop;

    public void use_poop(List<Vector2> neighbors){
        // if (poop > 0){
        //     List<Vector2> empty = new List<Vector2>();

        //     foreach (Vector2 pos in neighbors){
        //         int type = instance.getTileType(pos);

        //         if (type == 1){
        //             empty.Add(pos);
        //         }
        //     }

            // instantiate new poop game obj
            // GameObject poop = Instantiate(poopPrefab, Vector2.zero, Quaternion.identity);
            // instance.addPiece(empty[index], poop);
            // newTile.GetComponent<TileInfo>().tileType = 7;

        //     --poop;
        // }
    }

    override public void near_dropoff()
    {

    }
}
