using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMove1 : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;

    public SpriteRenderer sr;
    public Color normalColor = Color.white;
    public Color invalidColor = Color.red;

    public float tileWidth  = 1.4475f * 2;
    public float tileHeight  = 0.84f * 2;

    private void Start()
    {
        mainCam = Camera.main;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (!CanPlace(transform.position))
        {
            transform.position = Vector2.zero;
            sr.color = normalColor;
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mainCam.transform.position.z)));
            mouseWorld.z = 0f;
            
            Vector2Int gridPos = WorldToGrid(mouseWorld);
            // 그리드 좌표 → 다시 월드 좌표 변환
            Vector3 snappedPos = GridToWorld(gridPos);

            transform.position = snappedPos;

            // 유효 여부 판정
            if (!CanPlace(snappedPos))
                sr.color = invalidColor;
            else
                sr.color = normalColor;
            
            // Vector2 mousePos = Input.mousePosition;
            // Vector2 gridPos = MouseToGridPosition(mainCam, mousePos, gridX, gridY);
            //
            // transform.position = gridPos;
            //
            // if (!CanPlace(gridPos))
            //     sr.color = invalidColor;
            // else
            // {
            //     sr.color = normalColor;
            // }
        }
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int gx = Mathf.RoundToInt((worldPos.x / tileWidth + worldPos.y / tileHeight));
        int gy = Mathf.RoundToInt((worldPos.y / tileHeight - (worldPos.x / tileWidth)));
        
        int gridX = Mathf.RoundToInt(gx);
        int gridY = Mathf.RoundToInt(gy);
        
        return new Vector2Int(gridX, gridY);
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        float wx = (gridPos.x - gridPos.y) * (tileWidth / 2f);
        float wy = (gridPos.x + gridPos.y) * (tileHeight / 2f);
        return new Vector3(wx, wy, 0f);
    }

    private bool CanPlace(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        return hit == null || hit.gameObject == gameObject;
    }

    // private TileManager _tileManager;
    // private TileRoad _tileRoad;
    // private Collider2D _collider;
    //
    // private Vector2 originalPosition;
    // private Color originalColor;
    // private SpriteRenderer[] _sprites;
    //
    //
    //
    // private void Awake()
    // {
    //     _tileManager = TileManager.Instance;
    //     _tileRoad = GetComponent<TileRoad>();
    //     _collider = GetComponent<PolygonCollider2D>();
    // }
    //
    // private void OnMouseDown()
    // {
    //     if (!_tileManager.isTileMoveMode)
    //     {
    //         Debug.LogWarning("It doesn't work when not in isTileMoveMode");
    //         return;
    //     }
    //
    //     if (EventSystem.current.IsPointerOverGameObject())
    //         return;
    //
    //
    //     originalPosition = transform.position;
    //     _sprites = GetComponentsInChildren<SpriteRenderer>();
    //     originalColor = _sprites[0].color;
    //     
    //     if (_collider != null)
    //         _collider.enabled = false;
    //
    //     //_gameManager.tileRoad = _tileRoad;
    //     //_gameManager.ShowTileInfo();
    // }
    //
    // private void OnMouseDrag()
    // {
    //     if (!_tileManager.isTileMoveMode)
    //     {
    //         Debug.LogWarning("It doesn't work when not in isTileMoveMode");
    //         return;
    //     }
    //
    //     //if (EventSystem.current.IsPointerOverGameObject())
    //     //    return;
    //     
    //     Plane plane = new Plane(Vector3.forward, 0);
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     
    //
    //     for (int i = 0; i < _sprites.Length; i++)
    //     {
    //         _sprites[i].sortingOrder = 100 + i;
    //     }
    //     
    //     if (plane.Raycast(ray, out float distance))
    //     {
    //         Vector2 point = ray.GetPoint(distance);
    //         Vector2 delta = point - originalPosition;
    //
    //         // 기준 벡터들
    //         Vector2 diagUpRight = new Vector2(_tileRoad.cellSize[0], _tileRoad.cellSize[1]);
    //         Vector2 diagUpLeft  = new Vector2(-_tileRoad.cellSize[0], _tileRoad.cellSize[1]);
    //         Vector2 diagDownRight = new Vector2(_tileRoad.cellSize[0], -_tileRoad.cellSize[1]);
    //         Vector2 diagDownLeft  = new Vector2(-_tileRoad.cellSize[0], -_tileRoad.cellSize[1]);
    //         Vector2 horizontal = new Vector2(_tileRoad.cellSize[0] * 2, 0f);
    //         Vector2 rhorizontal = new Vector2(-_tileRoad.cellSize[0] * 2, 0f);
    //         Vector2 vertical   = new Vector2(0f, _tileRoad.cellSize[1] * 2);
    //         Vector2 rvertical   = new Vector2(0f, -_tileRoad.cellSize[1] * 2);
    //         
    //
    //         // 가장 가까운 방향 찾기
    //         Vector2[] directions = { diagUpRight, diagUpLeft, diagDownRight, diagDownLeft, horizontal, rhorizontal, vertical, rvertical };
    //         Vector2 bestDir = directions[0];
    //         float maxDot = Vector2.Dot(delta.normalized, directions[0].normalized);
    //
    //         for (int i = 1; i < directions.Length; i++)
    //         {
    //             float dot = Vector2.Dot(delta.normalized, directions[i].normalized);
    //             if (dot > maxDot)
    //             {
    //                 maxDot = dot;
    //                 bestDir = directions[i];
    //             }
    //         }
    //
    //         // 몇 칸 움직일지 계산
    //         float magnitude = delta.magnitude;
    //         float stepSize = bestDir.magnitude;
    //         int steps = Mathf.RoundToInt(magnitude / stepSize);
    //
    //         // 새로운 위치 계산
    //         Vector2 newPos = originalPosition + (bestDir.normalized * stepSize * steps);
    //         transform.position = new Vector2(newPos.x, newPos.y);
    //     }
    //
    //     bool isValid = IsValidPosition(transform.position);
    //
    //     foreach (var sprite in _sprites)
    //     {
    //         if (isValid)
    //             sprite.color = originalColor;
    //         else
    //             sprite.color = Color.red;
    //     }
    // }
    //
    // private void OnMouseUp()
    // {
    //     if (!_tileManager.isTileMoveMode)
    //     {
    //         Debug.LogWarning("It doesn't work when not in isTileMoveMode");
    //         return;
    //     }
    //
    //     //if (EventSystem.current.IsPointerOverGameObject())
    //     //    return;
    //     
    //     foreach (var sprite in _sprites)
    //     {
    //         sprite.color = originalColor;
    //     }
    //     
    //     Collider2D hit = Physics2D.OverlapPoint(transform.position);
    //     TileRoad tileRoad = hit != null ? hit.GetComponent<TileRoad>() : null;
    //
    //     if (tileRoad != null)
    //     {
    //         Debug.Log($"{tileRoad.row}, {tileRoad.col}");
    //         transform.position = originalPosition;
    //         UpdateGridPosition();
    //         Debug.Log("this location already located");
    //     }
    //     else
    //     {
    //         UpdateGridPosition();    
    //     }
    //     
    //     if (_collider != null)
    //         _collider.enabled = true;
    // }
    //
    // private void UpdateGridPosition()
    // {
    //     _tileRoad.UpdateMapping(_tileRoad.GetGridSize(_tileRoad.mapLevel), transform.position);
    //     _tileRoad.UpdateTileSerialNumber();
    // }
    //
    // private bool IsValidPosition(Vector2 pos)
    // {
    //     Collider2D hit = Physics2D.OverlapPoint(pos);
    //     return hit == null || hit.gameObject == this.gameObject;
    // }

}
