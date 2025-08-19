using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BlockInfo2 : MonoBehaviour
{
    [SerializeField] public BlockData _blockData;
    [SerializeField] public SpriteRenderer spriteRenderer;
    public int level = 1;
    

    public void UpdateWorldLevel(int level)
    {
        this.level = level;
        // if (_blockData.blockCategory == BlockCategory.None)
        // {
            spriteRenderer.sprite = _blockData.sprites[level - 1];    
        // }
    }
}
