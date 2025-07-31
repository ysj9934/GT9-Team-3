using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTest : MonoBehaviour
{
    public GameObject enemyPrefab;

    // 적을 생성할 위치와 목표 위치를 Vector2 타입으로 정의
    // 수정될 변동값
    private Vector2 spawnPosition = new Vector2(-3f, 0f);
    private Vector2 targetPosition = new Vector2(5f, 0f);

    void Start()
    {
        GameObject enemy = SpawnEnemyAt(spawnPosition);

        // Enemy의 스크립트 컴포넌트 EnemyTest를 가져와 목표 위치를 설정
        EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
        enemyScript.targetPosition = targetPosition;

        Debug.Log("Enemy spawned at: " + spawnPosition + " → target: " + targetPosition);
    }

    //Vector2 타입의 position을 받아 유니티 엔진 오브젝트를 해당 위치에서 복제 생성하는 함수
    GameObject SpawnEnemyAt(Vector2 position)
    {
        return Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}