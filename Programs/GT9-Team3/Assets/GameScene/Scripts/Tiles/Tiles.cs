using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    // (Test) 타일 오브젝트 클릭시 90도 회전
    // 현재: 단방향 회전
    private void OnMouseDown()
    {
        transform.Rotate(0, 0, 90);
    }
}
