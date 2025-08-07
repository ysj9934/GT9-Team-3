using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate : MonoBehaviour
{
    [SerializeField] public TileRotation currentRotationIndex;
    [SerializeField] private GameObject[] rotetedPrefabs;
    
    // 다음 각도의 맵 타일 생성하기
    public void RotateLeft()
    {
        int newIndex = ((int)currentRotationIndex - 1 + 4) % 4;

        RotateTile(newIndex);
    }

    public void RotateRight()
    {
        int newIndex = ((int)currentRotationIndex + 1) % 4;

        RotateTile(newIndex);
    }
    
    public void RotateTile(int index)
    {
        Vector3 pos = transform.position;
        
        Destroy(gameObject);
        
        GameObject newTile = Instantiate(rotetedPrefabs[index], 
            pos, 
            Quaternion.identity);
        newTile.transform.SetParent(transform.parent);
        TileRoad tileRoad = newTile.GetComponent<TileRoad>();
        tileRoad.Initialize(tileRoad.mapLevel, pos);
        
        
    }

}