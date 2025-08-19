using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TileUI
/// created by: yoon
/// created at: 2025.08.18
/// description:
/// TileUI
/// </summary>

public class TileUI1 : MonoBehaviour
{
    [SerializeField] public GameObject tileUI;

    private void Awake()
    {
        tileUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        Debug.Log("TileUI");
        
        tileUI.SetActive(!tileUI.activeSelf);
    }

    public void CloseUI()
    {
        tileUI.SetActive(false);
    }
}
