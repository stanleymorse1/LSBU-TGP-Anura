using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    private GameObject startCam;
    public GameObject fightCam;
    int enemyAtk;
    [HideInInspector]
    public Enemy enemy;
    public PlayerController player;
    public Sprite[] cards;

    public Image pCard;
    public Image eCard;

    public bool debounce;
    public bool applyFx;

    [SerializeField]
    private Animator anim;
    public void startFight()
    {
        startCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        Invoke("camSwap", 1f);
        anim.SetTrigger("EnterBattle");
        //Play fight intro animation
        player.inFight = true;
    }
    void camSwap()
    {
        startCam.SetActive(!startCam.activeSelf);
        fightCam.SetActive(!fightCam.activeSelf);
    }
    public void attack(int atk)
    {
        debounce = true;
        anim.SetTrigger("Start");
        //Add a weights system for picking attacks - have it customizable per enemy.
        enemyAtk = Random.Range(0, 4);
        pCard.sprite = cards[atk];
        eCard.sprite = cards[enemyAtk];
        if (enemyAtk == 3)
            enemy.hurt(-10);
        //ok this is such a fucking mess but hear me out: 0 is attack, 1 is magic, 2 is block.
        //RPS game controls - 0 beats 1, 1 beats 2, 2 beats 0.
        //Block only deals damage when it is attacked.
        //Heal will always lose unless opponent blocks or also heals.
        //Draws cause both parties to take half their attacker's damage.

        if ((atk != 2 && atk == enemyAtk -1) || atk == 2 && enemyAtk == 0 || (atk <= 1 && enemyAtk == 3))
        {
            Debug.Log(atk + " beats " + enemyAtk);
            anim.SetTrigger("PlayerWin");
            StartCoroutine(applyDamage("win"));
            Debug.Log("Win");
        }
        else if (atk == enemyAtk && atk <= 1)
        {
            anim.SetTrigger("Draw");
            StartCoroutine(applyDamage("draw"));
            Debug.Log("Draw");
        }
        else if ((enemyAtk!= 2 && enemyAtk == atk - 1) || enemyAtk == 2 && atk == 0 || (enemyAtk <= 1 && atk == 3))
        {
            anim.SetTrigger("EnemyWin");
            StartCoroutine(applyDamage("lose"));
            Debug.Log("Lose");
        }
        else
        {
            anim.SetTrigger("Stale");
            Debug.Log("Stalemate!");
            Invoke("coolDown", 1);
        }
    }
    public void endFight()
    {
        player.inFight = false;
        Invoke("camSwap", 1f);
        //enemy.gameObject.GetComponent<CombatTrigger>().exitBattle();
        Destroy(enemy.transform.parent.gameObject, 1.5f);
        anim.SetTrigger("EnterBattle");
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
        debounce = false;
    }
    void coolDown()
    {
        debounce = false;
    }
}