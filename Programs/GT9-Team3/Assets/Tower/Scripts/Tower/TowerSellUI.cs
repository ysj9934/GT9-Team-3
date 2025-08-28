using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSellUI : MonoBehaviour
{
    private Tower1 currentTarget;
    public RectTransform root;      // 패널 루트
    public Vector2 anchoredPosition = new Vector2(0, 0);  // 왼쪽 위치 고정

    public static TowerSellUI Instance;

    public GameObject panel;
    private Tower1 currentTower;

    // 기획 프리펩
    public Image towerIconImage;
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI targetCountText;
    public TextMeshProUGUI attackTypeText;
    public TextMeshProUGUI sellValueText;
    public TextMeshProUGUI upgradeValueText;
    public TextMeshProUGUI slowEffectText;
    public TextMeshProUGUI slowTimeText;
    public TextMeshProUGUI ccTimeText;
    public TextMeshProUGUI tileRangeText;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(Tower1 tower)
    {

        if (tower == null || tower.towerdata == null || tower.blueprint == null)
        {
            Debug.LogWarning("[TowerSellUI] 타워 데이터가 아직 초기화되지 않음");
            return;
        }

        // 기존 타워의 사거리 숨김
        if (currentTarget != null && currentTarget != tower)
        {
            Transform prevRV = currentTarget.transform.Find("RangeVisual");
            if (prevRV != null)
                prevRV.gameObject.SetActive(false);
        }

        currentTower = tower;
        currentTarget = tower;

        root.gameObject.SetActive(true);

        // 왼쪽 고정 위치로 이동
        root.anchoredPosition = anchoredPosition;

        TowerData d = tower.towerdata;

        towerIconImage.sprite = tower.blueprint.icon;     // TowerBluePrint에서 받아옴
        towerNameText.text = d.innerName;
        levelText.text = $"Lv.{d.towerLevel}";
        attackSpeedText.text = $"{d.attackSpeed}/s";
        rangeText.text = d.attackRange.ToString();
        targetCountText.text = d.targetCount.ToString();
        attackTypeText.text = d.attackType.ToString();
        sellValueText.text = $"Cost : {d.sellValue}";
        upgradeValueText.text = $"Cost : {d.UpgradeValue}";

        ProjectileData p = tower.towerdata.projectileData;

        damageText.text = p.damage.ToString();
        slowEffectText.text = p.slowEffect.ToString();
        slowTimeText.text = $"{p.slowTime}/s";
        ccTimeText.text = $"{p.stunTime}/s";
        tileRangeText.text = $"{p.impactRadius}/Tile";
    }

    public void Hide()
    {
        // 사거리 꺼주기
        if (currentTarget != null)
        {
            Transform rv = currentTarget.transform.Find("RangeVisual");
            if (rv != null)
                rv.gameObject.SetActive(false);
        }

        root.gameObject.SetActive(false);
        currentTarget = null;
    }

    public bool IsOpenFor(Tower1 tower)
    {
        return root.gameObject.activeSelf && currentTarget != null && currentTarget.GetInstanceID() == tower.GetInstanceID();
    }


    public void OnClickClose()
    {
        Hide();
    }


    public void OnClickSell()
    {
        Debug.Log("판매 버튼 눌림");

        currentTower.blockInfo._tileInfo._tilePlaceOnTower.HandleTowerPlacement(
            currentTower.blockInfo.blockSerialNumber, 
            true, 
            null,
            currentTower);

        panel.SetActive(false);

        //if (currentTower != null)
        //{
        //    Debug.Log("타워 제거 및 골드 환급");
        //    ResourceManager.Instance.Earn(currentTower.data.makeCost, currentTower.data.sellValue);
        //    Destroy(currentTower.gameObject);
        //    panel.SetActive(false);
        //}
    }

    public void Refresh(Tower1 tower)
    {
        // 내부적으로 tower.data에서 정보 불러와 텍스트나 이미지 업데이트
        TowerData d = tower.towerdata;
        ProjectileData p = d.projectileData;

        towerIconImage.sprite = tower.blueprint.icon;
        towerNameText.text = d.innerName;
        levelText.text = $"Lv.{d.towerLevel}";
        attackSpeedText.text = $"{d.attackSpeed}/s";
        rangeText.text = d.attackRange.ToString();
        targetCountText.text = d.targetCount.ToString();
        attackTypeText.text = d.attackType.ToString();
        sellValueText.text = $"Cost : {d.sellValue}";
        upgradeValueText.text = $"Cost : {d.UpgradeValue}";

        damageText.text = p.damage.ToString();
        slowEffectText.text = p.slowEffect.ToString();
        slowTimeText.text = $"{p.slowTime}/s";
        ccTimeText.text = $"{p.stunTime}/s";
        tileRangeText.text = $"{p.impactRadius}/Tile";
    }
}
