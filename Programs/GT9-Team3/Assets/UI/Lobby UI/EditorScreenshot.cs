#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class EditorScreenshot
{
    [MenuItem("Tools/Take SceneView Screenshot")]
    static void TakeSceneViewScreenshot()
    {
        var sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null)
        {
            Debug.LogError("SceneView가 없습니다!");
            return;
        }

        // RenderTexture 준비
        int width = (int)sceneView.position.width;
        int height = (int)sceneView.position.height;
        var rt = new RenderTexture(width, height, 24);

        // SceneView를 RenderTexture로 렌더링
        sceneView.camera.targetTexture = rt;
        sceneView.Repaint();                 // 씬뷰 강제 렌더
        sceneView.camera.Render();

        // Texture2D로 변환
        RenderTexture.active = rt;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // PNG 저장
        string path = Application.dataPath + "/../SceneViewScreenshot.png";
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        Debug.Log("SceneView 스크린샷 저장됨: " + path);

        // 리소스 해제
        sceneView.camera.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);
    }
}
#endif