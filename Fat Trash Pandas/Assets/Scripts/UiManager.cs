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


    //gamemode manager
    public GameObject man;
    GameModeManager manager;

    //human
    public GameObject humanPanel;
    public Button gasButton;
    public Button getGasButton;
    public Button removePoopButton;
    public Button humanPickUp;
    public Button humanDropOff;
    public Button humanMove;
    //inv
    public RawImage boost;
    public RawImage humanTrashInv;

    //raccoon
    public GameObject raccoonPanel;
    public Button poopButton;
    public Button tunnelButton;
    public Button raccoonPickUp;
    public Button raccoonDropOff;
    public Button raccoonMove;
    //inv
    public RawImage racpoop1;
    public RawImage racpoop2;
    public RawImage racpoop3;
    public RawImage racpoop4;
    public RawImage racpoop5;
    public RawImage ractrash1;
    public RawImage ractrash2;
    public RawImage ractrash3;
    public RawImage ractrash4;
    
    //display texts
    public Text displayTurn;
    public Text numSteps;

    //scores
    public Text humanScore;
    public Text raccoonScore;
    public Text mode;

    //winning screens
    public GameObject humanWin;
    public GameObject raccoonWin;

    void Start() {
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
        humanScore.text = "" + manager.dump;
        raccoonScore.text = "" + manager.den;
        //mode
        mode.text = "Mode: ";

        //human turn
        if (manager.playerIndex < 2){
            //human vs racccoon action
            humanPanel.SetActive(true);
            raccoonPanel.SetActive(false);

            if (manager.playerIndex == 0){
                //text
                displayTurn.text = "HUMAN 1 TURN";
                numSteps.text = "" + human1.steps;
                
                if (human1.move_mode){
                    mode.text += "Moving";
                } else {
                    mode.text += "Waiting";
                }

                //inventory
                if (human1.has_gas)
                    boost.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                else
                    boost.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

                if (human1.trash > 0)
                    humanTrashInv.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                else
                    humanTrashInv.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

                if (human1.move_mode){
                    gasButton.interactable = false;
                    getGasButton.interactable = false;
                    removePoopButton.interactable = false;
                    humanPickUp.interactable = false;
                    humanDropOff.interactable = false;
                } else {
                    //buttons
                    //gas button
                    gasButton.interactable = human1.has_gas;
                    getGasButton.interactable = human1.near_gas;
                    humanMove.interactable = true;

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
                        humanPickUp.interactable = true;
                    } else {
                        humanPickUp.interactable = false;
                    }
                    
                    //drop off trash
                    if (human1.near_dump && human1.trash > 0) {
                        humanDropOff.interactable = true;
                    } else {
                        humanDropOff.interactable = false;
                    }
                }

            } else {
                displayTurn.text = "HUMAN 2 TURN";
                numSteps.text = "" + human2.steps;

                if (human2.move_mode){
                    mode.text += "Moving";
                } else {
                    mode.text += "Waiting";
                }

                //inventory
                if (human2.has_gas)
                    boost.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                else
                    boost.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

                if (human2.trash > 0)
                    humanTrashInv.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                else
                    humanTrashInv.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

                if (human2.move_mode){
                    gasButton.interactable = false;
                    getGasButton.interactable = false;
                    removePoopButton.interactable = false;
                    humanPickUp.interactable = false;
                    humanDropOff.interactable = false;
                } else {
                    //buttons
                    //gas button
                    gasButton.interactable = human2.has_gas;
                    getGasButton.interactable = human2.near_gas;
                    humanMove.interactable = true;

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
                        humanPickUp.interactable = true;
                    } else {
                        humanPickUp.interactable = false;
                    }
                    
                    //drop off trash
                    if (human2.near_dump && human2.trash > 0) {
                        humanDropOff.interactable = true;
                    } else {
                        humanDropOff.interactable = false;
                    }
                }
            }
        } else {
            //human vs racccoon action
            humanPanel.SetActive(false);
            raccoonPanel.SetActive(true);

            displayTurn.text = "RACCOON TURN";
            numSteps.text = "" + raccoon.steps;

            //inventory
            switch(raccoon.poop){
                case 1:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 2:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 3:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 4:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 5:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    break;
                default:
                    racpoop1.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop2.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    racpoop5.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
            }

            switch(raccoon.trash){
                case 1:
                    ractrash1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash2.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 2:
                    ractrash1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 3:
                    ractrash1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash3.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
                case 4:
                    ractrash1.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash2.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash3.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    ractrash4.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    break;
                default:
                    ractrash1.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash2.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash3.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    ractrash4.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                    break;
            }

            if (raccoon.move_mode){
                mode.text += "Moving";
            } else if (raccoon.poop_mode){
                mode.text += "Pooping";
            } else if (raccoon.tunnel_mode){
                mode.text += "Tunneling";
            } else {
                mode.text += "Waiting";
            }


            if (raccoon.move_mode){
                raccoonPickUp.interactable = false;
                raccoonDropOff.interactable = false;
                poopButton.interactable = false;
                tunnelButton.interactable = false;
            } else if (raccoon.poop_mode){
                raccoonPickUp.interactable = false;
                raccoonDropOff.interactable = false;
                tunnelButton.interactable = false;
                raccoonMove.interactable = false;
            } else if (raccoon.tunnel_mode){
                raccoonPickUp.interactable = false;
                raccoonDropOff.interactable = false;
                poopButton.interactable = false;
                raccoonMove.interactable = false;
            } else {
                //buttons
                //poop
                if(raccoon.poop > 0){
                    poopButton.interactable = true;
                } else {
                    poopButton.interactable = false;
                }

                tunnelButton.interactable = raccoon.near_tunnel;
                raccoonMove.interactable = true;

                //pick up trash
                if (raccoon.near_trash && raccoon.trash < 5){
                    raccoonPickUp.interactable = true;
                } else {
                    raccoonPickUp.interactable = false;
                }

                //drop off trash
                if (raccoon.near_den && raccoon.trash > 0){
                    raccoonDropOff.interactable = true;
                } else {
                    raccoonDropOff.interactable = false;
                }
            }
        }
    }

    public void moveButton(){
        //depending on the current player
        //if available steps, enable button
        //it no more available steps, disable button

        if (manager.playerIndex == 0){
            if (!human1.move_mode){
                human1.move();
                human1.move_mode = true;
            } else {
                human1.move_mode = false;
                HexMap.instance.unHighlightTiles();
            }
        } else if (manager.playerIndex == 1){
            if (!human2.move_mode){
                human2.move();
                human2.move_mode = true;
            } else {
                human2.move_mode = false;
                HexMap.instance.unHighlightTiles();
            }
        } else {
            if (!raccoon.move_mode){
                raccoon.move();
                raccoon.move_mode = true;
            } else {
                raccoon.move_mode = false;
                HexMap.instance.unHighlightTiles();
            }
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
            if(raccoon.poop < 5)
            {
                ++raccoon.poop;
            }
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
        HexMap.instance.unHighlightTiles();
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
            if (!raccoon.poop_mode){
                raccoon.use_poop();
                raccoon.poop_mode = true;
            } else {
                raccoon.poop_mode = false;
                HexMap.instance.unHighlightTiles();
            }
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

    public void tunnel(){
        if (!raccoon.tunnel_mode){
            raccoon.useTunnel();
            raccoon.tunnel_mode = true;
        } else {
            raccoon.tunnel_mode = false;
            HexMap.instance.unHighlightTiles();
        }
        updateUI();
    }

    //TODO: Make the endGame function display a message on the screen to declare the winner
    public void endGame(string team)
    {
        poopButton.interactable = false;
        gasButton.interactable = false;
        humanPickUp.interactable = false;
        humanDropOff.interactable = false;
        humanMove.interactable = false;
        raccoonPickUp.interactable = false;
        raccoonDropOff.interactable = false;
        raccoonMove.interactable = false;

        if(team.Equals("den")) {
            raccoonWin.SetActive(true);
        } else {
            humanWin.SetActive(true);
        }
    }
}
