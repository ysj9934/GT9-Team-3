using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField] private TileRoad _tile;
    [SerializeField] public bool connectedUp, connectedDown, connectedLeft, connectedRight;
    
    
    private void OnEnable()
    {
        _tile.connectedUp = connectedUp;
        _tile.connectedDown = connectedDown;
        _tile.connectedLeft = connectedLeft;
        _tile.connectedRight = connectedRight;
    }
}
