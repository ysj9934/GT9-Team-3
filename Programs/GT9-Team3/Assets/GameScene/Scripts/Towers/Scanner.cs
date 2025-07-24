using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 가까운 적을 인식합니다.
public class Scanner : MonoBehaviour
{
    // (Test) 현재 scanRange와 UI_Range는 연동 안되어 있음
    [SerializeField] private float scanRange;           // scan range
    [SerializeField] private LayerMask targetLayer;     // target Enemy
    [SerializeField] private RaycastHit2D[] targets;    // target all Enemy
    [SerializeField] public Transform nearestTarget;    // target nearest
    
    // (생각) 최초 적 포착 이후 적의 사망 or 스캐너에서 벗어난 이후 가장 가까운 적 인식
    // (현재) 가장 가까운 적을 인식
    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    // 가장 가까운 적 포착
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100f;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDistance = Vector3.Distance(myPos, targetPos);

            if (curDistance < diff)
            {
                diff = curDistance;
                result = target.transform;
            }
        }

        return result;
    }
}
