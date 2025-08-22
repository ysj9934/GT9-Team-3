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

public class TileUI : MonoBehaviour
{
    private TileInfo _tileInfo;
    [SerializeField] public GameObject tileUI;

    private bool isActive = false;
    
    private void Awake()
    {
        tileUI.SetActive(false);
    }

    private void Start()
    {
        _tileInfo = GetComponent<TileInfo>();
    }

     private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        _tileInfo._tileManager.CloseAllUI(this);

        tileUI.SetActive(!tileUI.activeSelf);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject())
    //            return;
            
    //        int layerMask = LayerMask.GetMask("Ground");
            
    //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
            
    //        // Debug.DrawRay(mousePos, Vector2.zero, Color.red, 1f);
    //        //
    //        // Debug.Log("Mouse Pos: " + mousePos);
    //        // if (hit.collider != null)
    //        // {
    //        //     Debug.Log("Hit: " + hit.collider.name);
    //        // }
    //        // else
    //        // {
    //        //     Debug.Log("No hit detected");
    //        // }

    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Hit: " + hit.collider.name);
                
    //            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
    //            if (tileInfo != null)
    //            {
    //                _tileInfo._tileManager.CloseAllUI(this);
    //                this.tileUI.SetActive(!tileUI.activeSelf);
    //            }
    //        }
    //        else
    //        {
    //            _tileInfo._tileManager.CloseAllUI(null);
    //        }
    //    }
    //}

    public void CloseUI()
    {
        tileUI.SetActive(false);
    }
}
