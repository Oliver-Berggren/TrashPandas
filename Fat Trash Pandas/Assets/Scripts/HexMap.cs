﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
    Vector2 worldToHex(Vector3 worldPos)
        Takes a world position, flattens it to the xz plane, and returns
        the closest hex's coordinate.

    Vector3 hexToWorld(Vector2 hexPos)
        Takes a hex tile coordinate and returns the world position of the
        center of the hex, with y = 0.

    List<Vector2> getNeighbors(Vector2 loc)
        Returns the hex coordinates of the 6 or less tiles touching the
        tile at the given hex coordinate. Returns all valid tile types,
        whether traversable or not.
    
    int getTileType(Vector2 tile) // none: 0, empty: 1, dump: 2, den: 3, gas: 4, hole: 5, trash: 6
        Returns the int type of the tile at the given hex coordinate.
        Note - empty tiles with a player pawn on them will return empty type.
    
    bool isTraversable(Vector2 tile)
        Returns bool of whether tile at given hex coordinate is traversable
        or not. Note - empty tiles with a player pawn on them will return
        false.
    
    void addPiece(Vector2 loc, GameObject obj)
        Adds the given GameObject to the tile at the specified hex coordinate
        by moving the object to the correct place and assigning it as
        occupant of the tile.

    GameObject removePiece(Vector2 loc)
        Sets occupant of tile at specified hex coordinate to null and returns
        reference to the GameObject that was removed from the tile. Note - 
        does not move the GameObject, and will return null and do nothing
        if tile has no occupant. Can be nested with addPiece in order to move
        player pawn from one tile to another immediately.

    List<Vector2> getTunnels()
        Returns the hex coordinates of all tunnels on the map, for use with 
        traveling between tunnels.

    Vector3 getMapCenter()
        Returns the world position of the center of the map (for camera
        rotation)
*/

public class HexMap : MonoBehaviour
{
    // Tile prefab type
    float sideLength;
    float tileHeight;

    // Tile prefabs
    public GameObject emptyTilePrefab;
    public GameObject dumpTilePrefab;
    public GameObject denTilePrefab;
    public GameObject gasTilePrefab;
    public GameObject holeTilePrefab;
    public GameObject trashPiecePrefab;

    // Materials for changing highlight
    public Material emptyTileDefault;
    public Material emptyTileHighlight;
    public Material tunnelTileHexDefault;
    public Material tunnelTileDefault;
    public Material tunnelTileHexHighlight;
    public Material tunnelTileHighlight;

    // Display Parameters
    public float spacing;

    // Placement unit values
    public float xOffset; 
    public float yOffset;

    // Map Containers
    Dictionary<Vector2, GameObject> tileMap = new Dictionary<Vector2, GameObject>();
    List<Vector2> tunnels = new List<Vector2>();
    List<Vector2> highlighted = new List<Vector2>();
    Vector2 den;
    Vector2 dump;

    // Map layout in text file form
    public TextAsset mapLayout;

    // Static game map instance
    public static HexMap instance;

    // Misc
    public Vector3 mapCenter;
    public Vector3 bounds;

    void Awake()
    {
        instance = this.GetComponent<HexMap>();

        List<List<int>> mapRaw = textToLayout();

        Renderer rend = emptyTilePrefab.GetComponentsInChildren<Renderer>()[0];
        // Calculate length of one side(same as distance from center point to corner point)
        sideLength = rend.bounds.size.x * rend.transform.localScale.x * 2 + spacing;

        // Calculate height of tile (to determine y offset of placement)
        tileHeight = rend.bounds.size.y * rend.transform.localScale.y * 2;

        // Calculate placement values
        xOffset = (3 * sideLength) / 2;
        yOffset = Mathf.Sqrt(3) * (sideLength);

        // Calculate map dimension variables
        bounds = new Vector3();
        bounds.x = xOffset * (mapRaw[0].Count);
        bounds.z = yOffset * (mapRaw.Count);
        mapCenter = new Vector3(xOffset * (mapRaw[0].Count - 1) / 2, 0, yOffset * (mapRaw.Count - 1) / 2);

        // Spawn Map tiles
        for(int y = 0; y < mapRaw.Count; ++y)
        {
            for(int x = 0; x < mapRaw[y].Count; ++x)
            {
                // none: 0, empty: 1, dump: 2, den: 3, gas: 4, hole: 5, trash: 6
                if(mapRaw[y][x] != 0) // tile drawn
                {
                    instance.addTile(new Vector2(x, y), mapRaw[y][x]);
                } // otherwise empty space with no tile
            }
        }
    }

