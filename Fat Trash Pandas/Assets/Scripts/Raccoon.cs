using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

    public bool near_trash;
    public bool near_tunnel;
    public bool near_human;
    public bool near_den;
    public int steps;
    public int poop;
    public int trash;
    
    //modes
    public bool move_mode;

    public void move(){
        // if (move_mode){
        //     //move somewhere
        // }
        // steps -= 1;
    }

    public void end_turn(){ 
        move_mode = false;
        if (near_human){
            return;
        }
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
        if (near_den){
            //drop off
            trash = 0;
        }
        end_turn();
    }

    public void use_poop(){

    }

    public void scare(){ 
        //make raccoon have a status lose_trash??
    }
}
