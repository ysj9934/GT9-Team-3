using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesRotator : MonoBehaviour
{
    [SerializeField] public TileRotation currentRotationIndex; 
    [SerializeField] private GameObject[] rotetedPrefabs;

    // 다음 각도의 맵 타일 생성하기

    public void RotateLeft()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Destroy(gameObject);

        currentRotationIndex = GetNextRotation(currentRotationIndex);
        int nextIndex = (int)currentRotationIndex;
        
        GameObject newTile = Instantiate(rotetedPrefabs[nextIndex], position, rotation);
        newTile.GetComponent<TilesRotator>().currentRotationIndex = currentRotationIndex;
    }

    public void RotateRight()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Destroy(gameObject);

        currentRotationIndex = GetPreRotation(currentRotationIndex);
        int nextIndex = (int)currentRotationIndex;
        
        GameObject newTile = Instantiate(rotetedPrefabs[nextIndex], position, rotation);
        newTile.GetComponent<TilesRotator>().currentRotationIndex = currentRotationIndex;
    }

    private TileRotation GetNextRotation(TileRotation rotationIndex)
    {
        return (TileRotation)(((int)rotationIndex + 1) % 4);
    }
    
    private TileRotation GetPreRotation(TileRotation rotationIndex)
    {
        return (TileRotation)(((int)rotationIndex + 1) % 4);
    }
}
