using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfigManager : MonoBehaviour
{
    public static EnemyConfigManager Instance { get; private set; }

    private GameManager _gameManager;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public EnemyConfig GetConfig(int monsterID)
    {
        var jsonData = _gameManager._dataManager.EnemyDataLoader.GetByKey(monsterID);
        if (jsonData == null)
        {
            Debug.LogError($"몬스터 ID {monsterID}에 대한 JSON 데이터 없음");
            return null;
        }

        return CreateConfigFromJson(jsonData);
    }


    public EnemyConfig CreateConfigFromJson(Enemy_DataTable jsonData)
    {
        var config = ScriptableObject.CreateInstance<EnemyConfig>();
        config.keycode = jsonData.key;
        config.Enemy_Name = jsonData.Enemy_Name;
        config.maxHP = jsonData.MaxHP;
        config.movementSpeed = jsonData.MovementSpeed;

        config.enemyPrefab = Resources.Load<GameObject>($"Prefabs/Enemy/{jsonData.Enemy_Image}");
        if (config.enemyPrefab == null)
            Debug.LogError($"Enemy prefab not found for {jsonData.Enemy_Name}");

        return config;
    }

#if UNITY_EDITOR
    [ContextMenu("Test Enemy Config")]
    void TestConfig()
    {
        var testJson = new Enemy_DataTable { key = 1000, Enemy_Name = "기어다니는 굼벵이", MaxHP = 100, MovementSpeed = 3.5f };
        var config = CreateConfigFromJson(testJson);
        Debug.Log($"Created config for: {config.Enemy_Name}");
    }
#endif

}
