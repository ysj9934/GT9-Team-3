using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] public GameObject canvas;
    private SpriteRenderer[] spriteRenderers;
    private TextMeshPro textMesh;

    private void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        textMesh = GetComponent<TextMeshPro>();
    }

    private void OnMouseDown()
    {
        Debug.Log($"이 타일은 {textMesh.text}");
        
        canvas.SetActive(true);
    }

    private void OnMouseEnter()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = new Color(0,255, 0);
        }
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = new Color(255,255, 255);
        }
    }
}
