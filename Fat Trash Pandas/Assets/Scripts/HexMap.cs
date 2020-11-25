using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Tile prefab type
    public GameObject tile;
    float sideLength;

    // Dimensions
    public int width;
    public int height;
    public float spacing;

    // Placement unit values
    float xOffset; 
    float yOffset;

    // Map Container
    Dictionary<Vector2, GameObject> tileMap = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        // Calculate length of one side(same as distance from center point to corner point)
        sideLength = tile.GetComponent<Renderer>().bounds.size.x / 2;

        // Calculate placement values
        yOffset = (3 * sideLength) / 2;
        xOffset = Mathf.Sqrt(3) * (sideLength + spacing);

        // Spawn Map tiles
        for(int y = 0; y < height; ++y)
        {
            float lineOffset = (y % 2) * (xOffset / 2);
            for(int x = 0; x < width; ++x)
            {
                GameObject newTile = Instantiate(tile, new Vector3(x * xOffset + lineOffset, 0, y * yOffset), Quaternion.identity);
                newTile.transform.parent = this.transform;
                newTile.name = "( " + x + " , " + y + " )";
                newTile.GetComponent<TileInfo>().init(-1, null, new Vector2(x,y));
                tileMap[new Vector2(x,y)] = newTile;
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

    // public List<TileInfo> getNeighbors(Vector2 tile)
    // {
// 
    // }
}
