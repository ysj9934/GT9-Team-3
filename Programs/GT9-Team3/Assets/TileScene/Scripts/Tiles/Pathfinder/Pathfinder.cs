using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private List<TileRoad> pathManager;
    private Transform[] pathPoints;
    private int currentPathIndex = 0;
    [SerializeField] private float moveSpeed = 10f;
    private float timer = 3f;
    private float sec = 0;

    private bool isMove;
    
    public void Initialize(List<TileRoad> path)
    {
        currentPathIndex = 0;

        pathManager = path;
        int childCount = pathManager.Count;
        pathPoints = new Transform[childCount];

        for (int index = 0; index < childCount; index++)
        {
            pathPoints[index] = pathManager[index].transform;
        }

        isMove = true;
    }

    private void Update()
    {
        if (!isMove) return;
        
        sec += Time.deltaTime;
        if (sec > timer)
        {
            Transform target = pathPoints[currentPathIndex];
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
    
            // 목표 지점에 거의 도달했을 경우 다음 지점으로
            if (currentPathIndex < pathPoints.Length - 1)
            {
                if (Vector3.Distance(transform.position, target.position) < 0.01f)
                {
                    currentPathIndex++;
                }    
            }
        }
    }

}
