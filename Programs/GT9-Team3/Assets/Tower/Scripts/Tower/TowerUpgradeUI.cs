using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeUI : MonoBehaviour
{
    public BlockInfo _blockInfo;
    private Tower1 selectedTower;

    private TowerData towerdata;


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
            return;
        }

        if (selectedTower.TryUpgrade())
        {
            Debug.Log("업그레이드 성공");

            // 선택된 타워 다시 갱신
            selectedTower.UpdateTowerVisual(selectedTower.towerdata.towerLevel);
            selectedTower.blockInfo.SetTowerUpgrade(selectedTower);
            // UI 정보 새로고침
            HUDCanvas.Instance.sellUI.Refresh(selectedTower);
        }
        else
        {
            Debug.Log("업그레이드 실패");
        }

    }
}
