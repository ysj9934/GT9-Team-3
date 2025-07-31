using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTest : MonoBehaviour
{
    public GameObject enemyPrefab;

    // ���� ������ ��ġ�� ��ǥ ��ġ�� Vector2 Ÿ������ ����
    // ������ ������
    private Vector2 spawnPosition = new Vector2(-3f, 0f);
    private Vector2 targetPosition = new Vector2(5f, 0f);

    void Start()
    {
        GameObject enemy = SpawnEnemyAt(spawnPosition);

        // Enemy�� ��ũ��Ʈ ������Ʈ EnemyTest�� ������ ��ǥ ��ġ�� ����
        EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
        enemyScript.targetPosition = targetPosition;

        Debug.Log("Enemy spawned at: " + spawnPosition + " �� target: " + targetPosition);
    }

    //Vector2 Ÿ���� position�� �޾� ����Ƽ ���� ������Ʈ�� �ش� ��ġ���� ���� �����ϴ� �Լ�
    GameObject SpawnEnemyAt(Vector2 position)
    {
        return Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}