using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy11 : MonoBehaviour
{
    TileManager _tileManager;
    
    public List<TileRoad> _routeManager;
    private Transform[] routePoints;
    private int currentRouteIndex = 0;
    [SerializeField] private float moveSpeed = 2f;
    public bool isMove = false;
    
    
    private void OnEnable()
    {
        TileSpawner tileSpawner = FindObjectOfType<TileSpawner>();
        
        transform.position = tileSpawner.transform.position;
        
        currentRouteIndex = 0;
        _tileManager = TileManager.Instance;
    
        _routeManager = _tileManager.path;
        
        int childCount = _routeManager.Count;
        routePoints = new Transform[childCount];
    
        for (int i = 0; i < childCount; i++)
        {
            routePoints[i] = _routeManager[i].transform;
        }
    }
    
    private void Update()
    {
        // if (routePoints.Length == 0 || currentRouteIndex >= routePoints.Length)
        //     return;
        if (!isMove) return;
        
        Transform target = routePoints[currentRouteIndex];
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    
        // 목표 지점에 거의 도달했을 경우 다음 지점으로
        if (currentRouteIndex < routePoints.Length - 1)
        {
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                currentRouteIndex++;
            }    
        }
        
    
        // Vector3 offset = new Vector3(5f, 0, 0);
        // pointPrefab.transform.position = transform.position;//  + offset;
        
    }
}
