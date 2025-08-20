using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : TileData
{
    public TilePlaceOnTower _tilePlaceOnTower;
    
    public GameObject[] rotatedPrefabs;
    public TileDirector tileDirector;
    
    public Dictionary<int, BlockInfo[]> blockInfos = new Dictionary<int, BlockInfo[]>();
    
    public Dictionary<SpriteRenderer, int> originBlockOrder = new Dictionary<SpriteRenderer, int>();
    private bool originOrderInitialized = false;
    
    protected override void Awake()
    {
        base.Awake();
        _tilePlaceOnTower = GetComponent<TilePlaceOnTower>();
    }

    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        CacheOriginOrders();
        SetBlockInfos();
        UpdateSpriteOrder();
    }

    public override void UpdateMapping(Vector2 pos)
    {
        base.UpdateMapping(pos);
    }

    private void SetBlockInfos()
    {
        for (int index = 0; index < rotatedPrefabs.Length; index++)
        {
            BlockInfo[] blockInfoArray = rotatedPrefabs[index].GetComponentsInChildren<BlockInfo>(true);
            blockInfos.Add(index, blockInfoArray);
        }
    }

    public void UpdateWorldLevel(int level)
    {
        for (int index = 0; index < blockInfos.Count; index++)
        {
            foreach (var blockInfo in blockInfos[index])
            {
                blockInfo.UpdateWorldLevel(level);
            }
        }
    }
    
    public void UpdateSpriteOrder()
    {
        foreach (var blockInfo in blockInfos)
        {
            BlockInfo[] biArray = blockInfo.Value;
            foreach (var bi in biArray)
            {
                SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in spriteRenderers)
                {
                    bool isTower = sr.GetComponent<Tower1>() != null;

                    if (isTower)
                    {
                        int towerOrder = 1000 + (tileIndex * 10);
                        sr.sortingOrder = towerOrder;
                    }
                    else
                    {
                        int baseOrder = originBlockOrder.ContainsKey(sr) ? originBlockOrder[sr] : 0;
                        sr.sortingOrder = baseOrder + (tileIndex * 10) - 1000;    
                    }
                }
            }
        }
    }
    
    // 최초 블럭 sortingOrder저장
    private void CacheOriginOrders()
    {
        if (originOrderInitialized) return;

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originBlockOrder.Clear();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            originBlockOrder[sr] = sr.sortingOrder;
        }
        
        originOrderInitialized = true;
    }
    
}
