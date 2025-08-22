using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerRange : MonoBehaviour
{

    [SerializeField] private int sortingOrder;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

}
