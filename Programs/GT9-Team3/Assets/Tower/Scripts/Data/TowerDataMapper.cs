using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataMapper
{
    public static TargetPriority[] BuildPriorityArray(params string[] values)
    {
        List<TargetPriority> list = new List<TargetPriority>();
        foreach (var val in values)
        {
            if (string.IsNullOrEmpty(val)) continue;

            switch (val)
            {
                case "Boss": list.Add(TargetPriority.Boss); break;
                case "Base_Closest": list.Add(TargetPriority.Base_Range); break;
                case "Low_HP": list.Add(TargetPriority.Lowest_HP); break;
                case "Tower_Closest": list.Add(TargetPriority.Base_Closest); break;
                default: Debug.LogWarning($"[우선순위 변환 실패] {val}"); break;
            }
        }
        return list.ToArray();
    }

    public static void ApplyToSO(TowerData so, TowerDataRow row)
    {

        so.innerName = row.Inner_Name;
        so.towerLevel = row.Tower_Grade;

        so.targetOrder = BuildPriorityArray(row.Target_Order1, row.Target_Order2, row.Target_Order3, row.Target_Order4);

        so.makeCost = ResourceType.Gold;        // 현재는 전부 Gold
        so.makeValue = row.Make_Value;

        so.sellCost = ResourceType.Gold;
        so.sellValue = row.Sell_Value;

        so.UpgradeCost = ResourceType.Gold;
        so.UpgradeValue = row.Upgrade_Value;

        Debug.Log($"[TowerDataMapper] 스탯 매핑 완료: ID = {row.key}, 이름 = {row.Inner_Name}, 레벨 = {row.Tower_Grade}, 생성비용 = {row.Make_Value}, 판매가 = {row.Sell_Value}");


        // 이외에 attackRange, attackSpeed, damage 등은 다른 테이블에서 불러와야 함
    }
}
