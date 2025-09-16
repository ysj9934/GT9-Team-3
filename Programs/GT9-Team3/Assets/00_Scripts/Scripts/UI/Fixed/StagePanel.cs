using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StagePanel : MonoBehaviour
{
    // Object Data
    [SerializeField] private TextMeshProUGUI worldPanelText;
    [SerializeField] private TextMeshProUGUI stagePanelText;
    [SerializeField] private TextMeshProUGUI roundPanelText;

    // StageInfoHUD
    public void ReceiveStageData(StageData stageData)
    {
        if (stageData != null)
        {
            worldPanelText.text = $"{stageData.worldCode}";
            stagePanelText.text = $"{stageData.stageCode}";
            roundPanelText.text = $"{stageData.roundCode}";
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }
}
