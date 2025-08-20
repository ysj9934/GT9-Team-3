using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tower Defense/Tower Blueprint")]
public class TowerBlueprint : ScriptableObject
{
    public string towerID;              // JSON에서 매칭할 ID
    public string displayName;
    public Sprite icon;

    public GameObject towerPrefab;

    [HideInInspector] public TowerData data; // 런타임에 연결

    public ResourceType CostType => data.makeCost;
    public int CostValue => data.makeValue;

    // 외부에서 JSON 데이터 주입
    public void ApplyLoadedData(Dictionary<int, TowerDataRow> table)
    {
        if (data == null)
        {
            Debug.LogWarning($"[타워블루프린트] ScriptableObject TowerData가 비어있음: {displayName}");
            return;
        }

        if (table.TryGetValue(data.towerID, out var row))
        {
            Debug.Log($"[TowerBlueprint] {data.towerID} 데이터 찾음. 매핑 시작");
            TowerDataMapper.ApplyToSO(data, row);
        }
        else
        {
            Debug.LogWarning($"[TowerBlueprint] {data.towerID} 에 해당하는 데이터 없음");
        }

        TowerDataMapper.ApplyToSO(data, row);

    }
}
