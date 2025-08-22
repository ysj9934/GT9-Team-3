using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerSellUI : MonoBehaviour
{
    private Tower1 currentTarget;
    public RectTransform root;      // �г� ��Ʈ
    public Vector2 anchoredPosition = new Vector2(0, 0);  // ���� ��ġ ����

    public static TowerSellUI Instance;

    public GameObject panel;
    private Tower1 currentTower;

    // ��ȹ ������
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(Tower1 tower)
    {
        // ���� Ÿ���� ��Ÿ� ����
        if (currentTarget != null && currentTarget != tower)
        {
            Transform prevRV = currentTarget.transform.Find("RangeVisual");
            if (prevRV != null)
                prevRV.gameObject.SetActive(false);
        }

        currentTower = tower;
        currentTarget = tower;

        root.gameObject.SetActive(true);

        // ���� ���� ��ġ�� �̵�
        root.anchoredPosition = anchoredPosition;

        //towerNameText.text = tower.data.innerName;
        //levelText.text = $"Lv.{tower.data.towerLevel}";
        //damageText.text = tower.data.damage.ToString();
        //attackSpeedText.text = $"{tower.data.attackSpeed}/s";
    }

    public void Hide()
    {
        // ��Ÿ� ���ֱ�
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
        Debug.Log("�Ǹ� ��ư ����");

        currentTower.blockInfo._tileInfo._tilePlaceOnTower.HandleTowerPlacement(
            currentTower.blockInfo.blockSerialNumber, 
            true, 
            null,
            currentTower);

        panel.SetActive(false);

        //if (currentTower != null)
        //{
        //    Debug.Log("Ÿ�� ���� �� ��� ȯ��");
        //    ResourceManager.Instance.Earn(currentTower.data.makeCost, currentTower.data.sellValue);
        //    Destroy(currentTower.gameObject);
        //    panel.SetActive(false);
        //}
    }
}
