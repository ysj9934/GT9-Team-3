using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildUI : MonoBehaviour
{
    [Header("Wiring")]
    public RectTransform root;            // Panel 루트
    public Transform listParent;          // 항목이 배치될 Content
    public TowerOptionItem itemPrefab;

    [Header("Catalog")]
    public List<TowerBlueprint> options;  // 노출할 타워 종류들

    private TowerPlacer placer;
    private Vector3Int pendingCell;
    private Vector3 pendingWorld;

    private void Start()
    {
        if (TowerDataTableLoader.Instance == null)
            new TowerDataTableLoader();  // 명시적 초기화

        var table = TowerDataTableLoader.Instance.ItemsDict;

        foreach (var bp in options)
        {
            bp.ApplyLoadedData(table);
            Debug.Log("데이터 매핑 중: " + bp.name);
        }

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

    public void Hide() => root.gameObject.SetActive(false);

    public void OnClickBuild(TowerBlueprint bp)
    {
        if (!ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue)) return;

        placer.PlaceTowerFromUI(bp, pendingWorld, pendingCell);
        Hide();
    }
}
