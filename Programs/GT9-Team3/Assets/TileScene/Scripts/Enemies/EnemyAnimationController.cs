using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Enemy _enemy;
    public Animator animator;

    public float attackInterval = 1.5f;  // 1초마다 공격
    private float timer = 0f;

    private bool isInAttack = false;  // 현재 공격 중인지 여부

    // 상태 관리
    private int state = 0;
    private bool action = false;

    private bool isDying = false;   // 죽는 중인지 여부
    private bool hasDied = false;   // 죽음이 완료되었는지 여부

    void Start()
    {
        animator = GetComponent<Animator>();
        // Animator와 Enemy 연결
        _enemy = GetComponentInParent<Enemy>();
        _enemy.SetAnimationController(this);
    }

    void Update()
    {
        if (TileManager.Instance == null || TileManager.Instance.endTile == null)
        {
            Debug.LogWarning("TileManager 또는 endTile이 설정되지 않았습니다.");
            return;
        }

        if (isDying)
        {
            // 죽는 애니메이션이 끝났는지 확인
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1.0f)
            {
                hasDied = true;
            }
            return; // 죽는 중이면 나머지 로직 실행하지 않음
        }

        Vector3 centerPos = TileManager.Instance.endTile.transform.position;
        float distance = Vector3.Distance(transform.position, centerPos);

        if (distance < 0.1f)
        {
            timer += Time.deltaTime;

            if (!isInAttack && timer >= attackInterval)
            {
                // 공격 시작
                animator.SetTrigger("Attack");
                isInAttack = true;
                timer = 0f;

                // 액션 상태 설정
                action = true;
            }
            else if (isInAttack && timer >= attackInterval)
            {
                // 공격 후 Idle 상태로 전환
                isInAttack = false;
                timer = 0f;

                state = 0;
                action = false;
            }
        }
        else
        {
            // 이동 중이면 공격 관련 상태 초기화
            timer = 0f;
            isInAttack = false;
            state = 2;   // 이동 상태
            action = false;
        }

        // 애니메이터에 상태 전달
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    // 죽는 애니메이션 실행
    public void PlayDieAnimation()
    {
        isDying = true;
        state = 9;
        action = false;
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
        //animator.SetTrigger("Death"); // Animator에서 Death 트리거로 전환 가능
    }

    // 죽음 완료 여부 반환
    public bool HasDied()
    {
        return hasDied;
    }
}