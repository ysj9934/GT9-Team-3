using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuildUI : MonoBehaviour
{
    private BlockInfo _blockInfo;

    [Header("Wiring")]
    public RectTransform root;            // Panel 루트
    public RectTransform listParent;          // 항목이 배치될 Content
    public TowerOptionItem itemPrefab;


    [Header("Catalog")]
    public List<TowerBlueprint> options;  // 노출할 타워 종류들

    private TowerPlacer placer;
    private Vector3Int pendingCell;
    private Vector3 pendingWorld;

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

        foreach (Transform c in listParent) Destroy(c.gameObject);

        foreach (var bp in options)
        {
            var item = Instantiate(itemPrefab, listParent);
            bool canAfford = ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue);
            item.Setup(bp, this, canAfford);
        }

        _blockInfo = blockInfo;
    }

    public void Hide() => root.gameObject.SetActive(false);

    public void OnClickBuild(TowerBlueprint bp)
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;


        if (!ResourceManager.Instance.CanAfford(bp.CostType, bp.CostValue)) return;

        //placer.PlaceTowerFromUI(bp, pendingWorld, pendingCell);
        _blockInfo.CallNumber(bp);

        Hide();
    }
}
