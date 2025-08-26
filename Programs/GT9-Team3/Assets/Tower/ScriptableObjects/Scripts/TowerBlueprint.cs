using System;
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

    public TowerData data; // 런타임에 연결

    public ResourceType CostType => data.makeCost;
    public int CostValue => data.makeValue;

    // 외부에서 JSON 데이터 주입
    public void ApplyLoadedData(Dictionary<int, TowerDataRow> Towertable, Dictionary<int, ProjectileDataRow> projectileTable)
    {
        if (data == null)
        {
            Debug.LogWarning($"[TowerBlueprint] {name}의 data가 비어 있음");
            return;
        }

        Debug.Log($"[TowerBlueprint] {name}의 데이터 매핑 시작: ID = {data.towerID}");

        if (!Towertable.TryGetValue(data.towerID, out var row))
        {
            Debug.LogWarning($"[TowerBlueprint] {name}: 데이터 테이블에 ID {data.towerID} 없음");
            return;
        }


        TowerDataMapper.ApplyToSO(data, row);

        if (data.projectileData != null && int.TryParse(data.projectileData.projectileID, out int projID))
        {
            if (projectileTable.TryGetValue(projID, out var projRow))
            {
                ProjectileDataMapper.ApplyToSO(data.projectileData, projRow);
            }
            else
            {
                Debug.LogWarning($"[TowerBlueprint] {name}: 프로젝타일 테이블에 ID {projID} 없음");
            }
        }
        else
        {
            Debug.LogWarning($"[TowerBlueprint] {name}: projectileData가 null이거나 ID 파싱 실패");
        }
    }

    internal void ApplyLoadedData(Dictionary<int, TowerDataRow> table)
    {
        var projectileTable = ProjectileDataTableLoader.Instance?.ItemsDict;
        if (projectileTable == null)
        {
            Debug.LogWarning("[TowerBlueprint] projectileTable이 null입니다.");
            return;
        }

        Debug.LogWarning($"[TowerBlueprint] {name}: projectileData가 null이거나 ID 파싱 실패");

        ApplyLoadedData(table, projectileTable);
    }
}
