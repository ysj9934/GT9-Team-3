using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlaceTower : MonoBehaviour
{
    private TileRoad tileRoad;
    private BlockInfo blockInfo;
    
    public bool hasTower;

    private void Awake()
    {
        tileRoad = GetComponentInParent<TileRoad>();
        blockInfo = GetComponent<BlockInfo>();
    }

    public void CallNumber()
    {
        tileRoad.ReceiveBlockNumber(blockInfo.blockSerialNumber);
        PlaceTower();
    }

    public void PlaceTower()
    {
        
    }
}
