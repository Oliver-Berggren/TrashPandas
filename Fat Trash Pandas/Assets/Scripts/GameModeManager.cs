using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button endTurn;
    public GameObject ui;
    List<string> players = new List<string>();
    public int playerIndex = 0;
    //initialising the base inventories
    public int den = 0, dump = 0;

    void Start()
    {
        //INITIALISING THE LIST WITH ONE RACCOON AND TWO HUMANS
        //Temporary initialisation for testing
        players.Add("H0");
        players.Add("H1");
        players.Add("R0");
    }

    //Ends user turn
    public void end()
    {
       //update points

       //check win conditions
        if(den == 5) 
        {   //send end game signals to playercontroller / ui manager
            ui.GetComponent<UiManager>().endGame("den");
        }
        else if(dump == 5)
        {
            ui.GetComponent<UiManager>().endGame("dump");
        }
        //next player in queue
        else
        {
            //next player in queue: x%y will always be between zero and one less than y
            endMessage();
            playerIndex = (playerIndex + 1) % players.Count;
            //signal playercontroller to pass controls to the current player
        }
    }

    //whoever starts can be decided here or elsewhere, players will just need to enter their names
    //and the controlling method can simply call the following method
    void addPlayer(string name)
    {
        players.Add(name);
    }

    void endMessage()
    {
        Debug.Log(players[playerIndex] +"'s turn has ended");
    }

    void setDen()
    {   
        ++den;
    }

    void setDump()
    {   
        ++dump;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TODO: drop off trash button
    
}
