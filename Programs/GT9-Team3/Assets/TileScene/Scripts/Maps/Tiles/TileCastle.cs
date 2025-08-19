using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCastle : TileData
{
    public BlockInfo2[] blockInfos = new BlockInfo2[9];
    
    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        
        UpdateSpriteOrder();
    }
    
    protected override void UpdateMapping(Vector2 pos)
    {
        base.UpdateMapping(pos);

        // _tileManager.tileMap[base.originTileRow, base.originTileCol] = null;
        // _tileManager.tileMap[base.tileRow, base.tileCol] = this;
    }
    
    public void UpdateWorldLevel(int level)
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
