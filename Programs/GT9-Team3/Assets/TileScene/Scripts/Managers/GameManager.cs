using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileManager _tileManager;

    public int mapLevel = 1;
    [SerializeField] private TextMeshProUGUI levelText;

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
        _tileManager = TileManager.Instance;
    }

    public void UpdateMapLevel(int mapLevel)
    {
        this.mapLevel = mapLevel;

        _tileManager.mapLevel = mapLevel;
        levelText.text = $"{this.mapLevel}";
    }


    public void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }

}
