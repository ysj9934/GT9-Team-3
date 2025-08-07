using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    [SerializeField] private GameObject tileUI;

    private void Awake()
    {
        tileUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        ToggleUI();
    }

    private void ToggleUI()
    {
        tileUI.SetActive(!tileUI.activeSelf);
    }
}
