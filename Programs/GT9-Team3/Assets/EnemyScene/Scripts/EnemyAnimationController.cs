using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;

    public float attackInterval = 1.5f;  // 1초 간격
    private float timer = 0f;

    private bool isInAttack = false;  // 현재 공격 중인지 여부

    // 상태 변수
    private int state = 0;
    private bool action = false;

    private bool isDying = false;   // 죽는 중 플래그
    private bool hasDied = false;   // 죽음 완료 플래그

    void Start()
    {
        animator = GetComponent<Animator>();
        // Animator가 자동으로 기본 상태부터 재생됨
    }

    void Update()
    {
        if (TileManager.Instance == null || TileManager.Instance.endTileRoad == null)
        {
            Debug.LogWarning("TileManager 또는 endTileRoad가 할당되지 않았습니다.");
            return;
        }

        if (isDying)
        {
            // 죽는 애니메이션 종료 체크
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1.0f)
            {
                hasDied = true;
            }
            return; // 죽는 중이면 다른 애니메이션 업데이트 하지 않음
        }

        Vector3 centerPos = TileManager.Instance.endTileRoad.transform.position;
        float distance = Vector3.Distance(transform.position, centerPos);

        if (distance < 0.1f)
        {
            timer += Time.deltaTime;

            if (!isInAttack && timer >= attackInterval)
            {
                // 공격 애니메이션 시작
                animator.SetTrigger("Attack");
                isInAttack = true;
                timer = 0f;

                // 공격 상태 세팅
                action = true;
            }
            else if (isInAttack && timer >= attackInterval)
            {
                // 1초 후 다시 Idle 상태로
                isInAttack = false;
                timer = 0f;

                state = 0;
                action = false;
            }
        }
        else
        {
            // 중앙 타일 벗어나면 초기화
            timer = 0f;
            isInAttack = false;
            state = 2;
            action = false;
        }

        // 애니메이터 파라미터 적용
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    // 죽음 애니메이션 시작 요청 메서드
    public void PlayDieAnimation()
    {
        isDying = true;
        state = 9;
        action = false;
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
        //animator.SetTrigger("Death"); // Animator에서 Death 상태로 전환
    }

    public bool HasDied()
    {
        return hasDied;
    }
}