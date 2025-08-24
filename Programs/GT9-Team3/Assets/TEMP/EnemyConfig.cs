using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : ScriptableObject
{
    public int keycode;
    public string Enemy_Name;
    public int maxHP;
    public float movementSpeed;
    public GameObject enemyPrefab;
}
