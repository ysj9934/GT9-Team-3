using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo1 : TileData
{
    public GameObject[] rotatePrefabs;
    public TileDirector tileDirector;
    
    public bool connectedUp;
    public bool connectedDown;
    public bool connectedLeft;
    public bool connectedRight;
    
    public Dictionary<int, BlockInfo2[]> blockInfos = new Dictionary<int, BlockInfo2[]>();
    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        // _tileManager.SetNeighbors();
        SetBlockInfos();
        UpdateSpriteOrder();
    }

    protected override void UpdateMapping(Vector2 pos)
    {
        base.UpdateMapping(pos);

    }

    private void SetBlockInfos()
    {
        for (int index = 0; index < rotatePrefabs.Length; index++)
        {
            BlockInfo2[] blockInfoArray = rotatePrefabs[index].GetComponentsInChildren<BlockInfo2>();
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
    
    private void UpdateSpriteOrder()
    { 
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = sr.sortingOrder + (tileIndex * 10) - 1000;
        }
    }
    
}
