using UnityEngine;

public class OpenWebPage : MonoBehaviour
{
    // Inspector에서 웹 URL만 넣어주면 됩니다 (예: https://www.instagram.com/fal_lingkingdom/#)
    public string url;

    // 내부에서 앱 URL도 자동 생성
    private string appUrl;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(url))
        {
            // 웹 주소에서 username 추출 ("/" 기준으로 나눈 마지막 조각 사용)
            string[] parts = url.Split('/');
            string username = parts[parts.Length - 2]; // 마지막에 "#" 같은게 있을 수 있어서 -2 사용
            appUrl = "instagram://user?username=" + username;
        }
    }

    public void OpenPage()
    {
        Debug.Log("버튼 클릭됨");

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogWarning("URL이 설정되지 않았습니다!");
            return;
        }

        // 앱 실행 시도
        Application.OpenURL(appUrl);

        // 앱이 없으면 웹으로 fallback
        Invoke(nameof(OpenWebFallback), 1.0f);
    }

    private void OpenWebFallback()
    {
        Application.OpenURL(url);
    }
}