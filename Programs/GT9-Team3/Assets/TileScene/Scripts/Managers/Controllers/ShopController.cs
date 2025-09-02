using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{

    // Object Managers
    private HUDCanvas _hudCanvas;


    // Object Prefabs
    public GameObject[] tilePrefabsCorner;
    public GameObject[] tilePrefabsCross;
    public GameObject[] tilePrefabsStraight;
    public GameObject[] tilePrefabsTShape;

    // Object Route
    public RectTransform inventoryContent;

    private void Start()
    {
        _hudCanvas = HUDCanvas.Instance;
    }


    public void CreateCornerTile()
    {
        float[] weights = { 0f, 0.15f, 0.45f, 0.30f, 0.10f };

        int selectedIndex = CalculateRandomIndex(weights);

        GameObject tileItem = Instantiate(
            tilePrefabsCorner[selectedIndex], 
            _hudCanvas._itemHudUI.inventoryContent);

        ItemSetting(tileItem);

        Debug.Log($"Create new Tile type: corner (index: {selectedIndex})");
    }

    public void CreateCrossTile()
    {
        float[] weights = { 0f, 0.2f, 0.5f, 0.30f };

        int selectedIndex = CalculateRandomIndex(weights);

        GameObject tileItem = Instantiate(
            tilePrefabsCross[selectedIndex],
            _hudCanvas._itemHudUI.inventoryContent);

        ItemSetting(tileItem);

        Debug.Log($"Create new Tile type: cross (index: {selectedIndex})");
    }

    public void CreateStraightTile()
    {
        float[] weights = { 0f, 0.15f, 0.45f, 0.30f, 0.10f };

        int selectedIndex = CalculateRandomIndex(weights);

        GameObject tileItem = Instantiate(
            tilePrefabsStraight[selectedIndex],
            _hudCanvas._itemHudUI.inventoryContent);

        ItemSetting(tileItem);

        Debug.Log($"Create new Tile type: straight (index: {selectedIndex})");
    }

    public void CreateTShapeTile()
    {
        float[] weights = { 0f, 0.15f, 0.45f, 0.30f, 0.10f };

        int selectedIndex = CalculateRandomIndex(weights);

        GameObject tileItem = Instantiate(
            tilePrefabsTShape[selectedIndex],
            _hudCanvas._itemHudUI.inventoryContent);

        ItemSetting(tileItem);

        Debug.Log($"Create new Tile type: tShape (index: {selectedIndex})");
    }

    private void ItemSetting(GameObject tileItem)
    {
        var tileImage = tileItem.GetComponent<TileUIObject>();
        var tileLink = tileItem.GetComponent<TileLink>();

        tileImage.Initialize(inventoryContent);
        tileImage.link = tileLink;

        tileLink.linkedUIObject = tileItem;
    }

    private int CalculateRandomIndex(float[] weights)
    {
        float total = weights.Sum();
        float randomValue = Random.Range(0, total);

        int selectedIndex = 0;
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        return selectedIndex;
    }
}
