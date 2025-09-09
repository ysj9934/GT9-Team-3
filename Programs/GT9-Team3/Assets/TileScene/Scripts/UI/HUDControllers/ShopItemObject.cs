using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemObject : MonoBehaviour
{
    private HUDCanvas _hudCanvas;

    // Object Structure
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private GameObject SelectedImage;
    [SerializeField] private GameObject blindShopItem;

    // Object Info
    [SerializeField] public TileShape itemShape;
    [SerializeField] public int maxCount = 2;
    public int currentCount = 0;
    public bool isSelected = false;
    public bool isDisabled = false;

    private void Awake()
    {
        currentCount = maxCount;
        Enabled();
        DisabledShopItem();
        UpdateItemCount();
    }

    private void Start()
    {
        _hudCanvas = GetComponentInParent<HUDCanvas>();
    }

    public void SelelctedShopItem()
    {
        _hudCanvas._itemHudUI.DisselectedItem();

        _hudCanvas._itemHudUI.currentSelectedShopItem = this;
        SelectedImage.SetActive(true);
    }

    public void DisabledShopItem()
    {
        SelectedImage.SetActive(false);
    }

    public void UpdateItemCount()
    {
        itemCountText.text = $"{currentCount} / {maxCount}";
    }

    public void Enabled()
    {
        blindShopItem.SetActive(false);
    }

    public void Disabled()
    {
        blindShopItem.SetActive(true);
    }    
}
