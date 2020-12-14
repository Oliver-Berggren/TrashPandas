using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    Dictionary<Vector2, Vector2> prevTiles; // previous tile, for pathing
    List<Vector2> path = new List<Vector2>(); // ordered list of tiles to travel through
    int currStep; // index of current tile being moved to in path
    static float moveDuration = 0.5f;
    static float turnDuration = 0.2f;
    Quaternion startRot;
    Quaternion endRot;
    float moveElapsed = 0;
    float turnElapsed = 0;
    bool isMoving = false;
    bool isTurning = false;

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
            moveElapsed += Time.deltaTime;
            // hasn't reached next tile yet
            if(moveElapsed < moveDuration)
            {
                Vector3 newPos;
                Vector3 startPos = HexMap.instance.hexToWorld(path[currStep - 1]);
                Vector3 endPos = HexMap.instance.hexToWorld(path[currStep]);
                newPos.x = Mathf.SmoothStep(startPos.x, endPos.x, moveElapsed / moveDuration);
                newPos.y = Mathf.SmoothStep(startPos.y, endPos.y, moveElapsed / moveDuration);
                newPos.z = Mathf.SmoothStep(startPos.z, endPos.z, moveElapsed / moveDuration);
                transform.position = newPos;
            }
            else // reached next tile
            {
                // Debug.Log("Tile Reached " + path[currStep]);
                ++currStep;
                moveElapsed = 0;
                if(currStep >= path.Count) // past end of path
                {
                    isMoving = false;
                    HexMap.instance.addPiece(hexLocation, this.gameObject);
                    HexMap.instance.unHighlightTiles();

                    if(is_raccoon)
                    {
                        AudioManager.instance.Stop("moveRaccoon");
                    }
                    else
                    {
                        AudioManager.instance.Stop("moveHuman");
                    }
                }
                else // more tiles on path
                {
                    isTurning = true;
                    isMoving = false;
                    startRot = transform.rotation;
                    endRot = Quaternion.LookRotation(HexMap.instance.getTile(path[currStep]).transform.position - transform.position);
                    if(!is_raccoon)
                    {
                        endRot *= Quaternion.Euler(0, -90, 0);
                    }
                    turnElapsed = 0;
                }
            }
        }
        else if(isTurning)
        {
            turnElapsed += Time.deltaTime;
            if(turnElapsed < turnDuration)
            {
                transform.rotation = Quaternion.Lerp(startRot, endRot, turnElapsed / turnDuration);
            }
            else
            {
                 // Debug.Log("Stopped turning " + hexLocation);
                 isTurning = false;
                 isMoving = true;
            }
        }
    }

    public void move(){
        PlayerController.instance.startListening(tryMove);
        possibleMoves = HexMap.instance.getPossibleMoves(hexLocation, steps, out prevTiles);
        HexMap.instance.highlightTiles(possibleMoves.Keys.ToList());
    }

    // Modes
    public void end_turn(){
        PlayerController.instance.stopListening();
        move_mode = false;

        List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
        // Debug.Log("position:" + hexLocation);
        // Debug.Log("neighbors");
        foreach (Vector2 pos in neighbors) {
            // Debug.Log(pos);

            if (HexMap.instance.getPiece(pos) == ui.player3){
                ui.raccoon.scare();
                break;
            }
        }

        steps = maxNumSteps;
        HexMap.instance.unHighlightTiles();
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
                    AudioManager.instance.Play("collectTrash");
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
        AudioManager.instance.Play("dumpTrash");
        game.end();
        ui.endTurnButton();
    }

    void tryMove(Vector2 loc)
    {
        if(possibleMoves.ContainsKey(loc))
        {
            if(HexMap.instance.getTileType(hexLocation) == 1 ||
               HexMap.instance.getTileType(hexLocation) == 5)
            {
                HexMap.instance.removePiece(hexLocation);
            }
            startMoveAnim(loc);
            hexLocation = loc;
            steps -= possibleMoves[loc];
            PlayerController.instance.stopListening();

            move_mode = false;

            updateNeighbors();
            if(is_raccoon)
            {
                AudioManager.instance.Play("moveRaccoon");
            }
            else
            {
                AudioManager.instance.Play("moveHuman");
            }
        }
        else if(HexMap.instance.isTile(loc))
        {
            AudioManager.instance.Play("invalidInput");
        }

        ui.updateUI();
    }

    void startMoveAnim(Vector2 loc)
    {
        isMoving = true;
        path = new List<Vector2>();
        
        // Setup path
        Vector2 currLoc = loc; // tracking current spot stepping back from destination to start
        while(currLoc != hexLocation)
        {
            path.Insert(0, currLoc);
            currLoc = prevTiles[currLoc];
        }
        path.Insert(0, currLoc);

        currStep = 1;
        moveElapsed = 0;
        transform.position = HexMap.instance.hexToWorld(hexLocation);
        isTurning = true;
        isMoving = false;
        startRot = transform.rotation;
        endRot = Quaternion.LookRotation(HexMap.instance.getTile(path[currStep]).transform.position - transform.position);
        if(!is_raccoon)
        {
            endRot *= Quaternion.Euler(0, -90, 0);
        }
        turnElapsed = 0;
        // Debug.Log("Move Start");
    }

    void updateNeighbors()
    {
        // Reset values to false
        near_dump = false;
        near_den = false;
        near_gas = false;
        near_trash = false;
        near_tunnel =false;

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
    }
}
