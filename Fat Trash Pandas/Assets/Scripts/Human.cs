using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : PlayerClass
{
    void Start()
    {
        hexLocation = HexMap.instance.getDump();
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
    
    public bool near_gas;
    public bool near_raccoon;
    public bool near_dump;
    public bool has_gas;

    public void use_gas(){
        this.steps += 2;
        has_gas = false;
    }

    public void get_gas(){
        has_gas = true;
    }

    public void remove_poop(List<Vector2> neighbors){
        // foreach (Vector2 pos in neighbors) {
        //     int type = instance.getTileType(pos); (get the tile type)
        //     if (type == 7){
        //         instance.removePiece(pos) (remove the poop gamepiece)
        //         change the tile type to 1 (empty)
        //     }
        // }
    }

    public void scare_raccoon(Raccoon rac, List<Vector2> neighbors){
        // if (rac.trash > 0){
        //     
        //
        
            // List<Vector2> empty = new List<Vector2>();
            // foreach (Vector2 pos in neighbors){
            //     int type = instance.getTileType(pos);
            //     if (type == 1){
            //         empty.Add(pos);
            //     }
            // }
            // Random rnd = new Random();
            // int index = rnd.Next(empty.Count);

            //instantiate new trash game obj
            //GameObject trash = Instantiate(trashPiecePrefab, Vector2.zero, Quaternion.identity);
            //instance.addPiece(empty[index], trash);
            //newTile.GetComponent<TileInfo>().tileType = 6;
        // }
    }


    override public void near_dropoff()
    {

    }
}
