using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    private GameObject startCam;
    private int plrAtk;
    public GameObject fightCam;
    int enemyAtk;
    [HideInInspector]
    public Enemy enemy;
    public CopyBar enemyBar;
    public PlayerController player;
    public Sprite[] cards;

    public Image pCard;
    public Image eCard;

    public GameObject overHUD;
    public GameObject battleHUD;
    public GameObject enemySprite;

    public bool debounce;
    public bool applyFx;

    [SerializeField]
    private Animator cardAnim;
    public Animator spriteAnim;
    private Animator enemyAnim;

    public TextMeshProUGUI plrDmg;
    public TextMeshProUGUI enemyDmg;

    AudioSource music;
    public GameObject regMusic;
    public AudioClip BossMusic;


    public AudioClip plrMagic;
    public AudioClip enemyMagic;
    public AudioClip heal;

    public AudioClip success;
    public AudioClip fail;
    public AudioClip draw;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        cardAnim.Play("FadeIn", 1);
    }
    public void startFight()
    {
        Debug.Log("Fightbegan");
        if(regMusic)
            regMusic.GetComponent<AudioSource>().Pause();
        if (enemy.boss)
        {
            music.clip = BossMusic;
            music.Play();
        }
        else
            music.Play();
        enemyAnim = enemySprite.GetComponent<Animator>();
        enemyAnim.runtimeAnimatorController = enemy.gameObject.GetComponent<Animator>().runtimeAnimatorController;
        startCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        Invoke("camSwap", 1f);
        cardAnim.SetTrigger("EnterBattle");
        //Play fight intro animation
        player.inFight = true;
        enemyBar.bar = enemy.healthBar;
    }
    void camSwap()
    {
        overHUD.SetActive(!overHUD.activeSelf);
        battleHUD.SetActive(!battleHUD.activeSelf);

        startCam.SetActive(!startCam.activeSelf);
        fightCam.SetActive(!fightCam.activeSelf);
    }
    public void attack(int atk)
    {
        plrAtk = atk;

        debounce = true;
        cardAnim.SetTrigger("Start");
        //Add a weights system for picking attacks - have it customizable per enemy.
        float eWeight = Random.Range(0f, 1f);
        Debug.Log("eWeight: " + eWeight);
        foreach (float weight in enemy.atkWeights)
        {
            Debug.Log("Weight: "+ weight);
            if(eWeight < weight)
            {
                enemyAtk = enemy.atkWeights.IndexOf(weight);
            }
        }

        pCard.sprite = cards[atk];
        eCard.sprite = cards[enemyAtk];

        //ok this is such a fucking mess but hear me out: 0 is attack, 1 is magic, 2 is block.
        //RPS game controls - 0 beats 1, 1 beats 2, 2 beats 0.
        //Block only deals damage when it is attacked.
        //Heal will always lose unless opponent blocks or also heals.
        //Draws cause both parties to take half their attacker's damage.

        if ((atk != 2 && atk == enemyAtk -1) || atk == 2 && enemyAtk == 0 || (atk <= 1 && enemyAtk == 3))
        {
            Debug.Log(atk + " beats " + enemyAtk);
            cardAnim.SetTrigger("PlayerWin");
            StartCoroutine(applyDamage("win"));
            Debug.Log("Win");
            enemyDmg.text = player.damage.ToString();
            AudioSource.PlayClipAtPoint(success, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        }
        else if (atk == enemyAtk && atk <= 1)
        {
            cardAnim.SetTrigger("Draw");
            StartCoroutine(applyDamage("draw"));
            Debug.Log("Draw");
            enemyDmg.text = (player.damage/2).ToString();
            plrDmg.text = (enemy.damage/2).ToString();
            AudioSource.PlayClipAtPoint(draw, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        }
        else if ((enemyAtk!= 2 && enemyAtk == atk - 1) || enemyAtk == 2 && atk == 0 || (enemyAtk <= 1 && atk == 3))
        {
            cardAnim.SetTrigger("EnemyWin");
            StartCoroutine(applyDamage("lose"));
            Debug.Log("Lose");
            plrDmg.text = enemy.damage.ToString();
            AudioSource.PlayClipAtPoint(fail, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        }
        else
        {
            cardAnim.SetTrigger("Stale");
            Debug.Log("Stalemate!");
            if (plrAtk == 3 || enemyAtk == 3)
                Invoke("applyHeal", 1.4f);
            if (plrAtk == 2)
                spriteAnim.SetTrigger("Block");
            if (enemyAtk == 2)
                enemyAnim.SetTrigger("Block");
            Invoke("coolDown", 1);
        }
    }
    public void endFight()
    {
        regMusic.GetComponent<AudioSource>().Play();
        music.Stop();
        player.inFight = false;
        Invoke("camSwap", 1f);
        //enemy.gameObject.GetComponent<CombatTrigger>().exitBattle();
        Destroy(enemy.transform.parent.gameObject, 1.5f);
        //enemy.transform.parent.gameObject.SetActive(false);
        cardAnim.SetTrigger("EnterBattle");
    }
    IEnumerator applyDamage(string state)
    {
        if (plrAtk == 0)
            spriteAnim.SetTrigger("Melee");
        else if (plrAtk == 1)
        {
            spriteAnim.SetTrigger("Magic");
            AudioSource.PlayClipAtPoint(plrMagic, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        }
        else if (plrAtk == 2)
            spriteAnim.SetTrigger("Block");

        if (enemyAtk == 0)
            enemyAnim.SetTrigger("Melee");
        else if (enemyAtk == 1)
        {
            enemyAnim.SetTrigger("Magic");
            AudioSource.PlayClipAtPoint(enemyMagic, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        }
            
        else if (enemyAtk == 2)
            enemyAnim.SetTrigger("Block");

        yield return new WaitUntil(() => applyFx == true);


        switch (state)
        {
            case "win":
                enemy.SendMessage("hurt", player.damage);
                break;
            case "draw":
                player.SendMessage("hurtheal", enemy.damage / 2);
                enemy.SendMessage("hurt", player.damage / 2);
                break;
            case "lose":
                player.SendMessage("hurtheal", enemy.damage);
                if(player.currentHP <= 0)
                {
                    cardAnim.SetTrigger("Defeat");
                    Invoke("restartLevel", 4);
                }
                break;
        }
        if (plrAtk == 3 || enemyAtk == 3)
            Invoke("applyHeal", 1.4f);

        yield return new WaitForSeconds(1.4f);
        applyFx = false;
        debounce = false;
    }

    void applyHeal()
    {
        AudioSource.PlayClipAtPoint(heal, transform.position, PlayerPrefs.GetInt("soundVol")/10);
        if (plrAtk == 3)
        {
            spriteAnim.SetTrigger("Heal");
            player.Invoke("heal", 0.5f);
        }
        if (enemyAtk == 3)
        {
            enemyAnim.SetTrigger("Heal");
            enemy.hurt(-enemy.healAmt);
        }
    }

    void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void coolDown()
    {
        debounce = false;
    }
}