    // Returns hex coordinate approximation of world pos
    public Vector2 worldToHex(Vector3 worldPos)
    {
        int x = (int)Mathf.Floor((worldPos.x + sideLength) / xOffset);
        int y = (int)Mathf.Floor((worldPos.z) / yOffset);

        return new Vector2(x,y);
    }

    // Returns world position of hex coordinate
    public Vector3 hexToWorld(Vector2 hexPos)
    {
        return new Vector3(hexPos.x * xOffset, 0, hexPos.y * yOffset + (hexPos.x % 2) * (yOffset / 2));
    }

    // Makes 2d list of map with tile types from text file
    List<List<int>> textToLayout()
    {
        List<List<int>> map = new List<List<int>>();
        map.Add(new List<int>());

        int x = 0;
        int y = 0;
        for(int i = 0; i < mapLayout.text.Length; ++i)
        {
            int tileValue = mapLayout.text[i] - '0';
            if(tileValue >= 0 && tileValue <= 6)
            {
                ++x;
                map[y].Add(tileValue);
            }
            else if(mapLayout.text[i] == '\n')
            {
                ++y;
                x = 0;
                map.Add(new List<int>());
            }
        }
        return map;
    }

    // Returns tile at given hex coordinate
    public GameObject getTile(Vector2 loc)
    {
        return tileMap[loc];
    }

    // Returns list of all neighboring tiles
    public List<Vector2> getNeighbors(Vector2 loc)
    {
        List<Vector2> neighbors = new List<Vector2>();
        Vector2 possibleNeighbor;

        for(int x = (int)loc.x - 1; x <= loc.x + 1; x += 2) // two above and two below
        {
            possibleNeighbor = new Vector2(x,loc.y);
            if(tileMap.ContainsKey(possibleNeighbor))
            {
                neighbors.Add(possibleNeighbor);
            }

            possibleNeighbor = new Vector2(x,loc.y - 1 + ((loc.x % 2) * 2));
            if(tileMap.ContainsKey(possibleNeighbor))
            {
                neighbors.Add(possibleNeighbor);
            }
        }

        // one above and one below
        possibleNeighbor = loc;
        possibleNeighbor.y += 1;
        if(tileMap.ContainsKey(possibleNeighbor))
        {
            neighbors.Add(possibleNeighbor);
        }

        possibleNeighbor.y -= 2;
        if(tileMap.ContainsKey(possibleNeighbor))
        {
            neighbors.Add(possibleNeighbor);
        }

        return neighbors;
    }

    public int getTileType(Vector2 tile)
    {
        return tileMap[tile].GetComponent<TileInfo>().tileType;
    }

    public void setTileType(Vector2 tile, int val)
    {
        tileMap[tile].GetComponent<TileInfo>().tileType = val;
    }

    // Returns whether or not the tile at the given hex coordinate is traversable (empty tile or not)
    public bool isTraversable(Vector2 tile)
    {
        return tileMap[tile].GetComponent<TileInfo>().occupant == null ? true : false;
    }

