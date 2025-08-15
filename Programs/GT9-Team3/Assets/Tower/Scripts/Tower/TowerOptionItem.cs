using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerOptionItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text costText;
    public Button buildButton;

    private TowerBlueprint bp;
    private TowerBuildUI owner;

    public void Setup(TowerBlueprint blueprint, TowerBuildUI ui, bool canAfford)
    {
        bp = blueprint;
        owner = ui;

        icon.sprite = bp.icon;
        nameText.text = bp.displayName;
        costText.text = $"{bp.CostType} : {bp.CostValue}";
        buildButton.interactable = canAfford;

        buildButton.onClick.RemoveAllListeners();
        buildButton.onClick.AddListener(() => owner.OnClickBuild(bp));
    }
}
