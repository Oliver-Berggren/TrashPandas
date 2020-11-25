using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int tileType;          // Type of base tile
    public GameObject occupant;   // Reference to token on tile, if there is one
    public Vector2 hexCoordinate; // Coordinate of hex tile in hex space

    public void init(int _tileType, GameObject _occupant, Vector2 _hexCoordinate)
    {
        tileType = _tileType;
        occupant = _occupant;
        hexCoordinate = _hexCoordinate;
    }
}
