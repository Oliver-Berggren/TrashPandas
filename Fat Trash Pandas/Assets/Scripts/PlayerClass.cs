using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerClass : MonoBehaviour
{
    // Action Variables
    public bool near_trash;
    public bool near_Opponent;
    public int steps;
    public int trash;
    public int maxNumSteps;

    // Movement variables
    Dictionary<Vector2, int> possibleMoves; // (hex coordinate, cost)
    Dictionary<Vector2, Vector2> prevTile; // previous tile, for pathing
    List<Vector2> path; // ordered list of tiles to travel through
    public float moveSpeed;
    bool isMoving = false;

    //human
    public bool near_gas;
    public bool near_raccoon;
    public bool has_gas;
    public bool near_dump;

    //raccoon
    public bool near_tunnel;
    public bool near_human;
    public int poop;
    public bool near_den;
    public bool is_raccoon;

    //location of player
    public Vector2 hexLocation;

    public GameObject UImanager;
    public GameObject gameManager;
    public GameModeManager game;
    public UiManager ui;

    // modes
    public bool move_mode;
    public bool poop_mode;
    public bool tunnel_mode;

    void Awake() {
        ui = UImanager.GetComponent<UiManager>();
        game = gameManager.GetComponent<GameModeManager>();
    }
    
    void Update()
    {
        if(isMoving)
        {

        }
    }

    public void move(){
        PlayerController.instance.startListening(tryMove);
        possibleMoves = HexMap.instance.getPossibleMoves(hexLocation, steps, out prevTile);
    }

    // Modes
    public void end_turn(){
        PlayerController.instance.stopListening();
        move_mode = false;

        List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
        Debug.Log("position:" + hexLocation);
        Debug.Log("neighbors");
        foreach (Vector2 pos in neighbors) {
            Debug.Log(pos);

            if (HexMap.instance.getPiece(pos) == ui.player3){
                ui.raccoon.scare();
                break;
            }
        }

        steps = maxNumSteps;
    }

    public void pick_up(){
        if(near_trash){
            near_trash = false;

            List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
            Vector2 trashLoc;
            foreach (Vector2 pos in neighbors) {
                if (HexMap.instance.getTileType(pos) == 6){
                    trashLoc = pos;
                    Destroy(HexMap.instance.getPiece(trashLoc));
                    HexMap.instance.removePiece(trashLoc);
                    HexMap.instance.setTileType(trashLoc, 1);
                    break;
                }
            }

            ++trash;
            game.end();
            ui.endTurnButton();
        }
    }

    public void drop_off(){
        if (near_dump){
            game.setDump();
        } else if (near_den){
            game.setDen(trash);
        }
        trash = 0;
        game.end();
        ui.endTurnButton();
    }

    void tryMove(Vector2 loc)
    {
        if(possibleMoves.ContainsKey(loc))
        {
            HexMap.instance.removePiece(hexLocation);
            moveAnim(loc);
            HexMap.instance.addPiece(loc, gameObject);
            hexLocation = loc;
            steps -= possibleMoves[loc];
            PlayerController.instance.stopListening();

            move_mode = false;

            ui.updateUI();
        }

            List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
            foreach (Vector2 pos in neighbors) {
                int type = HexMap.instance.getTileType(pos);
                switch(type){
                    //dump
                    case 2:
                        near_dump = true;
                        break;
                    //den
                    case 3:
                        near_den = true;
                        break;
                    //gas station
                    case 4:
                        near_gas = true;
                        break;
                    //trash
                    case 6:
                        near_trash = true;
                        break;
                }
            }

            //tunnel
            if (HexMap.instance.getTileType(hexLocation) == 5){
                near_tunnel = true;
            }

        ui.updateUI();
    }

    void moveAnim(Vector2 loc)
    {
        isMoving = true;
        path = new List<Vector2>();
        
        // Setup path
        Vector2 currLoc = loc; // tracking current spot stepping back from destination to start
        while(currLoc != hexLocation)
        {
            path.Insert(0, currLoc);
            currLoc = prevTile[currLoc];
        }
        path.Insert(0, currLoc);
    }
}
