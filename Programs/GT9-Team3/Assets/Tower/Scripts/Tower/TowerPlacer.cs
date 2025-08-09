using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask placementMask;
    public GameObject towerPrefab;
    public TowerData towerData;

    [SerializeField] private Tilemap groundTilemap; // Ground Ÿ�ϸ� ����
    public Grid grid; // Ÿ�� Ÿ�� ���� ��ġ�ϱ�

    public GameObject previewPrefab;
    private GameObject currentPreview;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    void Start()
    {
        currentPreview = Instantiate(previewPrefab);
    }

    void Update()
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        Vector3Int cellPos = grid.WorldToCell(worldPos);
        Vector3 snappedPos = grid.GetCellCenterWorld(cellPos);
        snappedPos.z = 0;

        // �̸����� ��ġ ����
        Vector3 previewPos = snappedPos;
        previewPos.y += 0.44f;
        currentPreview.transform.position = previewPos;

        // ��ġ ���� ���ο� ���� ���� ����
        bool canPlace = CanPlaceTowerAt(cellPos);
        SpriteRenderer sr = currentPreview.GetComponent<SpriteRenderer>();
        sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

        // Ŭ���ؼ� ��ġ
        if (Input.GetMouseButtonDown(0) && canPlace) // && ResourceManager.Instance.CanAfford(...)
        {
            PlaceTower(snappedPos, cellPos);
        }
    }

    public bool CanPlaceTowerAt(Vector3Int cellPos)
    {
        UnityEngine.Tilemaps.TileBase tile = groundTilemap.GetTile(cellPos);
        return tile != null && !occupiedCells.Contains(cellPos);
    }

    public void PlaceTower(Vector3 position, Vector3Int cellPos)
    {
        // Ÿ���� Ÿ�Ϻ��� �ణ ���� ��ġ
        position.y += 0.44f;

        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y * 100);

        Tower1 towerScript = tower.GetComponent<Tower1>();
        if (towerScript != null)
        {
            towerScript.ApplyData(towerData);
        }
        else
        {
            Debug.LogError("[����] Tower1 ��ũ��Ʈ�� �����տ� ����");
        }

        ResourceManager.Instance.Spend(towerData.makeCost, towerData.makeValue);
        occupiedCells.Add(cellPos);
    }
}
