using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHubUI : MonoBehaviour
{
    // Object Managers
    private ResourceManager _resourceManager;

    // Object Struture
    [SerializeField] private GameObject hudTabOffPanel;
    [SerializeField] private GameObject hudShopTabPanel;
    [SerializeField] private GameObject hudInventoryTabPanel;

    // Inventory
    [SerializeField] public RectTransform inventoryContent;
    [SerializeField] private GameObject hudTowerInfoPanel;
    [SerializeField] private TextMeshProUGUI hudTowerInfoText_Common;
    [SerializeField] private TextMeshProUGUI hudTowerInfoText_Splash;
    [SerializeField] private TextMeshProUGUI hudTowerInfoText_Slow;
    [SerializeField] private TextMeshProUGUI hudTowerInfoText_Stun;
    [SerializeField] private TextMeshProUGUI hudTowerInfoText_Doom;

    // Shop
    public ShopItemObject currentSelectedShopItem;
    [SerializeField] private GameObject hudShopItem_Corner;
    [SerializeField] private GameObject hudShopItem_TShape;
    [SerializeField] private GameObject hudShopItem_Straight;
    [SerializeField] private GameObject hudShopItem_Cross;
    [SerializeField] private GameObject hudShopItemBuyFalse;
    [SerializeField] private GameObject hudShopItemBuyTrue;
    public List<ShopItemObject> hudShopItemList = new List<ShopItemObject>();

    private void Awake()
    {
        CloseHubTab();
        CloseTowerInfoPanel();
        ClosePurchaseButton();
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
    }

    private bool isPurchaseButtonOpen = false;

    private void Update()
    {
        bool canPurchase = currentSelectedShopItem != null &&
                       _resourceManager.GetAmount(ResourceType.Tilepiece) >= 200 &&
                       currentSelectedShopItem.currentCount > 0;

        if (canPurchase && !isPurchaseButtonOpen)
        {
            OpenPurchaseButton();
            isPurchaseButtonOpen = true;
        }
        else if (!canPurchase && isPurchaseButtonOpen)
        {
            ClosePurchaseButton();
            isPurchaseButtonOpen = false;
        }


        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (hudTowerInfoPanel.activeSelf)
            {
                CloseTowerInfoPanel();
            }

            //if (currentSelectedShopItem != null)
            //{
            //    DisselectedItem();
            //}
        }
    }

    bool IsPointerOverUI(GameObject targetUI)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = GameUIManager.Instance.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == targetUI || result.gameObject.transform.IsChildOf(targetUI.transform))
                return true;
        }

        return false;
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

        currentSelectedShopItem = null;
        DisselectedItem();
    }

    public void CloseHubTab()
    {
        hudTabOffPanel.SetActive(true);
        hudShopTabPanel.SetActive(false);
        hudInventoryTabPanel.SetActive(false);

        currentSelectedShopItem = null;
        DisselectedItem();
    }

    // Inventory Tab
    private TileInfo currentTileInfo;
    public void SetTowerInfo(TileInfo tileInfo, Vector2 mousePos)
    {
        if (tileInfo != null)
            currentTileInfo = tileInfo;

        hudTowerInfoPanel.transform.position = mousePos + new Vector2(0, 240.5f);
    }

    public void OpenTowerInfoPanel()
    {
        if (currentTileInfo != null)
        {
            hudTowerInfoText_Common.text = $"일반 타워 {currentTileInfo.towerInfo[TowerCategory.Common] / 4}";
            hudTowerInfoText_Splash.text = $"스플래시 타워 {currentTileInfo.towerInfo[TowerCategory.Splash] / 4}";
            hudTowerInfoText_Slow.text = $"슬로우 타워 {currentTileInfo.towerInfo[TowerCategory.Slow] / 4}";
            hudTowerInfoText_Stun.text = $"스턴 타워 {currentTileInfo.towerInfo[TowerCategory.Stun] / 4}";
            hudTowerInfoText_Doom.text = $"파멸 타워 {currentTileInfo.towerInfo[TowerCategory.Doom] / 4}";
        }
        else
        {
            hudTowerInfoText_Common.text = $"일반 타워 {0}";
            hudTowerInfoText_Splash.text = $"스플래시 타워 {0}";
            hudTowerInfoText_Slow.text = $"슬로우 타워 {0}";
            hudTowerInfoText_Stun.text = $"스턴 타워 {0}";
            hudTowerInfoText_Doom.text = $"파멸 타워 {0}";
        }

        hudTowerInfoPanel.SetActive(true);
    }

    public void CloseTowerInfoPanel()
    {
        hudTowerInfoPanel.SetActive(false);
    }


    // Shop Tab
    public void OpenPurchaseButton()
    {
        hudShopItemBuyFalse.SetActive(false);
        hudShopItemBuyTrue.SetActive(true);
    }

    public void ClosePurchaseButton()
    {
        hudShopItemBuyFalse.SetActive(true);
        hudShopItemBuyTrue.SetActive(false);
    }

    public void DisselectedItem()
    { 
        foreach (var item in hudShopItemList)
        {
            item.DisabledShopItem();
        }

        currentSelectedShopItem = null;
    }

    public void PurchaseTile()
    {
        ShopController _shopController = GameManager.Instance._shopController;

        if (currentSelectedShopItem == null)
        { 
            DisselectedItem();
            return;
        }

        if (!_resourceManager.CanAfford(ResourceType.Tilepiece, 200)) return;

        _resourceManager.Spend(ResourceType.Tilepiece, 200);
        GameUIManager.Instance.canvasFixed.ResourcePanel.ShowTilePiece();

        TileShape tileShape = currentSelectedShopItem.itemShape;

        switch (tileShape)
        {
            case TileShape.Corner:
                _shopController.CreateCornerTile();
                break;
            case TileShape.TShape:
                _shopController.CreateTShapeTile();
                break;
            case TileShape.Straight:
                _shopController.CreateStraightTile();
                break;
            case TileShape.Cross:
                _shopController.CreateCrossTile();
                break;
        }

        currentSelectedShopItem.currentCount--;
        if (currentSelectedShopItem.currentCount < 1)
        {
            currentSelectedShopItem.Disabled();
        }

        GameUIManager.Instance.canvasPopup.toastMessage.FloatingUIShow(
            "[타일 구매 완료]",
            "타일을 구매 완료하셨습니다.", 
            Color.cyan
            );

        currentSelectedShopItem.UpdateItemCount();
    }

}
