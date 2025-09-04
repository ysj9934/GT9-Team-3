using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomSettingManager : MonoBehaviour
{
    public static CustomSettingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        waveSettingPrefab.gameObject.SetActive(false);
    }

    // custom setting
    [SerializeField] public RectTransform customSettingPrefab;
    [SerializeField] public RectTransform customSettingButton;
    [SerializeField] public TextMeshProUGUI customSettingButtonText;
    public bool isOpen_CustomSetting = true;
    
    // waveSetting
    [SerializeField] public RectTransform waveSettingPrefab;
    [SerializeField] public RectTransform waveSettingButton;
    [SerializeField] public TextMeshProUGUI waveSettingButtonText;
    
    public void ToggleCustomSetting()
    {
        if (isOpen_CustomSetting)
        {
            customSettingPrefab.anchoredPosition = new Vector2(1130, 0);
            customSettingButton.anchoredPosition = new Vector2(-220, 430);
            customSettingButtonText.text = "<";
            isOpen_CustomSetting = false;
        }
        else
        {
            customSettingPrefab.anchoredPosition = new Vector2(785, 0);
            customSettingButton.anchoredPosition = new Vector2(-115, 430);
            customSettingButtonText.text = ">";
            isOpen_CustomSetting = true;
        }
    }
    
    public void OpenWaveSetting()
    {
        waveSettingPrefab.gameObject.SetActive(true);
        customSettingPrefab.gameObject.SetActive(false);
    }

    public void CloseWaveSetting()
    {
        waveSettingPrefab.gameObject.SetActive(false);
        customSettingPrefab.gameObject.SetActive(true);
    }

    // Create Tile
    public void CreateCornerTile()
    {
        TileManager.Instance._shopController.CreateCornerTile();
    }
    public void CreateCrossTile()
    {
        TileManager.Instance._shopController.CreateCrossTile();
    }
    public void CreateStraightTile()
    {
        TileManager.Instance._shopController.CreateStraightTile();
    }
    public void CreateTShapeTile()
    {
        TileManager.Instance._shopController.CreateTShapeTile();
    }
}
