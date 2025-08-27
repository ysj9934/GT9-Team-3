using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvas : MonoBehaviour
{

    public static SettingCanvas Instance { get; private set; }

    public HUD_CustomSetting customSetting;
    public HUD_CastleHP castleHUD;
    public HUD_GameDefeat gameDefeatHUD;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        customSetting = GetComponentInChildren<HUD_CustomSetting>();
        castleHUD = GetComponentInChildren<HUD_CastleHP>();
        gameDefeatHUD = GetComponentInChildren<HUD_GameDefeat>();

        if (gameDefeatHUD != null)
        { 
            gameDefeatHUD.gameObject.SetActive(false);
        }
    }


}
