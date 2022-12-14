using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int maxHP;
    [HideInInspector]
    public int currentHP;
    int prevHP;
    public float damage;
    public float detectRange;
    public int healAmt;

    [SerializeField]
    Slider healthBar;

    [HideInInspector]
    public string input;
    AudioSource footStep;
    //string prev = "";
    //string[] directions;

    GameObject currentPoint;
    GameObject nextPoint;
    Animator anim;
    Battle fightManager;

    bool moving;
    [HideInInspector]
    public bool inFight;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        footStep = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        prevHP = maxHP;
        currentHP = maxHP;
        fightManager = GameObject.FindWithTag("GameController").GetComponent<Battle>();
    }
    public void readCard(string s)
    {
        if (!inFight)
            gridWalk(s);
        if (inFight && !fightManager.debounce)
            attack(s);
    }
    void Update()
    {
        //DEBUG KEYBOARD CONTROLS EXCUSE SHITTINESS
        if (!moving)
        {
            //if(input != null)
            //{
            //    gridWalk(input);
            //}
            if (Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Vertical") > 0)
            {
                gridWalk("up");
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
            {
                gridWalk("left");
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < 0)
            {
                gridWalk("down");
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
            {
                gridWalk("right");
            }

            // Enable combat moves
            if (inFight && fightManager.debounce == false)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Fire3"))
                {
                    attack("attack");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Jump"))
                {
                    attack("magic");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("Fire2"))
                {
                    attack("block");
                }
                else if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetButtonDown("Fire1")) && currentHP < maxHP)
                {
                    attack("heal");
                }
            }

            Collider[] nearColliders = Physics.OverlapSphere(transform.position, detectRange);
            if (nearColliders.Length > 0)
                foreach (Collider collider in nearColliders)
                {
                    if (collider.CompareTag("Grid"))
                    {
                        currentPoint = collider.gameObject;
                    }
                    if (collider.CompareTag("Pickup"))
                    {
                        //Debug.Log("YE");
                        collider.GetComponent<GeneralPickup>().pickUp(transform);
                    }
                }
        }
        anim.SetBool("Moving", moving);
        if (nextPoint && currentPoint != nextPoint && Vector3.Distance(transform.position, nextPoint.transform.position) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextPoint.transform.position, ref velocity, 0.1f, 8f);
            moving = true;
        }
        else
        {
            if (GetComponent<VolumeControls>())
            {
                if (GetComponent<VolumeControls>().active == false)
                    footStep.Stop();
            }
            else
            {
                footStep.Stop();
            }

            moving = false;
        }
        // Update healthbar if health changes
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
    }
    void gridWalk(string action)
    {
        footStep.time = 0.2f;
        footStep.Play();
        //Read text file, clean up text, convert to int and process command
        //int action = int.Parse(ReadString());
        GridPoint point = currentPoint.GetComponent<GridPoint>();
        input = action;
        switch (action)
        {
            case "up":
                if (point.up)
                {
                    anim.SetTrigger("Forward");
                    nextPoint = point.up.gameObject;
                }
                break;
            case "left":
                if (point.left)
                {
                    anim.SetTrigger("Left");
                    nextPoint = point.left.gameObject;
                }
                break;
            case "down":
                if (point.down)
                {
                    anim.SetTrigger("Back");
                    nextPoint = point.down.gameObject;
                }
                break;
            case "right":
                if (point.right)
                {
                    anim.SetTrigger("Right");
                    nextPoint = point.right.gameObject;
                }
                break;
            default:
                break;
        }
    }
    void attack(string action)
    {
        fightManager.debounce = true;
        switch (action)
        {
            case "attack":
                fightManager.attack(0);
                break;
            case "magic":
                fightManager.attack(1);
                break;
            case "block":
                fightManager.attack(2);
                break;
            case "heal":
                fightManager.attack(3);
                break;
            default:
                break;
        }
    }
    public void heal()
    {
        hurtheal();
    }
    public void hurtheal(int amt = 0)
    {
        //No amount specified means heal
        if(amt == 0)
        {
            if (currentHP <= maxHP - 20)
                currentHP += healAmt;
            else
                currentHP += (maxHP - currentHP);
        }
        else
        {
            currentHP -= amt;
        }
        Debug.Log(currentHP);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}