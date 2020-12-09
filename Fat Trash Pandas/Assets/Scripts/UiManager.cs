using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    //player objects
    public GameObject player1;
    public Human human1;
    public GameObject player2;
    public Human human2;
    public GameObject player3;
    public Raccoon raccoon;
    public GameObject man;
    GameModeManager manager;

    //action button
    public GameObject poop;
    Button poopButton;
    public Button pickUp;
    public Button dropOff;
    public Button move;

    //human
    public GameObject humanPanel;
    public Button gasButton;
    public Button getGasButton;
    public Button removePoopButton;
    
    //display texts
    public Text displayTurn;
    public Text numSteps;
    public Text inventory;
    public Text score;
    public Text mode;

    void Start() {
        poopButton = poop.GetComponent<Button>();

        human1 = player1.GetComponent<Human>();
        human2 = player2.GetComponent<Human>();

        raccoon = player3.GetComponent<Raccoon>();

        manager = man.GetComponent<GameModeManager>();

        updateUI();
    }

    public void updateUI(){
        //depending on the current player
        //update the enable human panel or raccoon panel
        //update the buttons if they need to be disabled/enabled

        //score
        score.text = "SCORE \n Humans: " + manager.dump + "\n Raccoon: " + manager.den;
        //mode
        mode.text = "Mode: ";

        //human turn
        if (manager.playerIndex < 2){
            //human vs racccoon action
            humanPanel.SetActive(true);
            poop.SetActive(false);

            if (manager.playerIndex == 0){
                //text
                displayTurn.text = "HUMAN 1 TURN";
                numSteps.text = "Steps: " + human1.steps;
                inventory.text = "Inventory: ";
                if (human1.has_gas)
                    inventory.text += "Gas ";
                    if (human1.trash > 0)
                        inventory.text += "and Trash";
                else if (human1.trash > 0)
                    inventory.text += "Trash";

                if (human1.move_mode){
                    mode.text += "Moving";
                } else {
                    mode.text += "Waiting";
                }

                //buttons
                //gas button
                gasButton.interactable = human1.has_gas;
                getGasButton.interactable = human1.near_gas;

                removePoopButton.interactable = false;
                List<Vector2> neighbors = HexMap.instance.getNeighbors(human1.hexLocation);
                foreach (Vector2 pos in neighbors) {
                    if (HexMap.instance.getTileType(pos) == 7){
                        removePoopButton.interactable = true;
                        break;
                    }
                }

                //pick up trash
                if (human1.near_trash && human1.trash < 1){
                    pickUp.interactable = true;
                } else {
                    pickUp.interactable = false;
                }
                
                //drop off trash
                if (human1.near_dump && human1.trash > 0) {
                    dropOff.interactable = true;
                } else {
                    dropOff.interactable = false;
                }

            } else {
                displayTurn.text = "HUMAN 2 TURN";
                numSteps.text = "Steps: " + human2.steps;
                inventory.text = "Inventory: ";
                if (human2.has_gas)
                    inventory.text += "Gas ";
                    if (human2.trash > 0)
                        inventory.text += "and Trash";
                else if (human2.trash > 0)
                    inventory.text += "Trash";

                if (human2.move_mode){
                    mode.text += "Moving";
                } else {
                    mode.text += "Waiting";
                }

                //buttons
                //gas button
                gasButton.interactable = human2.has_gas;
                getGasButton.interactable = human2.near_gas;

                removePoopButton.interactable = false;
                List<Vector2> neighbors = HexMap.instance.getNeighbors(human2.hexLocation);
                foreach (Vector2 pos in neighbors) {
                    if (HexMap.instance.getTileType(pos) == 7){
                        removePoopButton.interactable = true;
                        break;
                    }
                }

                //pick up trash
                if (human2.near_trash && human2.trash < 1){
                    pickUp.interactable = true;
                } else {
                    pickUp.interactable = false;
                }
                
                //drop off trash
                if (human2.near_dump && human2.trash > 0) {
                    dropOff.interactable = true;
                } else {
                    dropOff.interactable = false;
                }
            }
        } else {
            //human vs racccoon action
            humanPanel.SetActive(false);
            poop.SetActive(true);

            displayTurn.text = "RACCOON TURN";
            numSteps.text = "Steps: " + raccoon.steps;
            inventory.text = "Inventory: ";
            if (raccoon.poop > 0)
                inventory.text += "Poop= " + raccoon.poop;
                if (raccoon.trash > 0)
                    inventory.text += "and Trash= " + raccoon.trash;
            else if (raccoon.trash > 0)
                inventory.text += "Trash= " + raccoon.trash;

            if (raccoon.move_mode){
                mode.text += "Moving";
            } else if (raccoon.poop_mode){
                mode.text += "Pooping";
            } else {
                mode.text += "Waiting";
            }

            //buttons
            //poop
            if(raccoon.poop > 0){
                poopButton.interactable = true;
            } else {
                poopButton.interactable = false;
            }

            //pick up trash
            if (raccoon.near_trash && raccoon.trash < 5){
                pickUp.interactable = true;
            } else {
                pickUp.interactable = false;
            }

            //drop off trash
            if (raccoon.near_den && raccoon.trash > 0){
                dropOff.interactable = true;
            } else {
                dropOff.interactable = false;
            }
        }
    }

    public void moveButton(){
        //depending on the current player
        //if available steps, enable button
        //it no more available steps, disable button

        if (manager.playerIndex == 0){
            human1.move_mode = true;
            human1.move();
        } else if (manager.playerIndex == 1){
            human2.move_mode = true;
            human2.move();
        } else {
            raccoon.move_mode = true;
            raccoon.move();
        }

        updateUI();
    }

    public void pickUpButton(){
        //depending on the current player
        //enable button
        //otherwise disable
        //pick up trash near them
        //disable button

        if (manager.playerIndex == 0){
            human1.pick_up();
        } else if (manager.playerIndex == 1){
            human2.pick_up();
        } else {
            raccoon.pick_up();
        }

        updateUI();
    }

    public void dropOffButton(){
        //depending on the current player
        //if player is near dump/den
        //enable button
        //otherwise disable
        //drop off all trash
        //disable button

        if (manager.playerIndex == 0){
            human1.drop_off();
        } else if (manager.playerIndex == 1){
            human2.drop_off();
        } else {
            raccoon.drop_off();
        }

        updateUI();
    }

    public void endTurnButton(){
        //depending on the current player
        //end their turn
        //go to next player's turn

        if (manager.playerIndex == 1){
            human1.end_turn();
        } else if (manager.playerIndex == 2){
            human2.end_turn();
        } else {
            raccoon.maxNumSteps = 5 - raccoon.trash;
            raccoon.end_turn();
        }

        updateUI();
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

        if (manager.playerIndex == 0){
            human1.use_gas();
        } else if (manager.playerIndex == 1){
            human2.use_gas();
        } else {
            raccoon.poop_mode = true;
            raccoon.use_poop();
        }

        updateUI();
    }

    public void receiveGas(){
        if (manager.playerIndex == 0){
            human1.get_gas();
        } else {
            human2.get_gas();
        }
    }

    public void removePoop(){
        if (manager.playerIndex == 0){
            human1.remove_poop();
        } else {
            human2.remove_poop();
        }
    }

    //TODO: Make the endGame function display a message on the screen to declare the winner
    public void endGame(string team)
    {
        poopButton.interactable = false;
        gasButton.interactable = false;
        pickUp.interactable = false;
        dropOff.interactable = false;
        move.interactable = false;

        if(team.Equals("den")) {
            displayTurn.text = "Raccoon Wins";
            Debug.Log("Raccoon Wins");
        } else {
            displayTurn.text = "Humans Wins";
            Debug.Log("Humans Win");
        }
    }
}
