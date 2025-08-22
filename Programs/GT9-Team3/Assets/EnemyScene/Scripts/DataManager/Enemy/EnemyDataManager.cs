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
        // 필요하면 씬 전환 시 유지
        // DontDestroyOnLoad(gameObject);
    }

    // 테스트용 Key값 (존재하는 key로 바꾸세요)
    public int testKey = 1000;

    public void PrintEnemyInfo(int key)
    {
        // EnemyReader 싱글턴 접근
        if (EnemyDataReader.Instance == null)
        {
            Debug.LogError("EnemyReader instance not found!");
            return;
        }

        // Enemy Master Table 접근
        var masterData = EnemyDataReader.Instance.GetEnemyMasterByKey(key);
        if (masterData != null)
        {
            Debug.Log($"적 ID : {masterData.key}, 이름 : {masterData.Enemy_Name}, 크기 : {masterData.Enemy_Size}, 유형 : {masterData.Enemy_Type}");
        }
        else
        {
            Debug.LogWarning($"Enemy Master data not found for key: {key}");
        }

        // Enemy Stat Table 접근
        var statData = EnemyDataReader.Instance.GetEnemyStatByKey(key);
        if (statData != null)
        {
            Debug.Log($"체력 : {statData.MaxHP}, 이동속도 : {statData.MovementSpeed}, 공격력 : {statData.AttackDamage}," +
                $" 공격속도 : {statData.AttackSpeed}, 사거리 : {statData.AttackRange}, 공격유형 : {statData.AttackType}, 발사체 ID : {statData.ProjectileID}," +
                $"방어 : {statData.Defense}, 타일 조각 보상 개수 : {statData.TilePieceAmount}, 상태이상 적용 여부 : {statData.IgnoreDebuff}, 스킬 ID : {statData.Enemy_Skill_ID}");
        }
        else
        {
            Debug.LogWarning($"{key}에 해당하는 적 데이터가 없어요");
        }
    }
}