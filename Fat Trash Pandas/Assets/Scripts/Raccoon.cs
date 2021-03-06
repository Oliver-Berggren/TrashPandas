﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : PlayerClass
{
    public GameObject poopPrefab;
    public GameObject trashPiecePrefab;
    List<Vector2> neighbors;

    void Start()
    {
        hexLocation = HexMap.instance.getDen();
        maxNumSteps = 4;
    }

    public void use_poop(){
        if (poop > 0){
            neighbors = HexMap.instance.getNeighbors(hexLocation);
            List<Vector2> empty = new List<Vector2>();

            foreach (Vector2 pos in neighbors){
                int type = HexMap.instance.getTileType(pos);

                if (type == 1){
                    empty.Add(pos);
                }
            }

            PlayerController.instance.startListening(tryPoop);
            HexMap.instance.highlightTiles(empty);
        }
    }

    public void scare(){
        if (trash > 0){
            List<Vector2> neighbors = HexMap.instance.getNeighbors(hexLocation);
            List<Vector2> empty = new List<Vector2>();
            foreach (Vector2 pos in neighbors){
                if (HexMap.instance.isTraversable(pos) && HexMap.instance.getTileType(pos) != 5){
                    empty.Add(pos);
                }
            }


            if(empty.Count > 0)
            {
                int index = Random.Range(0, empty.Count);
                
                GameObject trashPrefab = Instantiate(trashPiecePrefab, Vector2.zero, Quaternion.identity);
                HexMap.instance.addPiece(empty[index], trashPrefab);
                HexMap.instance.setTileType(empty[index], 6);

                --trash;
                ++steps;

                ui.updateUI();
            }
        }
        AudioManager.instance.Play("raccoonScared");
    }

    public void useTunnel(){
        PlayerController.instance.startListening(tryTunnel);
        HexMap.instance.highlightTiles(HexMap.instance.getTunnels());
    }

    void tryPoop(Vector2 loc){
        if (neighbors.Contains(loc) && HexMap.instance.getTileType(loc) == 1 &&
            HexMap.instance.isTraversable(loc)){
            GameObject poopObj = Instantiate(poopPrefab, Vector2.zero, Quaternion.identity);
            HexMap.instance.addPiece(loc, poopObj);
            HexMap.instance.setTileType(loc, 7);
            --poop;

            poop_mode = false;

            PlayerController.instance.stopListening();
            HexMap.instance.unHighlightTiles();
            AudioManager.instance.Play("placePoop");
        }

        ui.updateUI();
    }

    void tryTunnel(Vector2 loc){
        List<Vector2> tunnels = HexMap.instance.getTunnels();

        if (tunnels.Contains(loc)){
            HexMap.instance.removePiece(hexLocation);
            HexMap.instance.addPiece(loc, gameObject);
            hexLocation = loc;

            tunnel_mode = false;

            PlayerController.instance.stopListening();

            game.end();
            ui.endTurnButton();
            HexMap.instance.unHighlightTiles();
            AudioManager.instance.Play("useTunnel");
        }

        ui.updateUI();
    }
}
