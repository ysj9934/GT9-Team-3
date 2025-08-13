using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeIndicator : MonoBehaviour
{
    public int segments = 60;

    private LineRenderer lr;

    void Awake()
    {
        // 자동으로 LineRenderer 가져오거나 없으면 추가
        lr = GetComponent<LineRenderer>();
        if (lr == null) lr = gameObject.AddComponent<LineRenderer>();

        // 기본 설정(필요 시 조정)
        lr.useWorldSpace = false; // 타워 자식 기준
        lr.loop = true;
        lr.positionCount = segments;
        lr.widthMultiplier = 0.03f;  // 선 굵기
        // lr.material = new Material(Shader.Find("Sprites/Default")); // 필요 시
    }

    public void SetRadius(float r)
    {
        if (lr == null) return;

        lr.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)segments * Mathf.PI * 2f;
            lr.SetPosition(i, new Vector3(Mathf.Cos(t) * r, Mathf.Sin(t) * r, 0));
        }
    }

    public void SetVisible(bool on)
    {
        if (lr != null) lr.enabled = on;
        gameObject.SetActive(on);
    }
}
