using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{

    // (Test) 기지 체력 최대 회복
    public void OnClickGameReBase()
    {
        
    }

    // (Test) 게임 나가기
    public void OnClickGameQuit()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
