﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
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
    public bool near_gas;
    public bool near_raccoon;
    public bool near_dump;
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

        //call highlighting message here

        
    }

    public void end_turn(){ 
        move_mode = false;
        if (near_gas){
            has_gas = true;
        }
        if (near_raccoon){
            scare();
        }
        //switch game mode
        steps = 2;  //step reset
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

    public void use_gas(){
        this.steps += 2;
        has_gas = false;
    }

    public void scare(){ 
        //make raccoon have a status lose_trash??
    }



}
