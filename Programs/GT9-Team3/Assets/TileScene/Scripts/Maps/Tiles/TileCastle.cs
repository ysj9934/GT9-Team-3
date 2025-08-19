using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCastle : TileData
{
    public BlockInfo2[] blockInfos = new BlockInfo2[9];
    
    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        SetBlockInfos();
        UpdateSpriteOrder();
    }
    
    private void SetBlockInfos()
    {
        blockInfos = GetComponentsInChildren<BlockInfo2>();
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
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = sr.sortingOrder + (tileIndex * 10) - 1000;
        }
    }
}
