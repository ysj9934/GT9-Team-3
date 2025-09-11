using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    // Object Structure
    public EnemyConfigController enemyConfigController;

    // Object Data
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 70;

    // Object Info
    public Queue<GameObject> pool = new();

    void Awake()
    {
        Instance = this;
        enemyConfigController = GetComponentInChildren<EnemyConfigController>();
        Init();
    }

    /// <summary>
    /// 적 유닛 초기화 
    /// </summary>
    /// <remarks>
    /// 2025.09.09 v1.3.0 최대 적 유닛 수 70으로 변경
    /// </remarks>
    public void Init()
    {
        if (poolSize < 1)
        {
            Debug.LogError("PoolSize가 너무 작습니다.");
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            if (obj != null)
            {
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            else
            {
                Debug.LogError("enemyPrefab이 존재하지 않습니다.");
            }
        }
    }

    /// <summary>
    /// 적 유닛 정보 가져오기
    /// </summary>
    /// <returns>
    /// GameObject 적 유닛 정보
    /// </returns>
    public GameObject GetEnemy()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                SetLayerRecursively(obj, LayerMask.NameToLayer("Enemy"));

                return obj;
            }
            else
            {
                Debug.LogError("잔여 pool이 없습니다.");

                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// 적 유닛 정보 반환
    /// </summary>
    public void ReturnEnemy(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        else
        { 
            Debug.LogWarning("반환할 오브젝트가 없습니다.");
        }
        
    }

    // 원진 layer 재귀
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
