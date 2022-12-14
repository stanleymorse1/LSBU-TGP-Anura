using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    int prevHP;
    int currentHP;
    public Slider healthBar;
    public Battle fightManager;
    public List<float> atkWeights = new List<float>();
    public bool boss = false;
    public int healAmt = 10;

    bool db;
    public int damage;
    private void Start()
    {
        prevHP = maxHP;
        currentHP = maxHP;
        fightManager = GameObject.FindWithTag("GameController").GetComponent<Battle>();
    }
    public void hurt(int amount)
    {
        currentHP -= amount;
    }
    private void Update()
    {
        if(currentHP != prevHP)
        {
            float cFrac = (float)currentHP/maxHP;
            float barVal = Mathf.Lerp(healthBar.value, cFrac, 0.1f);
            healthBar.value = barVal;
            //Only update previous health when bar settles
            if (healthBar.value <= cFrac + 0.01f && healthBar.value >= cFrac -0.01f)
                prevHP = currentHP;
            if (barVal == 0)
            {
                healthBar.fillRect.gameObject.SetActive(false);
            }
        }
        if (currentHP <= 0 && !db)
        {
            db = true;
            //healthBar.fillRect.gameObject.SetActive(false);
            Debug.Log("Death");
            fightManager.endFight();
            //gameObject.GetComponent<CombatTrigger>().exitBattle();
        }
    }
    private void OnDestroy()
    {
        gameObject.GetComponent<CombatTrigger>().exitBattle();
    }
}