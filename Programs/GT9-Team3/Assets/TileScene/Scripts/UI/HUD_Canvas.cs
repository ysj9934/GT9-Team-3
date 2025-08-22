using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Canvas : MonoBehaviour
{
    public static HUD_Canvas Instance { get; private set; }

    public HUD_CastleHP castleHUD;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        castleHUD = GetComponentInChildren<HUD_CastleHP>();
    }

}
