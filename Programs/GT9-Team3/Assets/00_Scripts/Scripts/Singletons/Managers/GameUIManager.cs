using System;
using UnityEngine;


public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] public CanvasFixed canvasFixed;
    [SerializeField] public CanvasWindow canvasWindow;
    [SerializeField] public CanvasPopup canvasPopup;
    [SerializeField] public TowerBuildUI canvasTower;

    private void Awake()
    {
        Instance = this;
    }

    
}
