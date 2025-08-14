using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMove : MonoBehaviour
{
    private GameManager11 _gameManager;
    private TileManager _tileManager;
    private TileRoad _tileRoad;
    private Collider2D _collider;

    private Vector2 originalPosition;
    private Color originalColor;
    private SpriteRenderer[] _sprites;

    

    private void Awake()
    {
        _gameManager = GameManager11.Instance;
        _tileManager = TileManager.Instance;
        _tileRoad = GetComponent<TileRoad>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void OnMouseDown()
    {
        if (!_tileManager.isTileEditMode && !_tileManager.isTileMoveMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;


        originalPosition = transform.position;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        originalColor = _sprites[0].color;
        
        //if (_collider != null)
        //    _collider.enabled = false;

        _gameManager.tileRoad = _tileRoad;
        _gameManager.ShowTileInfo();
    }

    private void OnMouseDrag()
    {
        if (!_tileManager.isTileEditMode && !_tileManager.isTileMoveMode) if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        Plane plane = new Plane(Vector3.forward, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        for (int i = 0; i < _sprites.Length; i++)
        {
            _sprites[i].sortingOrder = 100 + i;
        }
        
        if (plane.Raycast(ray, out float distance))
        {
            Vector2 point = ray.GetPoint(distance);
            Vector2 delta = point - originalPosition;

            // 기준 벡터들
            Vector2 diagUpRight = new Vector2(_tileRoad.cellSize[0], _tileRoad.cellSize[1]);
            Vector2 diagUpLeft  = new Vector2(-_tileRoad.cellSize[0], _tileRoad.cellSize[1]);
            Vector2 diagDownRight = new Vector2(_tileRoad.cellSize[0], -_tileRoad.cellSize[1]);
            Vector2 diagDownLeft  = new Vector2(-_tileRoad.cellSize[0], -_tileRoad.cellSize[1]);
            Vector2 horizontal = new Vector2(_tileRoad.cellSize[0] * 2, 0f);
            Vector2 rhorizontal = new Vector2(-_tileRoad.cellSize[0] * 2, 0f);
            Vector2 vertical   = new Vector2(0f, _tileRoad.cellSize[1] * 2);
            Vector2 rvertical   = new Vector2(0f, -_tileRoad.cellSize[1] * 2);
            

            // 가장 가까운 방향 찾기
            Vector2[] directions = { diagUpRight, diagUpLeft, diagDownRight, diagDownLeft, horizontal, rhorizontal, vertical, rvertical };
            Vector2 bestDir = directions[0];
            float maxDot = Vector2.Dot(delta.normalized, directions[0].normalized);

            for (int i = 1; i < directions.Length; i++)
            {
                float dot = Vector2.Dot(delta.normalized, directions[i].normalized);
                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestDir = directions[i];
                }
            }

            // 몇 칸 움직일지 계산
            float magnitude = delta.magnitude;
            float stepSize = bestDir.magnitude;
            int steps = Mathf.RoundToInt(magnitude / stepSize);

            // 새로운 위치 계산
            Vector2 newPos = originalPosition + (bestDir.normalized * stepSize * steps);
            transform.position = new Vector2(newPos.x, newPos.y);
        }

        bool isValid = IsValidPosition(transform.position);

        foreach (var sprite in _sprites)
        {
            if (isValid)
                sprite.color = originalColor;
            else
                sprite.color = Color.red;
        }
    }

    private void OnMouseUp()
    {
        if (!_tileManager.isTileEditMode && !_tileManager.isTileMoveMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        foreach (var sprite in _sprites)
        {
            sprite.color = originalColor;
        }
        
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        TileRoad tileRoad = hit != null ? hit.GetComponent<TileRoad>() : null;

        if (tileRoad != null)
        {
            transform.position = originalPosition;
            UpdateGridPosition();
            Debug.Log("this location already located");
        }
        else
        {
            UpdateGridPosition();    
        }
        
        //if (_collider != null)
        //    _collider.enabled = true;
    }

    private void UpdateGridPosition()
    {
        _tileRoad.UpdateMapping(_tileRoad.GetGridSize(_tileRoad.mapLevel), transform.position);
        _tileRoad.UpdateTileSerialNumber();
    }

    private bool IsValidPosition(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        return hit == null || hit.gameObject == this.gameObject;
    }

}
