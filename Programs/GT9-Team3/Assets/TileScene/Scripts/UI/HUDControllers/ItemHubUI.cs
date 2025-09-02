using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHubUI : MonoBehaviour
{

    // Object Struture
    [SerializeField] private GameObject hudTabOffPanel;
    [SerializeField] private GameObject hudShopTabPanel;
    [SerializeField] private GameObject hudInventoryTabPanel;

    // Inventory content
    [SerializeField] public RectTransform inventoryContent;

    private void Awake()
    {
        CloseHubTab();
    }

    public void OpenShopTab()
    {
        hudTabOffPanel.SetActive(false);
        hudShopTabPanel.SetActive(true);
        hudInventoryTabPanel.SetActive(false);
    }

    public void OpenInventoryTab()
    {
        hudTabOffPanel.SetActive(false);
        hudShopTabPanel.SetActive(false);
        hudInventoryTabPanel.SetActive(true);
    }

    public void CloseHubTab()
    {
        hudTabOffPanel.SetActive(true);
        hudShopTabPanel.SetActive(false);
        hudInventoryTabPanel.SetActive(false);
    }

    // Shop Tab

}
