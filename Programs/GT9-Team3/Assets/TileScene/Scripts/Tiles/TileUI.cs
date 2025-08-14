using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileUI : MonoBehaviour
{
    private TileManager _tileManager;
    private TileRoad _tileRoad;
    private BlockInfo[] _blockInfos;
    [SerializeField] private GameObject tileUI;

    // �ӽ� ī�޶� ��ġ
    // Temporary camera position
    [SerializeField] private CinemachineVirtualCamera tileCamera;
    private static TileUI currentOpenUI;
    
    private void Awake()
    {
        _tileManager = TileManager.Instance;
        _tileRoad = GetComponent<TileRoad>();
        _blockInfos = GetComponentsInChildren<BlockInfo>();

        tileUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!_tileManager.isTileEditMode)
        {
            Debug.Log("It is not TIleEditMode");
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
            ToggleUI();
        //ToggleUI();


        // Ÿ�� ���� �� ��� ���� Collider2D Ȱ��ȭ
        // Enable Collider2D on block info when tile is selected
        _tileRoad.isSelected = true;
        if (_tileRoad.isSelected)
        {
            foreach (var blockInfo in _blockInfos)
            {
                if (blockInfo._collider2D != null)
                {
                    blockInfo._collider2D.enabled = true;
                }
            }
        }

        _tileRoad._collider2D.enabled = false; // Ÿ�� ���� �� Ÿ�� Collider2D ��Ȱ��ȭ

        // Ÿ�� ī�޶� �۵�
        // Activate tile camera
        tileCamera.Priority = 21;
    }

    public void ToggleUI()
    {
        bool isOpening = !tileUI.activeSelf;
        tileUI.SetActive(isOpening);
        currentOpenUI = isOpening ? this : null;

        if (currentOpenUI != null && currentOpenUI != this)
        {
            currentOpenUI.CloseUI();
        }
    }
    
    public void CloseUI()
    {
        tileUI.SetActive(false);
        // Ÿ�� ī�޶� ��Ȱ��ȭ
        // Deactivate tile camera
        tileCamera.Priority = 0;
        _tileRoad.isSelected = false;
        _tileRoad._collider2D.enabled = true; // Ÿ�� ���� ���� �� Ÿ�� Collider2D Ȱ��ȭ
    }
}
