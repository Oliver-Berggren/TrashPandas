using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : PlayerClass
{

    // Update is called once per frame
    // void Update()
    // {
        //if in move mode,
        //highlight moveable hexes
        //do the stuff in the playercontroller.cs thing
        //move()
        //subtract that from steps if they click on a valid hex
    // }

    public bool near_tunnel;
    public bool near_human;
    public bool near_den;
    public int poop;

    // public void move(){
        // if (move_mode){
        //     //move somewhere
        // }
        // steps -= 1;
    // }

    public void use_poop(){

    }

    public void scare(){ 
        //make raccoon have a status lose_trash??
    }

    override public void near_dropoff()
    {

    }
}
