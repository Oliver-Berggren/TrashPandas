using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button endTurn;
    List<string> players = new List<string>();
    int playerIndex = 0;

    void Start()
    {
        
        //whoever starts can be decided here or elsewhere, players will just need to enter their names
       //and the controlling method can simply call the following method

       /*public void addPlayer(string name)
        {
            players.Add(name);
        }
        */

        //INITIALISING THE LIST WITH ONE RACCOON AND TWO HUMANS
        //Temporary initialisation for testing
        players.Add("R0");
        players.Add("H0");
        players.Add("H1");

        //logging before end is triggered
        endTurn.onClick.AddListener(delegate {endMessage(players[playerIndex]); });
        endTurn.onClick.AddListener(end);

    }

    //Ends user turn
    void end()
    {
       //update points

       //check win conditions

       //next player in queue: x%y will always be between zero and one less than y
       playerIndex = (playerIndex + 1) % players.Count;
    }

    void endMessage(string name)
    {
        Debug.Log(name+"'s turn has ended");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
