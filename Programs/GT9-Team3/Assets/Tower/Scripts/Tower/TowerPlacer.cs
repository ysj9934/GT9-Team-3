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

    [SerializeField] private Tilemap groundTilemap;     // Ground 타일맵 참조
    public Grid grid;                                   // 타워 타일 위에 설치하기

    public GameObject previewPrefab;
    private GameObject currentPreview;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    public TowerBuildUI buildUI;                        // 인스펙터 연결

    void Start()
    {
        currentPreview = Instantiate(previewPrefab);
    }

    //void Update()
    //{
    //    Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    //    worldPos.z = 0;

    //    Vector3Int cellPos = grid.WorldToCell(worldPos);
    //    Vector3 snappedPos = grid.GetCellCenterWorld(cellPos);
    //    snappedPos.z = 0;

    //    // 미리보기 위치 갱신
    //    Vector3 previewPos = snappedPos;
    //    previewPos.y += 0.24f;
    //    currentPreview.transform.position = previewPos;

    //    // 설치 가능 여부에 따라 색상 변경
    //    bool canPlace = CanPlaceTowerAt(cellPos);
    //    SpriteRenderer sr = currentPreview.GetComponent<SpriteRenderer>();
    //    sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

    //    // 클릭: 팝업 오픈
    //    if (Input.GetMouseButtonDown(0) && canPlace)
    //    {
    //        Vector2 screenPos = Input.mousePosition;
    //        buildUI.ShowAt(this, cellPos, snappedPos, screenPos);
    //    }
    //}

    public bool CanPlaceTowerAt(Vector3Int cellPos)
    {
        UnityEngine.Tilemaps.TileBase tile = groundTilemap.GetTile(cellPos);
        return tile != null && !occupiedCells.Contains(cellPos);
    }

    // UI에서 최종 선택 후 호출
    public void PlaceTowerFromUI(TowerBlueprint bp, Vector3 position, Vector3Int cellPos)
    {
        // 타워를 타일보다 약간 위에 배치
        position.y += 0.24f;

        GameObject tower = Instantiate(bp.towerPrefab, position, Quaternion.identity);
        var sr = tower.GetComponent<SpriteRenderer>();
        if (sr) sr.sortingOrder = -(int)(position.y * 100);

        var towerScript = tower.GetComponent<Tower1>();
        if (towerScript != null) towerScript.ApplyData(bp.data);

        ResourceManager.Instance.Spend(bp.CostType, bp.CostValue);
        occupiedCells.Add(cellPos);
    }

    public void PlaceTowerFromUI2(TowerBlueprint bp)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        GameObject go = Instantiate(bp.towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        Tower1 tower = go.GetComponent<Tower1>();
        //tower.Intialize(this);
        if (tower != null) tower.ApplyData(bp.data);
        tower.ApplyData(bp.data);
        ResourceManager.Instance.Spend(bp.CostType, bp.CostValue);

        //Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);

        //GameObject tower = Instantiate(bp.towerPrefab, pos, Quaternion.identity);
        //var sr = tower.GetComponent<SpriteRenderer>();
        //if (sr) sr.sortingOrder = -(int)(position.y * 100);

        //var towerScript = tower.GetComponent<Tower1>();
        

        ResourceManager.Instance.Spend(bp.CostType, bp.CostValue);
        //occupiedCells.Add(cellPos);
    }

}
