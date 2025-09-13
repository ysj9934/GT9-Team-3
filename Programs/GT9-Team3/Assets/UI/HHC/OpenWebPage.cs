using UnityEngine;

public class OpenWebPage : MonoBehaviour
{
    // Inspector에서 버튼별로 URL 설정 가능
    public string url;

    public void OpenPage()
    {
        Debug.Log("버튼 클릭됨");
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
        else
            Debug.LogWarning("URL이 설정되지 않았습니다!");
    }
}