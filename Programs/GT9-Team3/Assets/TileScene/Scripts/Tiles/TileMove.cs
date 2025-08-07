using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMove : MonoBehaviour
{
    private TileRoad _tile;

    private void Awake()
    {
        _tile = GetComponent<TileRoad>();
    }

    private void OnMouseDrag()
    {
        Plane plane = new Plane(Vector3.forward, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.position = SnapToGrid(point);
        }
    }
    
    // 그리드 크기에 맞춰 타일 움직임
    private Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / _tile.cellSize[0]) * _tile.cellSize[0];
        float y = Mathf.Round(pos.y / _tile.cellSize[1]) * _tile.cellSize[1];
        
        return new Vector3(x, y, pos.z);
    }

    private void OnMouseUp()
    {
        UpdateGridPosition();
    }
    
    private void UpdateGridPosition()
    {
        _tile.GetMapping(_tile.GetGridSize(_tile.mapLevel), transform.position);
        _tile.UpdateTileSerialNumber();
    }

}
