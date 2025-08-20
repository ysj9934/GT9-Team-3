using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    // 테스트용 Key값 (존재하는 key로 바꾸세요)
    public int testKey = 1000;

    private void Start()
    {
        // EnemyReader 싱글턴 접근
        if (EnemyDataReader.Instance == null)
        {
            Debug.LogError("EnemyReader instance not found!");
            return;
        }

        // Enemy Master Table 접근
        var masterData = EnemyDataReader.Instance.GetEnemyMasterByKey(testKey);
        if (masterData != null)
        {
            Debug.Log($"[Master] Key: {masterData.key}, Name: {masterData.Enemy_Name}, 크기: {masterData.Enemy_Size}, 유형: {masterData.Enemy_Type}");
        }
        else
        {
            Debug.LogWarning($"Enemy Master data not found for key: {testKey}");
        }

        // Enemy Stat Table 접근
        var statData = EnemyDataReader.Instance.GetEnemyStatByKey(testKey);
        if (statData != null)
        {
            Debug.Log($"[Stat] 체력: {statData.MaxHP}, 이동속도 : {statData.MovementSpeed}, 공격력: {statData.AttackDamage}," +
                $" 공격속도: {statData.AttackSpeed}, 사거리: {statData.AttackRange}, 공격유형: {statData.AttackType}, ProjectileID: {statData.ProjectileID}," +
                $"방어: {statData.Defense}, TilePieceAmount: {statData.TilePieceAmount}, 상태이상 적용 여부: {statData.IgnoreDebuff}, 스킬 ID: {statData.Enemy_Skill_ID}");
        }
        else
        {
            Debug.LogWarning($"Enemy Stat data not found for key: {testKey}");
        }
    }
}