using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveStart : MonoBehaviour
{
    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        SleepOnButton();
    }

    public void WakeOnButton()
    { 
        image.color = new Color(0, 0, 0, 1);
        button.interactable = true;
    }

    public void SleepOnButton()
    {
        image.color = new Color(1, 1, 1, 0.5f);
        button.interactable = false;
    }
}
