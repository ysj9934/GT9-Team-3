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
    private TileInfo1 _tileInfo;
    [SerializeField] public GameObject tileUI;

    private bool isActive = false;
    
    private void Awake()
    {
        tileUI.SetActive(false);
    }

    private void Start()
    {
        _tileInfo = GetComponent<TileInfo1>();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        _tileInfo._tileManager.CloseAllUI(this);
        
        tileUI.SetActive(!tileUI.activeSelf);
    }

    public void CloseUI()
    {
        tileUI.SetActive(false);
    }
}
