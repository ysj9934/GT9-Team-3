using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Canvas : MonoBehaviour
{

    public static HUD_Canvas Instance { get; private set; }

    public HUD_CustomSetting customSetting;
    public HUD_CastleHP castleHUD;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        customSetting = GetComponentInChildren<HUD_CustomSetting>();
        castleHUD = GetComponentInChildren<HUD_CastleHP>();
    }

}
