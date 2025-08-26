using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_GameDefeat : MonoBehaviour
{
    

    public void ShowDefeatPanel()
    {
        gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        TileCastle tileCastle = FindObjectOfType<TileCastle>();
        Castle castle = tileCastle.GetComponentInChildren<Castle>();
        castle.ResetButton();

        gameObject.SetActive(false);
    }
}
