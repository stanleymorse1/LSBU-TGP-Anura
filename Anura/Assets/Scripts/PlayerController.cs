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
    int currentHP;
    int prevHP;
    public float damage;
    public float detectRange;

    [SerializeField]
    Slider healthBar;

    string input;
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
        anim = GetComponent<Animator>();
        prevHP = maxHP;
        currentHP = maxHP;
        fightManager = GameObject.FindWithTag("GameController").GetComponent<Battle>();
    }
    public void readCard(string s)
    {
        if(!inFight)
            gridWalk(s);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            hurtheal(30);
        }
        //DEBUG KEYBOARD CONTROLS EXCUSE SHITTINESS
        if (!moving)
        {
            //if(input != null)
            //{
            //    gridWalk(input);
            //}
            if (Input.GetKeyDown(KeyCode.W))
            {
                gridWalk("up");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                gridWalk("left");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                gridWalk("down");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gridWalk("right");
            }

            // Enable combat moves
            if (inFight)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    fightManager.attack(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    fightManager.attack(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    fightManager.attack(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && currentHP < maxHP)
                {
                    hurtheal(20);
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
            transform.position = Vector3.SmoothDamp(transform.position, nextPoint.transform.position, ref velocity, 0.1f, 4f);
            moving = true;
        }
        else
        {
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
        if(currentHP <= 0)
        {
            //End game screen
        }
    }
    void gridWalk(string action)
    {
        //Read text file, clean up text, convert to int and process command
        //int action = int.Parse(ReadString());
        GridPoint point = currentPoint.GetComponent<GridPoint>();
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

    public void hurtheal(int amount)
    {
        currentHP += amount;
        Debug.Log(currentHP);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}