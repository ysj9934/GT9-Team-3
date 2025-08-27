using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // �ʿ��ϸ� �� ��ȯ �� ����
        // DontDestroyOnLoad(gameObject);
    }

    // �׽�Ʈ�� Key�� (�����ϴ� key�� �ٲټ���)
    public int testKey = 1000;

    public void PrintEnemyInfo(int key)
    {
        // EnemyReader �̱��� ����
        if (EnemyDataReader.Instance == null)
        {
            Debug.LogError("EnemyReader instance not found!");
            return;
        }

        // Enemy Master Table ����
        var masterData = EnemyDataReader.Instance.GetEnemyMasterByKey(key);
        if (masterData != null)
        {
            Debug.Log($"�� ID : {masterData.key}, �̸� : {masterData.Enemy_Name}, ũ�� : {masterData.Enemy_Size}, ���� : {masterData.Enemy_Type}");
        }
        else
        {
            Debug.LogWarning($"Enemy Master data not found for key: {key}");
        }

        // Enemy Stat Table ����
        var statData = EnemyDataReader.Instance.GetEnemyStatByKey(key);
        if (statData != null)
        {
            Debug.Log($"ü�� : {statData.MaxHP}, �̵��ӵ� : {statData.MovementSpeed}, ���ݷ� : {statData.AttackDamage}," +
                $" ���ݼӵ� : {statData.AttackSpeed}, ��Ÿ� : {statData.AttackRange}, �������� : {statData.AttackType}, �߻�ü ID : {statData.ProjectileID}," +
                $"��� : {statData.Defense}, Ÿ�� ���� ���� ���� : {statData.TilePieceAmount}, �����̻� ���� ���� : {statData.IgnoreDebuff}, ��ų ID : {statData.Enemy_Skill_ID}");
        }
        else
        {
            Debug.LogWarning($"{key}�� �ش��ϴ� �� �����Ͱ� �����");
        }
    }
}