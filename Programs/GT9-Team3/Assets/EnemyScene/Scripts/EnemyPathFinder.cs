using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
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
        //?ë³¸
        //    if (!isMove) return;

        //    sec += Time.deltaTime;
        //    if (sec > timer)
        //    {
        //        Transform target = pathPoints[currentPathIndex];
        //        Vector3 dir = (target.position - transform.position).normalized;
        //        transform.position += dir * moveSpeed * Time.deltaTime;

        //        // ëª©í‘œ ì§€?ì— ê±°ì˜ ?„ë‹¬?ˆì„ ê²½ìš° ?¤ìŒ ì§€?ìœ¼ë¡?
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

        // MoveTowardsë¥??¬ìš©??ëª©í‘œ?ê¹Œì§€ ?•í™•???´ë™
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // ëª©í‘œ?ì— ?„ë‹¬?ˆìœ¼ë©??¤ìŒ ì§€?ìœ¼ë¡??´ë™
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentPathIndex++;
        }
    }
}
