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
    
    [SerializeField] public RectTransform customSettingPrefab;
    [SerializeField] public RectTransform customSettingButton;
    [SerializeField] public TextMeshProUGUI customSettingButtonText;
    private bool isOpen = false;
    
    public void ToggleTileInfo()
    {
        if (isOpen)
        {
            customSettingPrefab.anchoredPosition = new Vector2(1130, 0);
            customSettingButton.anchoredPosition = new Vector2(-220, 430);
            customSettingButtonText.text = "<";
            isOpen = false;
        }
        else
        {
            customSettingPrefab.anchoredPosition = new Vector2(785, 0);
            customSettingButton.anchoredPosition = new Vector2(-115, 430);
            customSettingButtonText.text = ">";
            isOpen = true;
        }
    }
}
