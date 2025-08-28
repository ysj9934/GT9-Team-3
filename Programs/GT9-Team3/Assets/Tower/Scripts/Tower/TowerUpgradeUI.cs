using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeUI : MonoBehaviour
{
    private Tower1 selectedTower;

    public void SetTargetTower(Tower1 tower)
    {
        selectedTower = tower;
    }

    public void OnUpgradeButtonClicked()
    {
        Debug.Log("업그레이드 버튼 눌림");

        if (selectedTower == null)
        {
            Debug.LogWarning("선택된 타워가 없습니다!");
        }

        if (selectedTower != null)
        {
            if (!selectedTower.TryUpgrade())
            {
                Debug.Log("업그레이드 실패");
            }
        }
        
    }
}
