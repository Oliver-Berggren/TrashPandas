using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : PlayerClass
{
    public GameObject trashPiecePrefab;

    void Start()
    {
        hexLocation = HexMap.instance.getDump();
        maxNumSteps = 2;
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
        foreach (Vector2 pos in neighbors) {
            int type = HexMap.instance.getTileType(pos);
            if (type == 7){
                HexMap.instance.removePiece(pos);
                //HexMap.instance.tileMap[pos].GetComponent<TileInfo>().tileType = 1;
            }
        }

        end_turn();
    }

    public void scare_raccoon(Raccoon rac, List<Vector2> neighbors){
        if (rac.trash > 0){
            List<Vector2> empty = new List<Vector2>();
            foreach (Vector2 pos in neighbors){
                int type = HexMap.instance.getTileType(pos);
                if (type == 1){
                    empty.Add(pos);
                }
            }

            int index = Random.Range(0, empty.Count);

            GameObject trash = Instantiate(trashPiecePrefab, Vector2.zero, Quaternion.identity);
            HexMap.instance.addPiece(empty[index], trash);
            //HexMap.instance.tileMap[empty[index]].GetComponent<TileInfo>().tileType = 6;
        }
    }


    override public void near_dropoff()
    {

    }
}
