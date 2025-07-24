using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private Transform wayPointManager;
    private int currentWayPointIndex = 0;
    private Transform[] wayPoints;


    [Header("Info")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public int attack = 1;
    // [SerializeField] public float maxHealth = 10;
    [SerializeField] public float health = 10;

    // WayPoint
    // 적의 움직임

    private void OnEnable()
    {
        GetWayPoint();
    }

    private void Update()
    {
        if (wayPoints.Length == 0 || currentWayPointIndex >= wayPoints.Length)
            return;

        Move();
    }

    private void GetWayPoint()
    {
        int wayPointCount = wayPointManager.childCount;
        wayPoints = new Transform[wayPointCount];

        for (int i = 0; i < wayPointCount; i++)
        {
            wayPoints[i] = wayPointManager.GetChild(i);
        }
    }

    private void Move()
    {
        Transform target = wayPoints[currentWayPointIndex];
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // 목표 지점에 거의 도달했을 경우 다음 지점으로
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWayPointIndex++;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            OnDestroy();
    }

    private void OnDestroy()
    {
        Debug.Log("Dead");

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MainBase mainBase = other.gameObject.GetComponent<MainBase>();
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if (mainBase == null && bullet == null) return;

        if (mainBase != null)
        {
            mainBase.TakeDamage(attack);
            OnDestroy();
        }
        else
        {
            TakeDamage(bullet.damage);
        }


    }

// private void OnCollisionEnter2D(Collision2D other)
    // {
    //     MainBase mainBase = other.gameObject.GetComponent<MainBase>();
    //     if (mainBase == null) return;
    //
    //     Debug.Log("OnCollisionEnter");
    //     
    //     mainBase.TakeDamage(Attack);
    //     
    //     Destroy(gameObject);
    // }
}
