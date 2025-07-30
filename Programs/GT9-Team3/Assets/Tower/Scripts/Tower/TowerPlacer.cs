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

    [SerializeField] private Tilemap groundTilemap; // Ground 타일맵 참조
    public Grid grid; // 타워 타일 위에 설치하기

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

        // 미리보기 위치 갱신
        Vector3 previewPos = snappedPos;
        previewPos.y += 0.44f;
        currentPreview.transform.position = previewPos;

        // 설치 가능 여부에 따라 색상 변경
        bool canPlace = CanPlaceTowerAt(cellPos);
        SpriteRenderer sr = currentPreview.GetComponent<SpriteRenderer>();
        sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

        // 클릭해서 설치
        if (Input.GetMouseButtonDown(0) && canPlace) // && ResourceManager.Instance.CanAfford(...)
        {
            PlaceTower(snappedPos, cellPos);
        }
    }

    bool CanPlaceTowerAt(Vector3Int cellPos)
    {
        TileBase tile = groundTilemap.GetTile(cellPos);
        return tile != null && !occupiedCells.Contains(cellPos);
    }

    void PlaceTower(Vector3 position, Vector3Int cellPos)
    {
        // 타워를 타일보다 약간 위에 배치
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
            Debug.LogError("[오류] Tower1 스크립트가 프리팹에 없음");
        }

        ResourceManager.Instance.Spend(towerData.makeCost, towerData.makeValue);
        occupiedCells.Add(cellPos);
    }
}
