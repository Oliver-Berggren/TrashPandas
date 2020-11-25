using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HexMap : MonoBehaviour
{
    // Tile prefab type
    public GameObject tile;
    float sideLength;

    // Display Parameters
    public float spacing;

    // Placement unit values
    float xOffset; 
    float yOffset;

    // Map Container
    Dictionary<Vector2, GameObject> tileMap = new Dictionary<Vector2, GameObject>();

    // Map layout in text file form
    public TextAsset mapLayout;

    void Start()
    {
        List<List<int>> mapRaw = textToLayout();
        // Calculate length of one side(same as distance from center point to corner point)
        sideLength = tile.GetComponent<Renderer>().bounds.size.z / 2 + spacing;

        // Calculate placement values
        yOffset = (3 * sideLength) / 2;
        xOffset = Mathf.Sqrt(3) * (sideLength);

        // Spawn Map tiles
        for(int y = 0; y < mapRaw.Count; ++y)
        {
            float lineOffset = (y % 2) * (xOffset / 2);
            for(int x = 0; x < mapRaw[y].Count; ++x)
            {
                // none: 0, empty: 1, dump: 2, den: 3, gas: 4, hole: 5, trash: 6
                if(mapRaw[y][x] != 0) // tile drawn
                {
                    GameObject newTile = Instantiate(tile, new Vector3(x * xOffset + lineOffset, 0, y * yOffset), Quaternion.identity);
                    newTile.transform.parent = this.transform;
                    newTile.name = "( " + x + " , " + y + " )";
                    tileMap[new Vector2(x,y)] = newTile;

                    if(mapRaw[y][x] == 6) // empty tile with trash placed
                    {
                        newTile.GetComponent<TileInfo>().init(6, null, new Vector2(x,y));
                    }
                    else // static or empty tile
                    {
                        newTile.GetComponent<TileInfo>().init(mapRaw[y][x], null, new Vector2(x,y));
                    }
                } // otherwise empty space with no tile
            }
        }
    }

    // Returns hex coordinate approximation of world pos
    public Vector2 worldToHex(Vector3 worldPos)
    {
        int y = (int)Mathf.Floor((worldPos.y + sideLength) / yOffset);
        int x = (int)Mathf.Floor((worldPos.y + (y % 2) * (xOffset / 2)) / xOffset);

        return new Vector2(x,y);
    }

    // Returns world position of hex coordinate
    public Vector3 hexToWorld(Vector2 hexPos)
    {
        return new Vector3(hexPos.x * xOffset + (hexPos.y % 2) * (xOffset / 2), 0, hexPos.y * yOffset);
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
    GameObject getTile(Vector2 loc)
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
}
