using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLink : MonoBehaviour
{
    public GameObject tileUIPrefab;     // UI 프리팹 원본
    public GameObject tileObjectPrefab; // 게임 오브젝트 프리팹 원본

    [HideInInspector] public GameObject linkedUIObject;
    [HideInInspector] public GameObject linkedWorldObject;
}
