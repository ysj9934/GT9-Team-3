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
        // �ڵ����� LineRenderer �������ų� ������ �߰�
        lr = GetComponent<LineRenderer>();
        if (lr == null) lr = gameObject.AddComponent<LineRenderer>();

        // �⺻ ����(�ʿ� �� ����)
        lr.useWorldSpace = false; // Ÿ�� �ڽ� ����
        lr.loop = true;
        lr.positionCount = segments;
        lr.widthMultiplier = 0.03f;  // �� ����
        // lr.material = new Material(Shader.Find("Sprites/Default")); // �ʿ� ��
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
