using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProjectileDataTable
{
    public int key;
    public string Inner_Name;
    public int Tower_Grade;
    public string Use_Projectile;
    public string Attack_type;

    public string Target_Order1;
    public string Target_Order2;
    public string Target_Order3;
    public string Target_Order4;

    public string Upgrade_Cost;
    public int Upgrade_Value;
    public string Make_Cost;
    public int Make_Value;
    public string Sell_Cost;
    public int Sell_Value;

    public int User_Level;
}

[Serializable]
public class ProjectileDataWrapper
{
    public List<ProjectileDataRow> Items;
}