using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : PlayerClass
{
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

    public void use_gas(){
        this.steps += 2;
        has_gas = false;
    }

    public void get_gas(){
        has_gas = true;

        game.end();
        ui.endTurnButton();
    }

    public void remove_poop(){
        List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
        foreach (Vector2 pos in neighbors) {
            int type = HexMap.instance.getTileType(pos);
            if (type == 7){
                Destroy(HexMap.instance.getPiece(pos));
                HexMap.instance.removePiece(pos);
                HexMap.instance.setTileType(pos, 1);
            }
        }

        game.end();
        ui.endTurnButton();
    }
}
