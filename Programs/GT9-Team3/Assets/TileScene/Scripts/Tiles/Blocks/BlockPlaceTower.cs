using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlaceTower : MonoBehaviour
{
    private TileRoad tileRoad;
    private BlockInfo blockInfo;
    [SerializeField] private GameObject towerPrefab;
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
        if (hasTower)
        {
            Debug.Log("Already has tower");
            return;
        }
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
        
        // 본인에게 설치
        GameObject go = Instantiate(towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        
        hasTower = true;
        Debug.Log("Placed tower");
    }
}
