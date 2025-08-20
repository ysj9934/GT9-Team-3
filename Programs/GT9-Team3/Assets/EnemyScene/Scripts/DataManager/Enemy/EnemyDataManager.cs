using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    // �׽�Ʈ�� Key�� (�����ϴ� key�� �ٲټ���)
    public int testKey = 1000;

    private void Start()
    {
        // EnemyReader �̱��� ����
        if (EnemyDataReader.Instance == null)
        {
            Debug.LogError("EnemyReader instance not found!");
            return;
        }

        // Enemy Master Table ����
        var masterData = EnemyDataReader.Instance.GetEnemyMasterByKey(testKey);
        if (masterData != null)
        {
            Debug.Log($"[Master] Key: {masterData.key}, Name: {masterData.Enemy_Name}, ũ��: {masterData.Enemy_Size}, ����: {masterData.Enemy_Type}");
        }
        else
        {
            Debug.LogWarning($"Enemy Master data not found for key: {testKey}");
        }

        // Enemy Stat Table ����
        var statData = EnemyDataReader.Instance.GetEnemyStatByKey(testKey);
        if (statData != null)
        {
            Debug.Log($"[Stat] ü��: {statData.MaxHP}, �̵��ӵ� : {statData.MovementSpeed}, ���ݷ�: {statData.AttackDamage}," +
                $" ���ݼӵ�: {statData.AttackSpeed}, ��Ÿ�: {statData.AttackRange}, ��������: {statData.AttackType}, ProjectileID: {statData.ProjectileID}," +
                $"���: {statData.Defense}, TilePieceAmount: {statData.TilePieceAmount}, �����̻� ���� ����: {statData.IgnoreDebuff}, ��ų ID: {statData.Enemy_Skill_ID}");
        }
        else
        {
            Debug.LogWarning($"Enemy Stat data not found for key: {testKey}");
        }
    }
}