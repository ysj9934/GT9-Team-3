using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Enemy _enemy;

    private Transform[] pathPoints;
    private int currentPathIndex = 0;

    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void pathPoint(List<Transform> path)
    {
        gameObject.transform.position = path[0].transform.position + new Vector3(0f, -0.16f, 0f);
        currentPathIndex = 0;

        int childCount = path.Count;
        pathPoints = new Transform[childCount];

        for (int index = 0; index < childCount; index++)
        {
            pathPoints[index] = path[index].transform;
        }
    }

    private void Update()
    {

        if (!_enemy.isAlive) return;

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }
            return; // 스턴 중엔 이동 스킵
        }

        Transform target = pathPoints[currentPathIndex];

        // MoveTowards를 사용해 목표점까지 정확히 이동
        Vector2 pos = target.position + new Vector3(0f, -0.16f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, pos, _enemy._enemyStat.enemyMovementSpeed * Time.deltaTime);

        // 목표점에 도달했으면 다음 지점으로 이동
        if (Vector3.Distance(transform.position, pos) < 0.01f)
        {
            currentPathIndex++;
        }

        //if (currentPathIndex >= pathPoints.Length)
        //{
        //    currentPathIndex = 0;
        //}

    }

    // 원진 : 상태이상 적용 함수
    public void ApplySlow(float slowAmount, float duration)
    {
        StartCoroutine(SlowCoroutine(slowAmount, duration));
    }

    private IEnumerator SlowCoroutine(float slowAmount, float duration)
    {
        float originalSpeed = _enemy._enemyStat.enemyMovementSpeed;
        _enemy._enemyStat.enemyMovementSpeed *= (1f - slowAmount);

        yield return new WaitForSeconds(duration);

        _enemy._enemyStat.enemyMovementSpeed = originalSpeed;
    }

    public void ApplyStun(float duration)
    {
        if (duration <= 0f) return;

        isStunned = true;
        stunTimer = duration;
    }

}
