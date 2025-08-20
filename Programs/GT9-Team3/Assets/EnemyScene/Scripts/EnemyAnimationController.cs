using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;

    public float attackInterval = 1.5f;  // 1ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    private float timer = 0f;

    private bool isInAttack = false;  // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½

    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    private int state = 0;
    private bool action = false;

    private bool isDying = false;   // ï¿½×´ï¿½ ï¿½ï¿½ ï¿½Ã·ï¿½ï¿½ï¿½
    private bool hasDied = false;   // ï¿½ï¿½ï¿½ï¿½ ï¿½Ï·ï¿½ ï¿½Ã·ï¿½ï¿½ï¿½

    void Start()
    {
        animator = GetComponent<Animator>();
        // Animatorï¿½ï¿½ ï¿½Úµï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½âº» ï¿½ï¿½ï¿½Âºï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½
    }

    void Update()
    {
        if (TileManager.Instance == null || TileManager.Instance.endTileRoad == null)
        {
            Debug.LogWarning("TileManager ¶Ç´Â endTileRoad°¡ ÇÒ´çµÇÁö ¾Ê¾Ò½À´Ï´Ù.");
            return;
        }

        if (isDying)
        {
            // ï¿½×´ï¿½ ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½ï¿½ï¿½ï¿½ Ã¼Å©
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1.0f)
            {
                hasDied = true;
            }
            return; // ï¿½×´ï¿½ ï¿½ï¿½ï¿½Ì¸ï¿½ ï¿½Ù¸ï¿½ ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ® ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        }

        Vector3 centerPos = TileManager.Instance.endTile.transform.position;
        float distance = Vector3.Distance(transform.position, centerPos);

        if (distance < 0.1f)
        {
            timer += Time.deltaTime;

            if (!isInAttack && timer >= attackInterval)
            {
                // ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½ï¿½ï¿½ï¿½
                animator.SetTrigger("Attack");
                isInAttack = true;
                timer = 0f;

                // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                action = true;
            }
            else if (isInAttack && timer >= attackInterval)
            {
                // 1ï¿½ï¿½ ï¿½ï¿½ ï¿½Ù½ï¿½ Idle ï¿½ï¿½ï¿½Â·ï¿½
                isInAttack = false;
                timer = 0f;

                state = 0;
                action = false;
            }
        }
        else
        {
            // ï¿½ß¾ï¿½ Å¸ï¿½ï¿½ ï¿½ï¿½ï¿½î³ªï¿½ï¿½ ï¿½Ê±ï¿½È­
            timer = 0f;
            isInAttack = false;
            state = 2;
            action = false;
        }

        // ï¿½Ö´Ï¸ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ä¶ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
    }

    // ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã» ï¿½Þ¼ï¿½ï¿½ï¿½
    public void PlayDieAnimation()
    {
        isDying = true;
        state = 9;
        action = false;
        animator.SetInteger("State", state);
        animator.SetBool("Action", action);
        //animator.SetTrigger("Death"); // Animator¿¡¼­ Death »óÅÂ·Î ÀüÈ¯
    }

    public bool HasDied()
    {
        return hasDied;
    }
}