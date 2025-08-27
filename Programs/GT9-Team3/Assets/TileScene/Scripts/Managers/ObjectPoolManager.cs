using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public int enemyID;
        public int poolSize;
    }

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.SetActive(true);

            SetLayerRecursively(obj, LayerMask.NameToLayer("Enemy"));

            return obj;
        }
        return null;
    }

    public GameObject GetEnemy()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);

            SetLayerRecursively(obj, LayerMask.NameToLayer("Enemy"));

            return obj;
        }
        return null;
    }

    public void ReturnEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    // 원진 layer 재귀
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;
        Debug.Log($"Set Layer: {obj.name} → {LayerMask.LayerToName(newLayer)}");

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
