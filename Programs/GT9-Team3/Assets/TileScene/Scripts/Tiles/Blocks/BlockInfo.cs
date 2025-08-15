using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockInfo : MonoBehaviour
{
    private TileManager _tileManager;
    private TileRoad _tileRoad;
    private Tower_Tile[] _towerTile;
    public Collider2D _collider2D;
    private Tower1 _tower1;
    
    [SerializeField] public int blockSerialNumber;
    [SerializeField] private BlockCategory blockCategory;
    public bool hasTower;
    // 임시 사용 타워 프리팹
    // Temporary tower prefab for testing purposes
    [SerializeField] private GameObject towerPrefab;
    

    // 타워 설치 UI
    [SerializeField] private GameObject towerPlacementUI;   // 타워 설치 UI GameObject;
    [SerializeField] public TowerBuildUI buildUI;

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
        buildUI = FindObjectOfType<TowerBuildUI>();

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


        if (!hasTower)
            //ToggleTowerPlacementUI();
            ToggleTowerPlacementUI2();
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

    public void ToggleTowerPlacementUI2()
    {
        // TileEditMode가 아닐 때 작동 안함.
        if (_tileManager == null)
        {
            _tileManager = TileManager.Instance;
        }

        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            //buildUI.SetActive(false);
            return;
        }

        bool isOpening = buildUI.root.gameObject.activeSelf;
        if (isOpening)
            buildUI.Hide(); 
        else
            buildUI.ShowAt(this);
    }


    // 타워 신규 생성
    public void CallNumber(TowerBlueprint bp)
    {
        // TileEditMode가 아닐 때 작동 안함.
        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        _tileRoad.ReceiveBlockNumber(blockSerialNumber, true, bp);
    }

    // 타워 판매
    public void CallNumber2()
    {
        // TileEditMode가 아닐 때 작동 안함.
        if (!_tileManager.isTileEditMode)
        {
            Debug.LogWarning("It doesn't work when not in TileEditMode");
            return;
        }

        _tileRoad.ReceiveBlockNumber(blockSerialNumber, false, null);
    }

    public void PlaceTower(TowerBlueprint bp)
    {
        if (hasTower)
        {
            Debug.LogWarning("A tower is already installed");
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        GameObject go = Instantiate(bp.towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        Tower1 tower = go.GetComponent<Tower1>();
        tower.Intialize(this);
        tower.ApplyData(bp.data);
        ResourceManager.Instance.Spend(bp.CostType, bp.CostValue);

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider2D != null)
            _collider2D.enabled = false;
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
        Tower1 tower = go.GetComponent<Tower1>();
        tower.Intialize(this);

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider2D != null)
            _collider2D.enabled = false;

        // 타워 설치시 UI 비활성화
        // Disable UI when a tower is placed;
        //ToggleTowerPlacementUI();
    }

    public void RemoveTower()
    {
        _tower1 = GetComponentInChildren<Tower1>();
        Destroy(_tower1.gameObject);

        hasTower = false;

        if (_collider2D != null)
            _collider2D.enabled = true;
    }
}
