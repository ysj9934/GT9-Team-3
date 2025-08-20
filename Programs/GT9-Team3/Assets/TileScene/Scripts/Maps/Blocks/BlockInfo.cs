using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class BlockInfo : MonoBehaviour
{
    public TileInfo _tileInfo;
    [SerializeField] public BlockData _blockData;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public BlockCategory blockCategory;
    [SerializeField] public int blockSerialNumber;

    private Collider2D _collider;
    
    public int level = 1;
    public bool hasTower = false;
    
    // temp
    [SerializeField] public GameObject towerUI;
    [SerializeField] public GameObject towerPrefab;

    private void Awake()
    {
        _tileInfo = GetComponentInParent<TileInfo>();
        _collider = GetComponent<Collider2D>();
        if (towerUI != null)
            towerUI.SetActive(false);
    }

    public void UpdateWorldLevel(int level)
    {
        this.level = level;
        spriteRenderer.sprite = _blockData.sprites[level - 1];
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            if (hit.collider.gameObject != this.gameObject)
            {
                // 내가 아닌 다른 GameObject가 클릭된 경우 → 무시
                return;
            }
        }

        towerUI.SetActive(!towerUI.activeSelf);
    }
    
    // 명칭변경 필요
    public void SetTower()
    {
        _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower);
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
        Tower2 tower = go.GetComponent<Tower2>();
        tower.Intialize(this);
        tower.ApplyData(bp.data);
        ResourceManager.Instance.Spend(bp.CostType, bp.CostValue);

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider != null)
            _collider.enabled = false;
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
        Tower2 tower = go.GetComponent<Tower2>();
        tower.Intialize(this);

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider != null)
            _collider.enabled = false;

        // 타워 설치시 UI 비활성화
        // Disable UI when a tower is placed;
        towerUI.SetActive(!towerUI.activeSelf);
    }
}
