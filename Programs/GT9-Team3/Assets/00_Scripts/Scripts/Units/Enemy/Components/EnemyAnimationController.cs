using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Enemy _enemy;
    public Animator animator;

    public float attackInterval = 1.5f;
    private float timer = 0f;

    private bool isInAttack = false;

    private int state = 0;
    private bool action = false;

    private bool isDying = false;
    private bool hasDied = false;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1.0f)
            {
                hasDied = true;
            }
            return;
        }

        Vector3 centerPos = TileManager.Instance.endTile.transform.position;
        float distance = Vector3.Distance(transform.position, centerPos);

        if (distance < 0.1f)
        {
            timer += Time.deltaTime;

            if (!isInAttack && timer >= attackInterval)
            {
                animator.SetTrigger("Attack");
                isInAttack = true;
                timer = 0f;
                
                action = true;
            }
            else if (isInAttack && timer >= attackInterval)
            {
                isInAttack = false;
                timer = 0f;

                state = 0;
                action = false;
            }
        }
        else
        {
            timer = 0f;
            isInAttack = false;
            state = 2;
            action = false;
        }

        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    public void PlayDieAnimation()
    {
        isDying = true;
        state = 9;
        action = false;
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    public bool HasDied()
    {
        return hasDied;
    }
}