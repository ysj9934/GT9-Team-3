using UnityEngine;
using System.Collections;

public class WaveDataManager : MonoBehaviour
{
    
    [SerializeField] private int testKey;
    public GameObject[] enemyPrefabs; // EnemyID�� ���� ���� ������ �迭
    //public Transform[] spawnerTransforms; // SpawnerID�� ���� ��ġ�� ���� ��ġ��

    private void Awake()
    {
        // �׽�Ʈ�� Key�� (�����ϴ� key�� �ٲټ���)
        testKey = 10104;  // �ν����� �� �����ϰ� �ڵ� �� ���� ����
    }

    private void Start()
    {
        // WaveReader �̱��� ����
        if (WaveDataReader.Instance == null)
        {
            Debug.LogError("���̺� ������ ����");
            return;
        }

        // Wave Master Table ����
        var masterData = WaveDataReader.Instance.GetWaveMasterByKey(testKey);
        if (masterData != null)
        {
            Debug.Log($"���̺� ���� : {masterData.key}, �̸�: {masterData.Inner_Name}, {masterData.RoundIndex}������ {masterData.WaveInRound}���̺�");
        }
        else
        {
            Debug.LogWarning($"�ش� Ű�� ã�� �� ���� : {testKey}");
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
                    Debug.Log($"{spawnSquence} ���� : {spawnStartTime}�ʺ��� {spawnerID} ���������� EnemyID_{i}�� {enemyID}�� ���Ͱ� " +
                              $"{spawnIntervalSec}�� �������� {spawnBatchSize}������ {spawnRepeat}�� ����");

                    EnemyDataManager.Instance.PrintEnemyInfo(enemyID);
                    // ���� �ڷ�ƾ ����
                    StartCoroutine(SpawnEnemiesCoroutine(
                        spawnStartTime, spawnerID, enemyID, spawnBatchSize, spawnRepeat, spawnIntervalSec
                    ));
                }
            }
        }
        else
        {
            Debug.LogWarning($"{testKey}�� �ش��ϴ� ���̺� ���� �����Ͱ� �����");
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
        //if (spawnerID < 0 || spawnerID >= spawnerTransforms.Length)
        //{
        //    Debug.LogWarning($"������ {spawnerID}�� ��ȿ���� �ʾƿ�");
        //    return;
        //}
        //if (enemyID < 0 || enemyID >= enemyPrefabs.Length)
        //{
        //    Debug.LogWarning($"�� {enemyID}�� ��ȿ���� �ʾƿ�");
        //    return;
        //}

        GameObject prefab = EnemyDataReader.Instance.GetPrefabByKey(enemyID);
        if (prefab == null)
        {
            Debug.LogWarning($"EnemyID {enemyID}�� �ش��ϴ� �������� ã�� �� ����");
            return;
        }

        if (TileManager.Instance == null || TileManager.Instance.startTile == null)
        {
            Debug.LogWarning("TileManager �Ǵ� startTile�� ����ֽ��ϴ�. ���� �Ұ�");
            return;
        }

        Transform spawnPoint = TileManager.Instance.startTile.transform;
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"�����ʿ��� �� {enemyID} ����");
    }
}