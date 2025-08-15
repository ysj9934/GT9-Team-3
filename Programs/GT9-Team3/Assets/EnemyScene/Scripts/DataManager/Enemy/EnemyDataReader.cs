using System.Collections.Generic;
using UnityEngine;

public class EnemyDataReader : MonoBehaviour
{
    public static EnemyDataReader Instance { get; private set; }

    // ������ ���̺� �δ�
    private Enemy_DataTable_EnemyMaster_DataTableLoader masterLoader;
    private Enemy_DataTable_EnemyStatTableLoader statLoader;

    private Dictionary<string, int> keyByImage; // Enemy_Image ��� ��ȸ��

    private void Awake()
    {
        // �̱��� �ʱ�ȭ �� �ߺ� ����
        if (Instance == null)
        {
            // �ٸ� ��ũ��Ʈ���� EnemyReader.Instance�� ������ �� �ְ� ��
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ������ ���̺� �ε�
        masterLoader = new Enemy_DataTable_EnemyMaster_DataTableLoader();
        statLoader = new Enemy_DataTable_EnemyStatTableLoader();

        //keyByName = new Dictionary<string, int>();
        keyByImage = new Dictionary<string, int>();

        // Enemy_Image ��� ��ȸ�� ��ųʸ��� �ʱ�ȭ��
        if (masterLoader != null && masterLoader.ItemsList != null)
        {
            foreach (var stat in masterLoader.ItemsList)
            {
                if (!string.IsNullOrEmpty(stat.Enemy_Image))
                    keyByImage[stat.Enemy_Image] = stat.key;
            }
        }
    }

    #region Enemy Master Table ����
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

    #region Enemy Stat Table ����
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

    //�̹��� �̸� ��� ��ȸ �Լ�
    public Enemy_DataTable_EnemyStatTable GetEnemyStatByImage(string image)
    {
        if (string.IsNullOrEmpty(image) || keyByImage == null)
            return null;

        if (keyByImage.TryGetValue(image, out int key))
            return GetEnemyStatByKey(key);

        return null;
    }
}