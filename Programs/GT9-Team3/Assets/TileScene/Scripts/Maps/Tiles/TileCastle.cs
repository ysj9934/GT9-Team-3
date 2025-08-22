using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCastle : TileData
{
    public BlockInfo[] blockInfos = new BlockInfo[9];

    public Dictionary<SpriteRenderer, int> originBlockOrder = new Dictionary<SpriteRenderer, int>();
    private bool originOrderInitialized = false;

    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        SetBlockInfos();
        CacheOriginOrders();
        UpdateSpriteOrder();
    }

    public void InitializeTemp(Vector2 pos)
    {
        base.Initialize(pos);
        UpdateSpriteOrder();
    }
    
    private void SetBlockInfos()
    {
        blockInfos = GetComponentsInChildren<BlockInfo>();
    }
    
    public override void UpdateWorldLevel(int level)
    {
        foreach (var blockInfo in blockInfos)
        {
            blockInfo.UpdateWorldLevel(level);
        }
    }
    
    private void UpdateSpriteOrder()
    { 
        //SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        //foreach (SpriteRenderer sr in spriteRenderers)
        //{
        //    sr.sortingOrder = sr.sortingOrder + (tileIndex * 10) - 1000;
        //}

        foreach (var bi in blockInfos)
        {
            SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var sr in spriteRenderers)
            {
                int baseOrder = originBlockOrder.ContainsKey(sr) ? originBlockOrder[sr] : 0;
                sr.sortingOrder = baseOrder + (tileIndex * 10) - 1000;
            }
        }
    }

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
