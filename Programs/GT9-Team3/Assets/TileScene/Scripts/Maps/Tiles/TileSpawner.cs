using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : TileData
{
    public BlockInfo[] blockInfos = new BlockInfo[9];
    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        SetBlockInfos();
        UpdateSpriteOrder();
    }

    public override void UpdateMapping(Vector2 pos)
    {
        base.UpdateMapping(pos);
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
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = sr.sortingOrder + (tileIndex * 10) - 1000;
        }
    }
}
