using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileUIInfo : MonoBehaviour
{
    [SerializeField] private TileRoad tileRoad;
    [SerializeField] private TextMeshProUGUI _cellSize;
    [SerializeField] private TextMeshProUGUI _tileSerialNumber;
    [SerializeField] private TextMeshProUGUI _tileCol;
    [SerializeField] private TextMeshProUGUI _tileCurrentRotationIndex;
    [SerializeField] private TextMeshProUGUI _connectedUp;
    [SerializeField] private TextMeshProUGUI _connectedDown;
    [SerializeField] private TextMeshProUGUI _connectedLeft;
    [SerializeField] private TextMeshProUGUI _connectedRight;
    
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        tileRoad = GetComponent<TileRoad>();

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
