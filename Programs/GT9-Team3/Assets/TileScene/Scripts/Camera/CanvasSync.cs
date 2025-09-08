using UnityEngine;
using UnityEngine.UI;

public class CanvasSync : MonoBehaviour
{
    private void Start()
    {
        ApplyCanvasAspect();
        //RemoveDuplicateCanvases();
    }

    private void ApplyCanvasAspect()
    {
        CanvasScaler scaler = GetComponentInChildren<CanvasScaler>();
        if (scaler == null) return;

        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = (windowAspect > targetAspect) ? 1 : 0;


    }

    //private void RemoveDuplicateCanvases()
    //{
    //    Canvas[] allCanvases = FindObjectsOfType<Canvas>();

    //    foreach (Canvas canvas in allCanvases)
    //    {
    //        if (canvas != GetComponentInChildren<Canvas>() && canvas.CompareTag("MainCanvas"))
    //        {
    //            Debug.LogWarning($"중복된 MainCanvas 제거: {canvas.name}");
    //            Destroy(canvas.gameObject);
    //        }
    //    }
    //}

}
