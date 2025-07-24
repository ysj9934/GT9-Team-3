using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T _prefab;
    private Transform _parent;
    private Queue<T> _pool = new Queue<T>();

    public ObjectPool(T prefab, int initialCount, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            T obj = GameObject.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public T Get()
    {
        if (_pool.Count == 0) return null;

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public int countInUse => 10 - _pool.Count;
}
