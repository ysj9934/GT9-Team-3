using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    private TileManager _tileManager;
    private TileRoad _tileRoad;
    private Tower_Tile[] _towerTile;
    public Collider2D _collider2D;
    
    [SerializeField] public int blockSerialNumber;
    [SerializeField] private BlockCategory blockCategory;
    private bool hasTower;
    // 임시 사용 타워 프리팹
    // Temporary tower prefab for testing purposes
    [SerializeField] private GameObject towerPrefab;
    

    // 타워 설치 UI
    [SerializeField] private GameObject towerPlacementUI;   // 타워 설치 UI GameObject;


    private void Awake()
    {
        _tileManager = TileManager.Instance;
        _tileRoad = GetComponentInParent<TileRoad>();
        _towerTile = GetComponentsInChildren<Tower_Tile>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _tileManager = TileManager.Instance;

        // 타워 설치 UI 비활성화
        if (towerPlacementUI != null)
            towerPlacementUI.SetActive(false);

        // TileEditMode가 아닐 때 작동 안함.
        if (_collider2D != null)
            _collider2D.enabled = false;
    }

    private void OnMouseDown()
    {
        if (_tileManager == null)
        {
            _tileManager = TileManager.Instance;
        }

        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            towerPlacementUI.SetActive(false);
            return;
        }

        if (!_tileRoad.isSelected)
            return;

        //_collider2D.enabled = true; // 타워 설치 UI 활성화 시 Collider2D 활성화

        if (!hasTower)
            ToggleTowerPlacementUI();
    }



    // 타일 편집모드에서 타워 설치를 가능하도록 한다.

    // 타워 설치 UI 활성화/비활성화 토글
    // Toggle tower placement UI on/off
    public void ToggleTowerPlacementUI()
    {
        // TileEditMode가 아닐 때 작동 안함.
        if (_tileManager == null)
        {
            _tileManager = TileManager.Instance;
        }

        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            towerPlacementUI.SetActive(false);
            return;
        }

        towerPlacementUI.SetActive(!towerPlacementUI.activeSelf);
    }


    // 타워 신규 생성
    public void CallNumber()
    {
        // TileEditMode가 아닐 때 작동 안함.
        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        _tileRoad.ReceiveBlockNumber(blockSerialNumber);
    }

    
    public void PlaceTower()
    {
        if (hasTower)
        {
            Debug.LogWarning("A tower is already installed");
            return;
        }
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        
        // 본인에게 설치
        GameObject go = Instantiate(towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        
        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider2D != null)
            _collider2D.enabled = false;

        // 타워 설치시 UI 비활성화
        // Disable UI when a tower is placed;
        ToggleTowerPlacementUI();
    }
}
