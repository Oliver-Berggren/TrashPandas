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

    // modes
    public bool move_mode;

    public void move(){
        // if (move_mode){
        //     //move somewhere
        // }
        // steps -= 1;
    }

    // Modes
    public void end_turn(){ 
        move_mode = false;
        // if (near_human){
        //     return;
        // }
        //switch game mode
        //step reset
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

    abstract public void near_dropoff();
}
