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

    public TowerData data; // ��Ÿ�ӿ� ����

    public ResourceType CostType => data.makeCost;
    public int CostValue => data.makeValue;

    // �ܺο��� JSON ������ ����
    public void ApplyLoadedData(Dictionary<int, TowerDataRow> table)
    {
        if (data == null)
        {
            Debug.LogWarning($"[TowerBlueprint] {name}�� data�� ��� ����");
            return;
        }

        Debug.Log($"[TowerBlueprint] {name}�� ������ ���� ����: ID = {data.towerID}");

        if (!table.TryGetValue(data.towerID, out var row))
        {
            Debug.LogWarning($"[TowerBlueprint] {name}: ������ ���̺� ID {data.towerID} ����");
            return;
        }


        TowerDataMapper.ApplyToSO(data, row);

    }
}
