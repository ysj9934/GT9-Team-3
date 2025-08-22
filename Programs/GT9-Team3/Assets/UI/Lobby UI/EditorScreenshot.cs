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
        var camera = sceneView.camera;
        var rt = new RenderTexture((int)sceneView.position.width, (int)sceneView.position.height, 24);
        camera.targetTexture = rt;
        camera.Render();

        // Texture2D로 변환
        RenderTexture.active = rt;
        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        // PNG 저장
        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/../SceneViewScreenshot.png";
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log("SceneView 스크린샷 저장됨: " + path);

        // 리소스 해제
        camera.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);
    }
}