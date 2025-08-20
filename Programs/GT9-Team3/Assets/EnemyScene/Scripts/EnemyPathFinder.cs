using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    private List<TileInfo> pathManager;
    private Transform[] pathPoints;
    private int currentPathIndex = 0;
    [SerializeField] private float moveSpeed = 0.5f;
    private float timer = 0.3f;
    private float sec = 0;

    private bool isMove;

    public void Initialize(List<TileInfo> path)
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
        //?�본
        //    if (!isMove) return;

        //    sec += Time.deltaTime;
        //    if (sec > timer)
        //    {
        //        Transform target = pathPoints[currentPathIndex];
        //        Vector3 dir = (target.position - transform.position).normalized;
        //        transform.position += dir * moveSpeed * Time.deltaTime;

        //        // 목표 지?�에 거의 ?�달?�을 경우 ?�음 지?�으�?
        //        if (currentPathIndex < pathPoints.Length - 1)
        //        {
        //            if (Vector3.Distance(transform.position, target.position) < 0.01f)
        //            {

        //                currentPathIndex++;
        //            }
        //        }

        //        if (currentPathIndex >= pathPoints.Length)
        //        {
        //            isMove = false;
        //            Debug.Log("Pathfinder is finished");
        //            gameObject.SetActive(false);
        //        }
        //    }
        //}
        if (!isMove) return;

        if (currentPathIndex >= pathPoints.Length)
        {
            isMove = false;
            //gameObject.SetActive(false);
            return;
        }

        Transform target = pathPoints[currentPathIndex];

        // MoveTowards�??�용??목표?�까지 ?�확???�동
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // 목표?�에 ?�달?�으�??�음 지?�으�??�동
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentPathIndex++;
        }
    }
}
