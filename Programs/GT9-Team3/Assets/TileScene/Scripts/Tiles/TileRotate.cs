using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate : MonoBehaviour
{
    // [SerializeField] public TileRotation currentRotationIndex;
    private TileRoad _tile;
    [SerializeField] private GameObject[] rotatedPrefabs;


    private void Start()
    {
        _tile = GetComponent<TileRoad>();
    }

    // 다음 각도의 맵 타일 생성하기
    public void RotateLeft()
    {
        int newIndex = ((int)_tile.currentRotationIndex - 1 + 4) % 4;
        
        RotateTile(newIndex);
    }

    public void RotateRight()
    {
        int newIndex = ((int)_tile.currentRotationIndex + 1) % 4;

        RotateTile(newIndex);
    }
    
    public void RotateTile(int index)
    {
        CloseTile();
        
        _tile.currentRotationIndex = (TileRotation) index;
        rotatedPrefabs[index].SetActive(true);
        _tile.Initialize(_tile.mapLevel, _tile.transform.position);
    }

    private void CloseTile()
    {
        foreach (var prefab in rotatedPrefabs)
        {
            prefab.SetActive(false);
        }
    }

}