using System;
using UnityEngine;


public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] public GameObject canvasFixed;
    [SerializeField] public GameObject canvasWindow;
    [SerializeField] public GameObject canvasPopup;

    private void Awake()
    {
        Instance = this;
    }
}
