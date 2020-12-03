using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject human1;
    public GameObject human2;
    public GameObject raccoon;
    public GameObject manager;

    public void updateUI(){
        //depending on the current player
        //update the enable human panel or raccoon panel
        //update the buttons if they need to be disabled/enabled
    }

    public void moveButton(){
        //depending on the current player
        //if available steps, enable button
        //it no more available steps, disable button
    }

    public void pickUpButton(){
        //depending on the current player
        //if player is near trash
        //enable button
        //otherwise disable
        //pick up trash near them
        //disable button
    }

    public void dropOffButton(){
        //depending on the current player
        //if player is near dump/den
        //enable button
        //otherwise disable
        //drop off all trash
        //disable button
    }

    public void endTurnButton(){
        //depending on the current player
        //end their turn
        //go to next player's turn
    }

    //gas for humans, poop for raccoon
    public void specialAction(){
        //depending on the current player
        //if human
        //has gas
        //enable button, otherwise disable
        //if raccoon
        //has poop
        //enable button, otherwisedisable
    }
}
