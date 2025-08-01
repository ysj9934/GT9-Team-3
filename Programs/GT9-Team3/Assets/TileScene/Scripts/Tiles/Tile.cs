using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float cellX = 1f;
    [SerializeField] private float cellY = 1f;
    
    void OnMouseDrag()
    {
        Plane plane = new Plane(Vector3.forward, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.position = SnapToGrid(point); // 격자 스냅 함수
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / cellX) * cellX;
        float y = Mathf.Round(pos.y / cellY) * cellY;
        return new Vector3(x, y, pos.z);
    }

}
