using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private List<TileRoad> pathManager;
    private Transform[] pathPoints;
    private int currentPathIndex = 0;
    [SerializeField] private float moveSpeed = 0.5f;
    private float timer = 0.3f;
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
        //원본
        //    if (!isMove) return;

        //    sec += Time.deltaTime;
        //    if (sec > timer)
        //    {
        //        Transform target = pathPoints[currentPathIndex];
        //        Vector3 dir = (target.position - transform.position).normalized;
        //        transform.position += dir * moveSpeed * Time.deltaTime;

        //        // 목표 지점에 거의 도달했을 경우 다음 지점으로
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

        // MoveTowards를 사용해 목표점까지 정확히 이동
        Vector2 pos = target.position + new Vector3(0f, 0.16f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);

        // 목표점에 도달했으면 다음 지점으로 이동
        if (Vector3.Distance(transform.position, pos) < 0.01f)
        {
            currentPathIndex++;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Basement basement = collision.GetComponent<Basement>();

        if (basement != null)
        {
            Debug.Log("You can start the game by connecting paths using the pathfinder");
            Destroy(gameObject);
        }
    }
}
