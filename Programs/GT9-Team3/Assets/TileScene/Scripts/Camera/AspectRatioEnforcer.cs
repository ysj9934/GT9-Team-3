using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    private void Start()
    {
        EnforceAspectRatio();
    }

    void EnforceAspectRatio()
    {
        float targetAspect = 16f / 9f;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = screenAspect / targetAspect;

        Camera cam = Camera.main;
        if (cam == null) return;

        if (scaleHeight < 1.0f)
        {
            // 화면이 더 높을 때 → 위아래 레터박스
            float inset = (1.0f - scaleHeight) / 2.0f;
            cam.rect = new Rect(0, inset, 1, scaleHeight);
        }
        else
        {
            // 화면이 더 넓을 때 → 좌우 레터박스
            float scaleWidth = 1.0f / scaleHeight;
            float inset = (1.0f - scaleWidth) / 2.0f;
            cam.rect = new Rect(inset, 0, scaleWidth, 1);
        }
    }

}
