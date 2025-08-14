using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    public void Initialize()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    { 
        
    }

}
