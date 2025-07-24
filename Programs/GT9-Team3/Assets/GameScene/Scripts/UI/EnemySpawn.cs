using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;

    // (Test) 적 유닛 스폰
    public void OnClickSpawn()
    {
        Instantiate(enemyPrefab, enemyParent);
    }
}
