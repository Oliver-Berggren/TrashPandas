using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerClass : MonoBehaviour
{
    // Action Variables
    public bool near_trash;
    public bool near_dropOff;
    public bool near_Opponent;
    public int steps;
    public int trash;
    public int maxNumSteps;
    public Dictionary<Vector2, int> possibleMoves; // (hex coordinate, cost)
    
    public Vector2 hexLocation;

    // modes
    public bool move_mode;

    public void move(){
        PlayerController.instance.startListening(tryMove);

        possibleMoves = HexMap.instance.getPossibleMoves(hexLocation, steps, false);
    }

    // Modes
    public void end_turn(){ 
        move_mode = false;
        // if (near_human){
        //     return;
        // }
        //switch game mode
        //step reset

        PlayerController.instance.stopListening();
        steps = maxNumSteps;
    }

    public void pick_up(){
        if(near_trash){
            //pick up
            near_trash = false;
        }
        end_turn();
    }

    public void drop_off(){
        // if (near_den){
        //     //drop off
        //     trash = 0;
        // }
        end_turn();
    }

    void tryMove(Vector2 loc)
    {
        if(possibleMoves.ContainsKey(loc))
        {
            HexMap.instance.removePiece(hexLocation);
            HexMap.instance.addPiece(loc, gameObject);
            hexLocation = loc;
            steps -= possibleMoves[loc];
        }
    }

    abstract public void near_dropoff();
}
