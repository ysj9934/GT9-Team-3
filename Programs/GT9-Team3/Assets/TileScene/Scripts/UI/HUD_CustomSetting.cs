using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_CustomSetting : MonoBehaviour
{
    [SerializeField] public Button waveSystembutton;

    private void Awake()
    {
        waveSystembutton.interactable = false; // 초기 상태로 비활성화
    }

}
