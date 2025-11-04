using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
<<<<<<< Updated upstream:Programs/GT9-Team3/Assets/TileScene/Scripts/Enemies/EnemyMovement.cs
=======
    // EnemyMovement.cs 상단 필드
    private Vector2 _prevPos;
    private Vector2 _currentDir;// 마지막 이동 방향(정지 시 마지막 방향 유지)

    // Object Structure
>>>>>>> Stashed changes:Programs/GT9-Team3/Assets/00_Scripts/Scripts/Units/Enemy/Components/EnemyMovement.cs
    private Enemy _enemy;

    private Transform[] pathPoints;
    private int currentPathIndex = 0;

    // 스턴
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _prevPos = transform.position;
        _currentDir = Vector2.zero;
    }

    public void pathPoint(List<Transform> path)
    {
        gameObject.transform.position = path[0].transform.position + new Vector3(0f, 0.16f, 0f);
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
        if (currentPathIndex >= pathPoints.Length) return;


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
        Vector2 pos = target.position + new Vector3(0f, 0.16f, 0f);
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
    // ✅ 이동 방향 추적용 (넉백 계산에 사용됨)
    private void LateUpdate()
    {
        Vector2 now = transform.position;
        Vector2 delta = now - _prevPos;

        // 프레임 간 실제 이동량으로 방향 갱신
        if (delta.sqrMagnitude > 0.000001f)
            _currentDir = delta.normalized;

        _prevPos = now;
    }
    public Vector2 GetCurrentDirection()
    {
        // Rigidbody2D가 있다면 우선 속도로 판단 (정확)
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 최신 유니티 경고 회피: velocity 대신 linearVelocity 권장
#if UNITY_2022_2_OR_NEWER
            Vector2 v = rb.linearVelocity;
#else
        Vector2 v = rb.velocity;
#endif
            if (v.sqrMagnitude > 0.000001f)
                return v.normalized;
        }

        // 속도가 거의 0이면, 최근 프레임 이동 방향을 사용
        return _currentDir; // 정지면 0, 최근 방향 유지
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
    public void KnockbackAlongPath(float distance)
    {
        if (pathPoints == null || pathPoints.Length < 2) return;

        // 현재 세그먼트 인덱스: (i-1) -> i 를 따라 전진 중
        int i = Mathf.Clamp(currentPathIndex, 1, pathPoints.Length - 1);

        // ✅ Transform에서 position으로 좌표를 꺼내 Vector2로 사용
        Vector2 a = (Vector2)pathPoints[i - 1].position;
        Vector2 b = (Vector2)pathPoints[i].position;
        Vector2 ab = b - a;
        float segLen = ab.magnitude;
        if (segLen < 0.0001f) return;

        // 현재 위치를 구간(a->b) 위로 투영하여 진행 거리 sOnSeg 계산
        Vector2 p = (Vector2)transform.position;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / (segLen * segLen));  // 0~1
        float sOnSeg = t * segLen;

        float remain = distance;   // 뒤로 당길 총 거리
        int idx = i;

        // 남은 거리가 현재 세그먼트를 넘어가면 이전 세그먼트로 계속 이동
        while (remain > 0f && idx > 0)
        {
            float move = Mathf.Min(sOnSeg, remain);
            sOnSeg -= move;
            remain -= move;

            if (remain > 0f)
            {
                idx--;
                if (idx <= 0)
                {
                    idx = 1;
                    sOnSeg = 0f;
                    break;
                }

                a = (Vector2)pathPoints[idx - 1].position;   // ✅ position
                b = (Vector2)pathPoints[idx].position;       // ✅ position
                ab = b - a;
                segLen = ab.magnitude;
                sOnSeg = segLen; // 이전 세그먼트의 끝에서 다시 시작
            }
        }

        // 세그먼트 위의 새 좌표
        Vector2 newPos = a + ab.normalized * sOnSeg;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        // 인덱스/방향 보정
        currentPathIndex = Mathf.Clamp(idx, 1, pathPoints.Length - 1);
        _prevPos = transform.position;
        _currentDir = (ab.sqrMagnitude > 0.0001f) ? ab.normalized : _currentDir;

        // 남은 속도 제거(옆으로 미끄러짐 방지)
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
#if UNITY_2022_2_OR_NEWER
            rb.linearVelocity = Vector2.zero;
#else
        rb.velocity = Vector2.zero;
#endif
        }
    }
}
