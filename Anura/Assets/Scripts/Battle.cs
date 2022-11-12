using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    int enemyAtk;
    [HideInInspector]
    public Enemy enemy;
    public PlayerController player;
    public void startFight()
    {
        //Play fight intro animation
        player.inFight = true;
    }
    public void attack(int atk)
    {
        //RPS game controls - 0 beats 1, 1 beats 2, 2 beats 0.
        enemyAtk = Random.Range(0, 2);
        if((atk == enemyAtk -1) || atk == 2 && enemyAtk == 0)
        {
            Debug.Log("Win");
            enemy.SendMessage("hurt", player.damage);
        }
        else if (atk == enemyAtk)
        {
            Debug.Log("Draw");
            player.SendMessage("hurt", enemy.damage / 2);
            enemy.SendMessage("hurt", player.damage / 2);
        }
        else
        {
            Debug.Log("Lose");
            player.SendMessage("hurt", enemy.damage);
        }
    }
    public void endFight()
    {
        player.inFight = false;
        enemy.gameObject.GetComponent<CombatTrigger>().exitBattle();
        Destroy(enemy.transform.parent.gameObject, 0.5f);
    }
}