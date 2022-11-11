using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    int enemyAtk;
    public Enemy enemy;
    public void startFight(Enemy attacker)
    {
        enemy = attacker;
    }
    public void attack(int atk)
    {
        enemyAtk = Random.Range(0, 2);
        if((atk == enemyAtk -1) || atk == 2 && enemyAtk == 0)
        {
            Debug.Log("Win");
            enemy.SendMessage("hurt");
        }
    }
}
