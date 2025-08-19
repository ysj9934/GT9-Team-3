using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager12 : MonoBehaviour
{
    public static GameManager12 Instance { get; private set; }
    public TileManager1 _tileManager;

    // level
    public int worldLevel = 1;
    public int tempLevel = 1;
    [SerializeField] public TextMeshProUGUI worldLevelText;
    
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
        _tileManager = TileManager1.Instance;
    }


    public void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }
    
    public void UpdateWorldLevel(int level)
    {
        this.worldLevel = level;
        worldLevelText.text = level.ToString();
        _tileManager.UpdateWorldLevel(this.worldLevel);
    }

    public void UpdateTempLevel(int level)
    {
        this.tempLevel = level;
        
        _tileManager.UpdateTempLevel(this.tempLevel);
    }


}
