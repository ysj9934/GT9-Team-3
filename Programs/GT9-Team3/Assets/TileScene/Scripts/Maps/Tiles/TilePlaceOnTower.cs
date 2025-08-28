using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlaceOnTower : MonoBehaviour
{
    private TileInfo _tileInfo;
    
    private bool _isTowerPlaced;

    public Dictionary<int, List<(int prefabIndex, int blockIndex)>> towerBlockMap = new Dictionary<int, List<(int, int)>>()
    {
        // case 11, 27, 39, 43
        { 11, new List<(int, int)> { (0, 0), (1, 6), (2, 8), (3, 2) } },
        { 27, new List<(int, int)> { (0, 0), (1, 6), (2, 8), (3, 2) } },
        { 39, new List<(int, int)> { (0, 0), (1, 6), (2, 8), (3, 2) } },
        { 43, new List<(int, int)> { (0, 0), (1, 6), (2, 8), (3, 2) } },
        
        // case 12, 24, 38, 46
        { 12, new List<(int, int)> { (0, 1), (1, 3), (2, 7), (3, 5) } },
        { 24, new List<(int, int)> { (0, 1), (1, 3), (2, 7), (3, 5) } },
        { 38, new List<(int, int)> { (0, 1), (1, 3), (2, 7), (3, 5) } },
        { 46, new List<(int, int)> { (0, 1), (1, 3), (2, 7), (3, 5) } },

        // case 13, 21, 37, 49
        { 13, new List<(int, int)> { (0, 2), (1, 0), (2, 6), (3, 8) } },
        { 21, new List<(int, int)> { (0, 2), (1, 0), (2, 6), (3, 8) } },
        { 37, new List<(int, int)> { (0, 2), (1, 0), (2, 6), (3, 8) } },
        { 49, new List<(int, int)> { (0, 2), (1, 0), (2, 6), (3, 8) } },

        // case 14, 28, 36, 42
        { 14, new List<(int, int)> { (0, 3), (1, 7), (2, 5), (3, 1) } },
        { 28, new List<(int, int)> { (0, 3), (1, 7), (2, 5), (3, 1) } },
        { 36, new List<(int, int)> { (0, 3), (1, 7), (2, 5), (3, 1) } },
        { 42, new List<(int, int)> { (0, 3), (1, 7), (2, 5), (3, 1) } },

        // case 16, 22, 34, 48
        { 16, new List<(int, int)> { (0, 5), (1, 1), (2, 3), (3, 7) } },
        { 22, new List<(int, int)> { (0, 5), (1, 1), (2, 3), (3, 7) } },
        { 34, new List<(int, int)> { (0, 5), (1, 1), (2, 3), (3, 7) } },
        { 48, new List<(int, int)> { (0, 5), (1, 1), (2, 3), (3, 7) } },

        // case 17, 29, 33, 41
        { 17, new List<(int, int)> { (0, 6), (1, 8), (2, 2), (3, 0) } },
        { 29, new List<(int, int)> { (0, 6), (1, 8), (2, 2), (3, 0) } },
        { 33, new List<(int, int)> { (0, 6), (1, 8), (2, 2), (3, 0) } },
        { 41, new List<(int, int)> { (0, 6), (1, 8), (2, 2), (3, 0) } },

        // case 18, 26, 32, 44
        { 18, new List<(int, int)> { (0, 7), (1, 5), (2, 1), (3, 3) } },
        { 26, new List<(int, int)> { (0, 7), (1, 5), (2, 1), (3, 3) } },
        { 32, new List<(int, int)> { (0, 7), (1, 5), (2, 1), (3, 3) } },
        { 44, new List<(int, int)> { (0, 7), (1, 5), (2, 1), (3, 3) } },

        // case 19, 23, 31, 47
        { 19, new List<(int, int)> { (0, 8), (1, 2), (2, 0), (3, 6) } },
        { 23, new List<(int, int)> { (0, 8), (1, 2), (2, 0), (3, 6) } },
        { 31, new List<(int, int)> { (0, 8), (1, 2), (2, 0), (3, 6) } },
        { 47, new List<(int, int)> { (0, 8), (1, 2), (2, 0), (3, 6) } },
        
    }; 
    
    private void Start()
    {
        _tileInfo = GetComponent<TileInfo>();
    }
    
    public void HandleTowerPlacement(int blockCase, bool hasTower, TowerBlueprint bp, Tower1 tower)
    {
        int SerialNumber = ((int)_tileInfo.tileDirector + 1) * 10 + blockCase;
        
        if (towerBlockMap.TryGetValue(SerialNumber, out var blockList))
        {
            foreach (var (prefabIndex, blockIndex) in blockList)
            {
                if (!hasTower)
                    _tileInfo.blockInfos[prefabIndex][blockIndex].TowerInstall(bp);
                else
                    _tileInfo.blockInfos[prefabIndex][blockIndex].TowerRemove(tower);
            }

        }
        else
        {
            Debug.Log($"No tower placement defined for blockCase {SerialNumber}");
        }
    }

}
