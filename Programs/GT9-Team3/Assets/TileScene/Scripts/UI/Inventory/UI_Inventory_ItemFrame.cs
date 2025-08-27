using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory_ItemFrame : MonoBehaviour
{
    public UI_Inventory_ItemImage itemImage;
    
    private void Awake()
    {
        itemImage = GetComponentInChildren<UI_Inventory_ItemImage>();
    }
}
