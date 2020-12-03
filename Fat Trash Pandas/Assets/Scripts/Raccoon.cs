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
        
    }

    bool near_trash;
    bool near_tunnel;
    bool near_human;
    bool near_den;
    public int steps;
    public bool has_gas;
    public bool has_trash;
    
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
        if (near_gas){
            has_gas = true;
        }
        if (near_human){
            return;
        }
        //switch game mode
        steps = /**/;  //step reset
    }

    public void pick_up(){
        if(near_trash){
            //pick up
            has_trash = true;
            near_trash = false;
        }
        end_turn();
    }

    public void drop_off(){
        if (near_dump){
            //drop off
            has_trash = false;
        }
        end_turn();
    }

    //human exclusive
    public void get_gas(){ 
        has_gas = true;
        end_turn();
    }

    public void use_gas(){
        this.steps += 2;
        has_gas = false;
    }

    public void scare(){ 
        //make raccoon have a status lose_trash??
    }
}
