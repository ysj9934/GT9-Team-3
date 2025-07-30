using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask placementMask;
    public GameObject towerPrefab;
    public TowerData towerData;

    public Grid grid;   // 타워 타일 위에 설치하기

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            Vector3Int cellPos = grid.WorldToCell(worldPos);          // 클릭한 셀 좌표
            Vector3 snappedPos = grid.CellToWorld(cellPos);           // 셀 기준 월드 위치
            snappedPos.z = 0;                                         // Z축 고정

            if (CanPlaceTowerAt(cellPos) && ResourceManager.Instance.CanAfford(towerData.makeCost, towerData.makeValue))
            {
                PlaceTower(snappedPos, cellPos);
            }
        }
    }

    bool CanPlaceTowerAt(Vector3 position)
    {
        // 타일맵, collider, 기타 설치 가능 조건 검사
        // 지금은 무조건 가능하게
        return true;
    }

    void PlaceTower(Vector3 position)
    {
        Debug.Log($"[타워 설치됨] 위치: {position}");

        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);

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
    }

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    bool CanPlaceTowerAt(Vector3Int cellPos)
    {
        return !occupiedCells.Contains(cellPos);
    }

    void PlaceTower(Vector3 position, Vector3Int cellPos)
    {
        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
        Tower1 towerScript = tower.GetComponent<Tower1>();
        towerScript.ApplyData(towerData);

        ResourceManager.Instance.Spend(towerData.makeCost, towerData.makeValue);
        occupiedCells.Add(cellPos);
    }
}
