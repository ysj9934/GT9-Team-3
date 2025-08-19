using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildUI : MonoBehaviour
{
    [Header("Wiring")]
    public RectTransform root;            // Panel ��Ʈ
    public Transform listParent;          // �׸��� ��ġ�� Content
    public TowerOptionItem itemPrefab;

    [Header("Catalog")]
    public List<TowerBlueprint> options;  // ������ Ÿ�� ������

    private TowerPlacer placer;
    private Vector3Int pendingCell;
    private Vector3 pendingWorld;

    private void Start()
    {
        if (TowerDataTableLoader.Instance == null)
            new TowerDataTableLoader();  // ����� �ʱ�ȭ

        var table = TowerDataTableLoader.Instance.ItemsDict;

        foreach (var bp in options)
        {
            bp.ApplyLoadedData(table);
            Debug.Log("������ ���� ��: " + bp.name);
        }

    }

    void Awake() => Hide();

    public void ShowAt(TowerPlacer caller, Vector3Int cell, Vector3 world, Vector2 screenPos)
    {
        placer = caller;
        pendingCell = cell;
        pendingWorld = world;

        // ��ġ(���콺 ��ó) ��ġ
        root.gameObject.SetActive(true);
        //root.position = screenPos;

        // ����Ʈ ����
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
