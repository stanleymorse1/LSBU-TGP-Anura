using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startHP;
    int currentHP;
    public int damage;
    public void hurt(int amount)
    {
        currentHP -= amount;
    }
}
