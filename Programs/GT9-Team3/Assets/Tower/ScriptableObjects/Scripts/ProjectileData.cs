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

    public bool isAOE;                      // 범위 공격 여부
    public float aoeRadius;

    public GameObject impactEffectPrefab;   // 타격 시 이펙트

    //public int armorDestroy;

    public float slowEffect;
    public float slowTime;
    public float stunTime;
}
