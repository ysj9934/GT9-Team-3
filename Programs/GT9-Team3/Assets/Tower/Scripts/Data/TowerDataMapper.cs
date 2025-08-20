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
                default: Debug.LogWarning($"[�켱���� ��ȯ ����] {val}"); break;
            }
        }
        return list.ToArray();
    }

    public static void ApplyToSO(TowerData so, TowerDataRow row)
    {

        so.innerName = row.Inner_Name;
        so.towerLevel = row.Tower_Grade;

        so.targetOrder = BuildPriorityArray(row.Target_Order1, row.Target_Order2, row.Target_Order3, row.Target_Order4);

        so.makeCost = ResourceType.Gold;        // ����� ���� Gold
        so.makeValue = row.Make_Value;

        so.sellCost = ResourceType.Gold;
        so.sellValue = row.Sell_Value;

        Debug.Log($"[TowerDataMapper] ���� ���� �Ϸ�: ID = {row.key}, �̸� = {row.Inner_Name}, ���� = {row.Tower_Grade}, ������� = {row.Make_Value}, �ǸŰ� = {row.Sell_Value}");


        // �̿ܿ� attackRange, attackSpeed, damage ���� �ٸ� ���̺��� �ҷ��;� ��
    }
}
