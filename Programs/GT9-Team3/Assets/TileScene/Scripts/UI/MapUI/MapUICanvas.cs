using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUICanvas : MonoBehaviour
{
    [SerializeField] private Button Startbutton;

    public void StageStartButton()
    {
        SceneLoader.Instance.LoadSceneByName("PresentationScene");
    }
}
