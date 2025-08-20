using System.Collections.Generic;
using UnityEngine;

public class EnemyDataReader : MonoBehaviour
{
    public static EnemyDataReader Instance { get; private set; }

    // 데이터 테이블 로더
    private Enemy_DataTable_EnemyMaster_DataTableLoader masterLoader;
    private Enemy_DataTable_EnemyStatTableLoader statLoader;

    private Dictionary<string, int> keyByImage; // Enemy_Image 기반 조회용

    private void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (Instance == null)
        {
            // 다른 스크립트에서 EnemyReader.Instance로 접근할 수 있게 됨
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 데이터 테이블 로드
        masterLoader = new Enemy_DataTable_EnemyMaster_DataTableLoader();
        statLoader = new Enemy_DataTable_EnemyStatTableLoader();

        //keyByName = new Dictionary<string, int>();
        keyByImage = new Dictionary<string, int>();

        // Enemy_Image 기반 조회용 딕셔너리를 초기화함
        if (masterLoader != null && masterLoader.ItemsList != null)
        {
            foreach (var stat in masterLoader.ItemsList)
            {
                if (!string.IsNullOrEmpty(stat.Enemy_Image))
                    keyByImage[stat.Enemy_Image] = stat.key;
            }
        }
    }

    #region Enemy Master Table 접근
    public Enemy_DataTable_EnemyMaster_DataTable GetEnemyMasterByKey(int key)
    {
        return masterLoader?.GetByKey(key);
    }

    public Enemy_DataTable_EnemyMaster_DataTable GetEnemyMasterByIndex(int index)
    {
        return masterLoader?.GetByIndex(index);
    }

    public List<Enemy_DataTable_EnemyMaster_DataTable> GetAllEnemyMasters()
    {
        return masterLoader != null ? new List<Enemy_DataTable_EnemyMaster_DataTable>(masterLoader.ItemsList) : null;
    }
    #endregion

    #region Enemy Stat Table 접근
    public Enemy_DataTable_EnemyStatTable GetEnemyStatByKey(int key)
    {
        return statLoader?.GetByKey(key);
    }

    public Enemy_DataTable_EnemyStatTable GetEnemyStatByIndex(int index)
    {
        return statLoader?.GetByIndex(index);
    }

    public List<Enemy_DataTable_EnemyStatTable> GetAllEnemyStats()
    {
        return statLoader != null ? new List<Enemy_DataTable_EnemyStatTable>(statLoader.ItemsList) : null;
    }
    #endregion

    //이미지 이름 기반 조회 함수
    public Enemy_DataTable_EnemyStatTable GetEnemyStatByImage(string image)
    {
        if (string.IsNullOrEmpty(image) || keyByImage == null)
            return null;

        if (keyByImage.TryGetValue(image, out int key))
            return GetEnemyStatByKey(key);

        return null;
    }
}