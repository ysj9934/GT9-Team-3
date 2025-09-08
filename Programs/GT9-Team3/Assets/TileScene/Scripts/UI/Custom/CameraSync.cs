using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSync : MonoBehaviour
{
    private static CameraSync instance;

    void Awake()
    {
        // 중복 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyAspect();
        RemoveDuplicateCameras();
    }

    void ApplyAspect()
    {
        //float targetAspect = 16f / 9f;
        //float windowAspect = (float)Screen.width / (float)Screen.height;
        //float scaleHeight = windowAspect / targetAspect;

        //Camera camera = Camera.main;
        //if (camera == null) return;

        //if (scaleHeight < 1.0f)
        //{
        //    camera.rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1, scaleHeight);
        //}
        //else
        //{
        //    float scaleWidth = 1.0f / scaleHeight;
        //    camera.rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1);
        //}
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = Camera.main;
        if (cam == null) return;

        if (scaleHeight < 1.0f)
        {
            float inset = (1.0f - scaleHeight) / 2.0f;
            cam.rect = new Rect(0, inset, 1, scaleHeight);
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            float inset = (1.0f - scaleWidth) / 2.0f;
            cam.rect = new Rect(inset, 0, scaleWidth, 1);
        }

    }

    void RemoveDuplicateCameras()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();

        foreach (Camera cam in allCameras)
        {
            if (cam != Camera.main && cam.CompareTag("MainCamera"))
            {
                Debug.LogWarning($"중복된 MainCamera 제거: {cam.name}");
                Destroy(cam.gameObject);
            }
        }
    }
}
