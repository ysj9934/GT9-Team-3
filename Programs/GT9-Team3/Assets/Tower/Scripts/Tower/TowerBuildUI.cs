using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildUI : MonoBehaviour
{
    public BlockInfo _blockInfo;

    [Header("Wiring")]
    public RectTransform root;            // Panel 루트
    public Transform listParent;          // 항목이 배치될 Content
    public TowerOptionItem itemPrefab;

    [Header("Catalog")]
    [SerializeField] public List<TowerBlueprint> options;  // 노출할 타워 종류들

    private TowerPlacer placer;
    private Vector3Int pendingCell;
    private Vector3 pendingWorld;

    private void Start()
    {
        if (TowerDataTableLoader.Instance == null)
            new TowerDataTableLoader();

        if (ProjectileDataLoader.Instance == null)
            new ProjectileDataLoader();

        var towerTable = TowerDataTableLoader.Instance.ItemsDict;
        var projectileTable = ProjectileDataLoader.Instance.ItemsDict;

        foreach (var bp in options)
        {
            bp.ApplyLoadedData(towerTable, projectileTable);
        }

        Debug.Log($"ProjectileDataLoader.Instance is null? -> {ProjectileDataLoader.Instance == null}");
        Debug.Log($"TowerDataLoader.Instance is null? -> {TowerDataTableLoader.Instance == null}");

    }

    void Awake() => Hide();

    public void ShowAt(TowerPlacer caller, Vector3Int cell, Vector3 world, Vector2 screenPos)
    {
        placer = caller;
        pendingCell = cell;
        pendingWorld = world;

        // 위치(마우스 근처) 배치
        root.gameObject.SetActive(true);
        //root.position = screenPos;

        // 리스트 갱신
        foreach (Transform c in listParent) Destroy(c.gameObject);

        foreach (var bp in options)
        {
            var item = Instantiate(itemPrefab, listParent);
            bool canAfford = ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue);
            item.Setup(bp, this, canAfford);
        }
    }

    public void ShowAt(BlockInfo blockInfo)
    {
        root.gameObject.SetActive(true);

        // 리스트 갱신
        foreach (Transform c in listParent) Destroy(c.gameObject);

        foreach (var bp in options)
        {
            var item = Instantiate(itemPrefab, listParent);
            bool canAfford = ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue);
            item.Setup2(bp, this, canAfford);
        }

        _blockInfo = blockInfo;
    }

    public void Hide() => root.gameObject.SetActive(false);

    public void OnClickBuild(TowerBlueprint bp)
    {
        if (!ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue)) return;

        placer.PlaceTowerFromUI(bp, pendingWorld, pendingCell);
        Hide();
    }

    public void OnClickBuild2(TowerBlueprint bp)
    {
        if (!ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue)) return;

        //placer.PlaceTowerFromUI2(bp);
        _blockInfo.SetTowerPlace(bp);

        Hide();
    }
}
