using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Enemy _enemy;
    public Animator animator;

    public float attackInterval = 1.5f;  // 1�� ����
    private float timer = 0f;

    private bool isInAttack = false;  // ���� ���� ������ ����

    // ���� ����
    private int state = 0;
    private bool action = false;

    private bool isDying = false;   // �״� �� �÷���
    private bool hasDied = false;   // ���� �Ϸ� �÷���

    void Start()
    {
        animator = GetComponent<Animator>();
        // Animator�� �ڵ����� �⺻ ���º��� �����
        _enemy = GetComponentInParent<Enemy>();
        _enemy.SetAnimationController(this);
    }

    


    void Update()
    {
        if (TileManager.Instance == null || TileManager.Instance.endTile == null)
        {
            Debug.LogWarning("TileManager �Ǵ� endTileRoad�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        if (isDying)
        {
            // �״� �ִϸ��̼� ���� üũ
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1.0f)
            {
                hasDied = true;
            }
            return; // �״� ���̸� �ٸ� �ִϸ��̼� ������Ʈ ���� ����
        }

        Vector3 centerPos = TileManager.Instance.endTile.transform.position;
        float distance = Vector3.Distance(transform.position, centerPos);

        if (distance < 0.1f)
        {
            timer += Time.deltaTime;

            if (!isInAttack && timer >= attackInterval)
            {
                // ���� �ִϸ��̼� ����
                animator.SetTrigger("Attack");
                isInAttack = true;
                timer = 0f;

                // ���� ���� ����
                action = true;
            }
            else if (isInAttack && timer >= attackInterval)
            {
                // 1�� �� �ٽ� Idle ���·�
                isInAttack = false;
                timer = 0f;

                state = 0;
                action = false;
            }
        }
        else
        {
            // �߾� Ÿ�� ����� �ʱ�ȭ
            timer = 0f;
            isInAttack = false;
            state = 2;
            action = false;
        }

        // �ִϸ����� �Ķ���� ����
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    // ���� �ִϸ��̼� ���� ��û �޼���
    public void PlayDieAnimation()
    {
        isDying = true;
        state = 9;
        action = false;
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
        //animator.SetTrigger("Death"); // Animator���� Death ���·� ��ȯ
    }

    public bool HasDied()
    {
        return hasDied;
    }
}