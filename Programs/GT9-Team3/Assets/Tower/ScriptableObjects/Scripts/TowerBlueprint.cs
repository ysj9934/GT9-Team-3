using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tower Defense/Tower Blueprint")]
public class TowerBlueprint : ScriptableObject
{
    public string towerID;              // JSON���� ��Ī�� ID
    public string displayName;
    public Sprite icon;

    public GameObject towerPrefab;

    [HideInInspector] public TowerData data; // ��Ÿ�ӿ� ����

    public ResourceType CostType => data.makeCost;
    public int CostValue => data.makeValue;

    // �ܺο��� JSON ������ ����
    public void ApplyLoadedData(Dictionary<int, TowerDataRow> table)
    {
        if (data == null)
        {
            Debug.LogWarning($"[Ÿ���������Ʈ] ScriptableObject TowerData�� �������: {displayName}");
            return;
        }

        if (table.TryGetValue(data.towerID, out var row))
        {
            Debug.Log($"[TowerBlueprint] {data.towerID} ������ ã��. ���� ����");
            TowerDataMapper.ApplyToSO(data, row);
        }
        else
        {
            Debug.LogWarning($"[TowerBlueprint] {data.towerID} �� �ش��ϴ� ������ ����");
        }

        TowerDataMapper.ApplyToSO(data, row);

    }
}
