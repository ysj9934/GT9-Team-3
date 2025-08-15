using UnityEngine;

public class WaveDataManager : MonoBehaviour
{
    // �׽�Ʈ�� Key�� (�����ϴ� key�� �ٲټ���)
    public int testKey = 10102;

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
                if (spawnSquence != -1) // -1�̸� ���� ��
                    Debug.Log($"{spawnSquence} ���� : {spawnStartTime}�ʿ� {spawnerID} ���������� EnemyID_{i}�� {enemyID}�� ���Ͱ� " +
                        $"{spawnIntervalSec}�� �������� {spawnBatchSize}������ {spawnRepeat}�� ����");
            }
        }
        else
        {
            Debug.LogWarning($"Wave Spawn data not found for key: {testKey}");
        }
    }
}