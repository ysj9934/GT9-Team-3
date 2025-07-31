using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2f;  // �̵� �ӵ� public���� ����
    [SerializeField] int hp = 10;
    public Vector2 targetPosition;  // ��ǥ ��ġ�� public���� ��

    private Transform[] path; // ��θ� ���� �̵��� �� ��� (��: Waypoint �ý���)
    private int pathIndex = 0;  // ��θ� ���� �̵��� �� ���

    void Start()
    {
        Debug.Log("EnemyTest Initialized");
        Debug.Log(" �� target: " + targetPosition);
    }

    // ��θ� �����ϴ� �Լ� (��: ��θ� ���� �̵��ϴ� ���� ���� �� ���)
    //public void SetPath(Transform[] newPath)
    //{
    //    path = newPath;
    //}

    void Update()
    {
        // �̵� ó��
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    //�浹 ó��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ѿ˿� ���� ���
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Enemy hit by Bullet");
            TakeDamage(1); // �⺻ ������ 1
            Destroy(collision.gameObject); // �Ѿ� ����
        }

        // �÷��̾�� �浹�� ���
        else if (collision.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with Player");
            // ��: �÷��̾ ������ �ְų� ���� ��
            Die(); // ������ ���̶��
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log(hp);
        TakeDamage(1); // Ŭ�� �� ������ 1
    }
}