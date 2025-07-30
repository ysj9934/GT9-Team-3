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

    public Grid grid;   // Ÿ�� Ÿ�� ���� ��ġ�ϱ�

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            Vector3Int cellPos = grid.WorldToCell(worldPos);          // Ŭ���� �� ��ǥ
            Vector3 snappedPos = grid.CellToWorld(cellPos);           // �� ���� ���� ��ġ
            snappedPos.z = 0;                                         // Z�� ����

            if (CanPlaceTowerAt(cellPos) && ResourceManager.Instance.CanAfford(towerData.makeCost, towerData.makeValue))
            {
                PlaceTower(snappedPos, cellPos);
            }
        }
    }

    bool CanPlaceTowerAt(Vector3 position)
    {
        // Ÿ�ϸ�, collider, ��Ÿ ��ġ ���� ���� �˻�
        // ������ ������ �����ϰ�
        return true;
    }

    void PlaceTower(Vector3 position)
    {
        Debug.Log($"[Ÿ�� ��ġ��] ��ġ: {position}");

        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);

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
