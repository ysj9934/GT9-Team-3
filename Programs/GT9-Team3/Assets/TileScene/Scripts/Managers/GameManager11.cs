using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager11 : MonoBehaviour
{
    public static GameManager11 Instance { get; private set; }
    public TileManager _tileManager;

    public int mapLevel = 1;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI _cellSize;
    [SerializeField] private TextMeshProUGUI _tileSerialNumber;
    [SerializeField] private TextMeshProUGUI _tileCol;
    [SerializeField] private TextMeshProUGUI _tileCurrentRotationIndex;
    [SerializeField] private TextMeshProUGUI _connectedUp;
    [SerializeField] private TextMeshProUGUI _connectedDown;
    [SerializeField] private TextMeshProUGUI _connectedLeft;
    [SerializeField] private TextMeshProUGUI _connectedRight;
    [SerializeField] public TileRoad tileRoad;
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

    public void ShowTileInfo()
    {
        _cellSize.text = $"cellSize : {tileRoad.cellSize}";
        _tileSerialNumber.text = $"_tileSerialNumber : {tileRoad.tileSerialNumber}";
        _tileCol.text = $"_tileCol : {tileRoad.col} / _tileRow : {tileRoad.row}";
        _tileCurrentRotationIndex.text = $"_tileCurrentRotationIndex : {tileRoad.currentRotationIndex}";
        _connectedUp.text = $"_connectedUp : {tileRoad.connectedUp}";
        _connectedDown.text = $"_connectedDown : {tileRoad.connectedDown}";
        _connectedLeft.text = $"_connectedLeft : {tileRoad.connectedLeft}";
        _connectedRight.text = $"_connectedRight : {tileRoad.connectedRight}";
    }



}
