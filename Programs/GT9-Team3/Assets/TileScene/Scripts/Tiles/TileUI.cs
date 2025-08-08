using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileUI : MonoBehaviour
{
    [SerializeField] private GameObject tileUI;

    private static TileUI currentOpenUI;
    
    private void Awake()
    {
        tileUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            ToggleUI();
    }

    private void ToggleUI()
    {
        if (currentOpenUI != null && currentOpenUI != this)
        {
            currentOpenUI.CloseUI();
        }

        bool isOpening = !tileUI.activeSelf;
        tileUI.SetActive(isOpening);
        
        currentOpenUI = isOpening ? this : null;
    }
    
    private void CloseUI()
    {
        tileUI.SetActive(false);
    }
}
