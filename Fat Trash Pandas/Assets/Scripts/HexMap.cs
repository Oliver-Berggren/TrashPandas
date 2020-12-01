using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    // Display Parameters
    public float spacing;

    // Placement unit values
    float xOffset; 
    float yOffset;

    // Map Container
    Dictionary<Vector2, GameObject> tileMap = new Dictionary<Vector2, GameObject>();

    // Map layout in text file form
    public TextAsset mapLayout;

    // Static game map instance
    public static HexMap instance;

    void Start()
    {
        instance = this.GetComponent<HexMap>();

        List<List<int>> mapRaw = textToLayout();

        // Calculate length of one side(same as distance from center point to corner point)
        sideLength = holeTilePrefab.GetComponent<Renderer>().bounds.size.x / 2 + spacing;

        // Calculate height of tile (to determine y offset of placement)
        tileHeight = emptyTilePrefab.GetComponent<Renderer>().bounds.size.y;

        // Calculate placement values
        xOffset = (3 * sideLength) / 2;
        yOffset = Mathf.Sqrt(3) * (sideLength);

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
    List<Vector2> getNeighbors(Vector2 loc)
    {
        List<Vector2> neighbors = new List<Vector2>();
        Vector2 possibleNeighbor;

        for(int y = (int)loc.y - 1; y <= loc.y + 1; y += 2) // two above and two below
        {
            possibleNeighbor = new Vector2(loc.x,y);
            if(tileMap.ContainsKey(possibleNeighbor))
            {
                neighbors.Add(possibleNeighbor);
            }

            possibleNeighbor = new Vector2(loc.x - 1 + ((loc.y % 2) * 2),y);
            if(tileMap.ContainsKey(possibleNeighbor))
            {
                neighbors.Add(possibleNeighbor);
            }
        }

        // two on either side
        possibleNeighbor = loc;
        possibleNeighbor.x += 1;
        if(tileMap.ContainsKey(possibleNeighbor))
        {
            neighbors.Add(possibleNeighbor);
        }

        possibleNeighbor.x -= 2;
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

    // Returns whether or not the tile at the given hex coordinate is traversable (empty tile or not)
    public bool isTraversable(Vector2 tile)
    {
        return tileMap[tile].GetComponent<TileInfo>().tileType == 1 ? true : false;
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
                break;
            case 3: // Den
                newTile = Instantiate(denTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                break;
            case 4: // Gas
                newTile = Instantiate(gasTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                break;
            case 5: // Hole
                newTile = Instantiate(holeTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                break;
            case 6: // Trash
                newTile = Instantiate(emptyTilePrefab, instance.hexToWorld(loc), Quaternion.identity);
                tileMap[loc] = newTile;
                GameObject trash = Instantiate(trashPiecePrefab, Vector2.zero, Quaternion.identity);
                instance.addPiece(loc, trash);
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
    void addPiece(Vector2 loc, GameObject obj)
    {
        Vector3 pos = instance.hexToWorld(loc);
        pos.y += tileHeight;
        obj.transform.position = pos;
        tileMap[loc].GetComponent<TileInfo>().occupant = obj;
    }

    // Removes a piece from a given empty tile
    GameObject removePiece(Vector2 loc)
    {
        GameObject obj = tileMap[loc].transform.GetComponent<TileInfo>().occupant;
        tileMap[loc].transform.GetComponent<TileInfo>().occupant = null;
        return obj;
    }
}
