using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSellUI : MonoBehaviour
{
    public static TowerSellUI Instance;

    public GameObject panel;
    private Tower1 currentTower;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(Tower1 tower)
    {
        currentTower = tower;
        panel.transform.position = Camera.main.WorldToScreenPoint(tower.transform.position);
        panel.SetActive(true);
    }

    public void OnClickSell()
    {
        Debug.Log("판매 버튼 눌림");

        if (currentTower != null)
        {
            Debug.Log("타워 제거 및 골드 환급");
            ResourceManager.Instance.Earn(currentTower.data.makeCost, currentTower.data.makeValue);
            Destroy(currentTower.gameObject);
            panel.SetActive(false);
        }
    }
}
