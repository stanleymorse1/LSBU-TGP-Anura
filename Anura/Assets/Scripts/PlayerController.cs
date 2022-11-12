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

    static string inputFile;
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
#if UNITY_EDITOR
        inputFile = "Assets/Resources/InputCodes.txt";
#else
        inputFile = Application.persistentDataPath + "/InputCodes.txt";
#endif
        anim = GetComponent<Animator>();
        EraseString();
        prevHP = maxHP;
        currentHP = maxHP;
        fightManager = GameObject.FindWithTag("GameController").GetComponent<Battle>();
    }
    public static void WriteString(string s)
    {
        //Checks if the input is already stored (debounce)
        //string[] inputs = inputFile.Split("\n");

        //if (!Array.Exists(inputs, p => p == s))
        if (new FileInfo(inputFile).Length == 0)
        {
            StreamWriter writer = new StreamWriter(inputFile, true);
            writer.Write(s);
            writer.Close();
            //Re-import the file to update the reference in the editor
            //AssetDatabase.ImportAsset(inputFile);
            //TextAsset asset = (TextAsset)Resources.Load("InputCodes");
        }

    }
    public static string ReadString()
    {
        //Read the text from directly from the txt file
        StreamReader reader = new StreamReader(inputFile);
        string output = reader.ReadToEnd();
        Debug.Log(output);
        reader.Close();
        return output;
    }
    //THIS SHOULD BE HANDLED IN PYTHON SCRIPT, REMOVE WHEN USING WITH SCANNER
    public void EraseString()
    {
        File.WriteAllText(inputFile, "");
        //AssetDatabase.ImportAsset(inputFile);
        //TextAsset asset = (TextAsset)Resources.Load("InputCodes");
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                WriteString("0");
                gridWalk();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                WriteString("1");
                gridWalk();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                WriteString("2");
                gridWalk();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                WriteString("3");
                gridWalk();
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
                        Debug.Log("YE");
                        collider.GetComponent<GeneralPickup>().pickUp(transform);
                    }
                }
        }

        //Detect if the input file has changed, and key is being pressed
        //if(inputFile != prev && new FileInfo(inputFile).Length > 0)
        //{
        //    move();
        //    prev = inputFile;
        //}
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
    void gridWalk()
    {
        //Read text file, clean up text, convert to int and process command
        int action = int.Parse(ReadString());
        GridPoint point = currentPoint.GetComponent<GridPoint>();
        switch (action)
        {
            case 0:
                if (point.up)
                {
                    anim.SetTrigger("Forward");
                    nextPoint = point.up.gameObject;
                }
                break;
            case 1:
                if (point.left)
                {
                    anim.SetTrigger("Left");
                    nextPoint = point.left.gameObject;
                }
                break;
            case 2:
                if (point.down)
                {
                    anim.SetTrigger("Back");
                    nextPoint = point.down.gameObject;
                }
                break;
            case 3:
                if (point.right)
                {
                    anim.SetTrigger("Right");
                    nextPoint = point.right.gameObject;
                }
                break;
            default:
                break;
        }
        EraseString();
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