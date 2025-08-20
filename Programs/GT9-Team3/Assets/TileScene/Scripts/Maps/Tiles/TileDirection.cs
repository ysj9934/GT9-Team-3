using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// TileDirection
/// create by : yoons
/// create at : 2025.08.19
/// </summary>
public class TileDirection : MonoBehaviour
{
    private TileInfo _tileInfo;
    
    [SerializeField] public TileDirector tileDirector;
    
    [SerializeField] public bool connectedUp;
    [SerializeField] public bool connectedDown;
    [SerializeField] public bool connectedLeft;
    [SerializeField] public bool connectedRight;

    private void Awake()
    {
        if (_tileInfo == null)
            _tileInfo = GetComponentInParent<TileInfo>();
        
        if (tileDirector != TileDirector.Degree_0)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _tileInfo.tileDirector = this.tileDirector;
        _tileInfo.connectedUp = this.connectedUp;
        _tileInfo.connectedDown = this.connectedDown;
        _tileInfo.connectedLeft = this.connectedLeft;
        _tileInfo.connectedRight = this.connectedRight;
    }
}
