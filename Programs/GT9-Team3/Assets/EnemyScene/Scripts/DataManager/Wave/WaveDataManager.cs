using UnityEngine;
using System.Collections;

public class WaveDataManager : MonoBehaviour
{
    // �׽�Ʈ�� Key�� (�����ϴ� key�� �ٲټ���)
    public int testKey = 10101;
    public GameObject[] enemyPrefabs; // EnemyID�� ���� ���� ������ �迭
    public Transform[] spawnerTransforms; // SpawnerID�� ���� ��ġ�� ���� ��ġ��

    private void Start()
    {
        // WaveReader �̱��� ����
        if (WaveDataReader.Instance == null)
        {
            Debug.LogError("WaveReader instance not found!");
            return;
        }

        // Wave Master Table ����
        var masterData = WaveDataReader.Instance.GetWaveMasterByKey(testKey);
        if (masterData != null)
        {
            Debug.Log($"[Master] Key: {masterData.key}, Name: {masterData.Inner_Name}, {masterData.RoundIndex}������ {masterData.WaveInRound}���̺�");
        }
        else
        {
            Debug.LogWarning($"Wave Master data not found for key: {testKey}");
        }

        // Wave Spawn Table ����
        var spawnData = WaveDataReader.Instance.GetWaveSpawnByKey(testKey);
        if (spawnData != null)
        {
            //Debug.Log($"[Spawn] Key(������ȣ): {spawnData.key}, EnemyID_01: {spawnData.EnemyID_01}, SpawnerID_01: {spawnData.SpawnerID_01}");
            for (int i = 1; i <= 5; i++)
            {
                int spawnSquence = spawnData.GetSpawnSquence(i);
                float spawnStartTime = spawnData.GetSpawnStartTime(i);
                int spawnerID = spawnData.GetSpawnerID(i);
                int enemyID = spawnData.GetEnemyID(i);
                int spawnBatchSize = spawnData.GetSpawnBatchSize(i);
                int spawnRepeat = spawnData.GetSpawnRepeat(i);
                float spawnIntervalSec = spawnData.GetSpawnIntervalSec(i);

                if (spawnSquence != -1 && enemyID != -1)    //�ش� ������(����)�� �����ϰ� ���� �����ϴٸ�
                {
                    Debug.Log($"{spawnSquence} ���� : {spawnStartTime}�ʿ� {spawnerID} ���������� EnemyID_{i}�� {enemyID}�� ���Ͱ� " +
                              $"{spawnIntervalSec}�� �������� {spawnBatchSize}������ {spawnRepeat}�� ����");

                    // ���� �ڷ�ƾ ����
                    StartCoroutine(SpawnEnemiesCoroutine(
                        spawnStartTime, spawnerID, enemyID, spawnBatchSize, spawnRepeat, spawnIntervalSec
                    ));
                }
            }
        }
        else
        {
            Debug.LogWarning($"Wave Spawn data not found for key: {testKey}");
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(
        float startTime, int spawnerID, int enemyID, int batchSize, int repeatCount, float intervalSec)
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(startTime);

        // �ݺ� ����
        for (int repeat = 0; repeat < repeatCount; repeat++)
        {
            for (int j = 0; j < batchSize; j++)
            {
                SpawnEnemy(spawnerID, enemyID);
            }

            // ���� �ݺ� �� ���
            if (repeat < repeatCount - 1)
                yield return new WaitForSeconds(intervalSec);
        }
    }

    private void SpawnEnemy(int spawnerID, int enemyID)
    {
        if (spawnerID < 0 || spawnerID >= spawnerTransforms.Length)
        {
            Debug.LogWarning($"������ {spawnerID}�� ��ȿ���� �ʾƿ�");
            return;
        }
        if (enemyID < 0 || enemyID >= enemyPrefabs.Length)
        {
            Debug.LogWarning($"�� {enemyID}�� ��ȿ���� �ʾƿ�");
            return;
        }

        Transform spawnPoint = spawnerTransforms[spawnerID];
        GameObject prefab = enemyPrefabs[enemyID];

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"{enemyID} �����ʿ��� {spawnerID} �� ����");
    }
}