using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDataMapper
{
    public static void ApplyToSO(ProjectileData so, ProjectileDataRow row)
    {
        so.projectileID = row.Key;
        so.projectileName = row.Inner_Name;
        so.damage = (int)row.Damage;
        so.speed = row.Projectile_Speed;
        so.impactRadius = row.Attack_Radius;
        so.slowEffect = row.Slow_Effect;
        so.slowTime = row.Slow_Time;
        so.stunTime = row.Stun_Time;

        // 타일 넉백, 버프 정보?
        // so.tileRange = row.Knockback_Tile_Count;
        // so.damageBoost = row.Damege_Increase;
        // so.attackSpeedBoost = row.Attack_Speed_Increase;
        // so.buffTowerCount = row.Buff_Tower_Count;

        Debug.Log($"[ProjectileDataMapper] 매핑 완료: 이름 = {row.Inner_Name}, 데미지 = {row.Damage}, 속도 = {row.Projectile_Speed}");
    }
}
