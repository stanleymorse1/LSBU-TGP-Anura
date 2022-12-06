using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    int enemyAtk;
    [HideInInspector]
    public Enemy enemy;
    public PlayerController player;
    public Sprite[] cards;

    public Image pCard;
    public Image eCard;

    public bool applyFx;

    [SerializeField]
    private Animator anim;
    public void startFight()
    {
        //Play fight intro animation
        player.inFight = true;
    }
    public void attack(int atk)
    {
        anim.SetTrigger("Start");
        //RPS game controls - 0 beats 1, 1 beats 2, 2 beats 0.
        enemyAtk = Random.Range(0, 2);
        pCard.sprite = cards[atk];
        eCard.sprite = cards[enemyAtk];
        if((atk == enemyAtk -1) || atk == 2 && enemyAtk == 0)
        {
            anim.SetTrigger("PlayerWin");
            StartCoroutine(applyDamage("win"));
            Debug.Log("Win");
        }
        else if (atk == enemyAtk)
        {
            anim.SetTrigger("Draw");
            StartCoroutine(applyDamage("draw"));
            Debug.Log("Draw");
            
        }
        else
        {
            anim.SetTrigger("EnemyWin");
            StartCoroutine(applyDamage("lose"));
            Debug.Log("Lose");
        }
    }
    public void endFight()
    {
        player.inFight = false;
        enemy.gameObject.GetComponent<CombatTrigger>().exitBattle();
        Destroy(enemy.transform.parent.gameObject, 0.5f);
    }
    IEnumerator applyDamage(string state)
    {
        yield return new WaitUntil(() => applyFx == true);
        switch (state)
        {
            case "win":
                enemy.SendMessage("hurt", player.damage);
                break;
            case "draw":
                player.SendMessage("hurtheal", -enemy.damage / 2);
                enemy.SendMessage("hurt", player.damage / 2);
                break;
            case "lose":
                player.SendMessage("hurtheal", -enemy.damage);
                break;
        }
        applyFx = false;
    }
}