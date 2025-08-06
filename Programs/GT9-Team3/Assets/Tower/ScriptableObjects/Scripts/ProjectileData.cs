using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileData", menuName = "Tower Defense/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public string projectileID;
    public float speed;
    public int damage;
    public float lifetime = 5f;

    public bool isAOE;                      // ���� ���� ����
    public float aoeRadius;

    public GameObject impactEffectPrefab;   // Ÿ�� �� ����Ʈ
}
