using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesRotator : MonoBehaviour
{
    [SerializeField] private GameObject[] rotetedPrefabs;

    private int currentRotationIndex = 0;

    private void OnMouseDown()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Destroy(gameObject);
        
        currentRotationIndex = (currentRotationIndex + 1) % rotetedPrefabs.Length;

        Instantiate(rotetedPrefabs[currentRotationIndex], position, rotation);
    }
}
