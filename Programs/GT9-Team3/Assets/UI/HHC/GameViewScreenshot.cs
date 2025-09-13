#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class GameViewScreenshot
{
    [MenuItem("Tools/Take GameView Screenshot")]
    public static void CaptureGameView()
    {
        // 타임스탬프 생성
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = $"GameViewScreenshot_{timestamp}.png";
        string path = Path.Combine(Application.dataPath, "../" + filename);

        // 캡처
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("게임뷰 스크린샷 저장됨: " + path);
    }

    [MenuItem("Tools/Take High-Res GameView Screenshot")]
    public static void CaptureGameViewHighRes()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = $"GameViewHighResScreenshot_{timestamp}.png";
        string path = Path.Combine(Application.dataPath, "../" + filename);

        // 2배 해상도 캡처
        ScreenCapture.CaptureScreenshot(path, 2);
        Debug.Log("고해상도 게임뷰 스크린샷 저장됨: " + path);
    }
}
#endif