    // Adds a tile of given type to given hex grid location
    void addTile(Vector2 loc, int type)
    {
        GameObject newTile;
        switch(type)
        {
            case 2: // Dump
                newTile = Instantiate(dumpTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                dump = loc;
                break;
            case 3: // Den
                newTile = Instantiate(denTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                den = loc;
                break;
            case 4: // Gas
                newTile = Instantiate(gasTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                break;
            case 5: // Hole
                newTile = Instantiate(holeTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                tunnels.Add(loc);
                break;
            case 6: // Trash
                newTile = Instantiate(emptyTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                GameObject trash = Instantiate(trashPiecePrefab, Vector2.zero, Quaternion.identity);
                instance.addPiece(loc, trash);
                trash.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                newTile.GetComponent<TileInfo>().tileType = 6;
                break;
            default: // Empty
                newTile = Instantiate(emptyTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                break;
        }

        newTile.GetComponent<TileInfo>().hexCoordinate = loc;
        newTile.transform.parent = this.transform;
        newTile.name = "( " + loc.x + " , " + loc.y + " )";
    }

    // Adds a piece to a given empty tile
    public void addPiece(Vector2 loc, GameObject obj)
    {
        Vector3 pos = instance.hexToWorld(loc);
        obj.transform.position = pos;
        tileMap[loc].GetComponent<TileInfo>().occupant = obj;
    }

    // Removes a piece from a given empty tile
    public GameObject removePiece(Vector2 loc)
    {
        if(tileMap.ContainsKey(loc))
        {
            GameObject obj = tileMap[loc].transform.GetComponent<TileInfo>().occupant;
            tileMap[loc].transform.GetComponent<TileInfo>().occupant = null;
            return obj;
        }
        return null;
    }

    public GameObject getPiece(Vector2 loc){
        if(tileMap.ContainsKey(loc))
        {
            return tileMap[loc].transform.GetComponent<TileInfo>().occupant;
        }
        return null;
    }

    // Returns the locations of all tunnels
    public List<Vector2> getTunnels()
    {
        return tunnels;
    }

    // Returns the location of den
    public Vector2 getDen()
    {
        return den;
    }

    // Returns the location of den
    public Vector2 getDump()
    {
        return dump;
    }

    // Returns the world location of center of map
    public Vector3 getMapCenter()
    {
        return mapCenter;
    }

    // Returns coordinates and number of moves to get to all tiles that can be reached in set number of steps
    public Dictionary<Vector2, int> getPossibleMoves(Vector2 loc, int maxMoves, out Dictionary<Vector2, Vector2> prev)
    {
        Dictionary<Vector2, int> possibleMoves = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> dist = new Dictionary<Vector2, int>();
        List<Vector2> queue = new List<Vector2>();
        prev = new Dictionary<Vector2, Vector2>();
        dist.Add(loc, 0);
        queue.Add(loc);
        prev.Add(loc, loc);

        while(queue.Count > 0)
        {
            Vector2 visit = queue[0];
            List<Vector2> neighbors = getNeighbors(visit);
            if(isTraversable(visit))
            {
                possibleMoves.Add(visit, dist[visit]);
            }

            foreach(Vector2 neighbor in neighbors)
            {
                int prevDist = int.MaxValue;
                if(dist.ContainsKey(neighbor))
                {
                    prevDist = dist[neighbor];
                }
                if(isTraversable(neighbor) && dist[visit] + 1 <= maxMoves && dist[visit] + 1 < prevDist)
                {
                    if(dist.ContainsKey(neighbor))
                    {
                        dist[neighbor] = dist[visit] + 1;
                        prev[neighbor] = visit;
                    }
                    else
                    {
                        dist.Add(neighbor, dist[visit] + 1);
                        queue.Add(neighbor);
                        prev.Add(neighbor, visit);
                    }
                }
            }
            queue.RemoveAt(0);
        }

        return possibleMoves;
    }

    // "Highlights" specified tiles to show possible interactable tiles
    public void highlightTiles(List<Vector2> tiles)
    {
        foreach(Vector2 tile in tiles)
        {
            foreach(Renderer rend in getTile(tile).GetComponentsInChildren<Renderer>())
            {
                if(rend.gameObject.name == "HexDefault") // Empty hex
                {
                    rend.material = emptyTileHighlight;
                }
                else if(rend.gameObject.name == "Hex") // Tunnel base hex
                {
                    rend.material = tunnelTileHexHighlight;
                }
                else if(rend.gameObject.name == "TunnelTop") // Tunnel top
                {
                    rend.material = tunnelTileHighlight;
                }
            }
            highlighted.Add(tile);
        }
    }

    // Resets all highlighted tiles to default
    public void unHighlightTiles()
    {
        foreach(Vector2 tile in highlighted)
        {
            foreach(Renderer rend in getTile(tile).GetComponentsInChildren<Renderer>())
            {
                if(rend.gameObject.name == "HexDefault") // Empty hex
                {
                    rend.material = emptyTileDefault;
                }
                else if(rend.gameObject.name == "Hex") // Tunnel base hex
                {
                    rend.material = tunnelTileHexDefault;
                }
                else if(rend.gameObject.name == "TunnelTop") // Tunnel top
                {
                    rend.material = tunnelTileDefault;
                }
            }
        }
        highlighted.Clear();
    }

    public bool isTile(Vector2 loc)
    {
        return tileMap.ContainsKey(loc);
    }
}
