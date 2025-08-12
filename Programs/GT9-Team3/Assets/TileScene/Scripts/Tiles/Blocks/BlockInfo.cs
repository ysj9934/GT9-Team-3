using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    private TileRoad tileRoad;
    
    [SerializeField] public int blockSerialNumber;
    [SerializeField] private BlockCategory blockCategory;
    [SerializeField] private GameObject towerPrefab;
    private bool hasTower;

    private void Awake()
    {
        tileRoad = GetComponentInParent<TileRoad>();
    }

    public void CallNumber()
    {
        tileRoad.ReceiveBlockNumber(blockSerialNumber);
    }

    // 타워 신규 생성
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
