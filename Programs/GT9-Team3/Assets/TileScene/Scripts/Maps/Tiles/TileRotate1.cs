using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate1 : MonoBehaviour
{
    private TileInfo1 _tileInfo;
    
    private void Start()
    {
        _tileInfo = GetComponent<TileInfo1>();
    }

    // 다음 각도의 맵 타일 생성하기
    public void RotateLeft()
    {
        int newIndex = ((int)_tileInfo.tileDirector - 1 + 4) % 4;
        
        RotateTile(newIndex);
    }

    public void RotateRight()
    {
        int newIndex = ((int)_tileInfo.tileDirector + 1) % 4;

        RotateTile(newIndex);
    }
    
    public void RotateTile(int index)
    {
        CloseTile();
        // _tileInfo.tileDirector = (TileDirector) index;
        _tileInfo.rotatedPrefabs[index].SetActive(true);
        Vector2 pos = _tileInfo.transform.position;
        _tileInfo.UpdateSpriteOrder();
    }

    private void CloseTile()
    {
        foreach (var prefab in _tileInfo.rotatedPrefabs)
        {
            prefab.SetActive(false);
        }
    }

}