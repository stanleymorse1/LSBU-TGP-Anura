using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int startHP;
    int currentHP;

    static string inputFile;
    //string prev = "";
    //string[] directions;

    GameObject currentPoint;
    GameObject nextPoint;
    bool moving;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
#if UNITY_EDITOR
        inputFile = "Assets/Resources/InputCodes.txt";
#else
        inputFile = Application.persistentDataPath + "/InputCodes.txt";
#endif
        EraseString();
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
            AssetDatabase.ImportAsset(inputFile);
            TextAsset asset = (TextAsset)Resources.Load("InputCodes");
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
            Collider[] nearColliders = Physics.OverlapSphere(transform.position, 1);
            if (nearColliders.Length > 0)
                foreach (Collider collider in nearColliders)
                {
                    if (collider.CompareTag("Grid"))
                    {
                        currentPoint = collider.gameObject;
                    }
                }
        }

        //Detect if the input file has changed, and key is being pressed
        //if(inputFile != prev && new FileInfo(inputFile).Length > 0)
        //{
        //    move();
        //    prev = inputFile;
        //}
        if (nextPoint && currentPoint != nextPoint && Vector3.Distance(transform.position, nextPoint.transform.position) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextPoint.transform.position, ref velocity, 0.3f, 4f);
            moving = true;
        }
        else
        {
            moving = false;
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
                    nextPoint = point.up.gameObject;
                }
                break;
            case 1:
                if (point.left)
                {
                    nextPoint = point.left.gameObject;
                }
                break;
            case 2:
                if (point.down)
                {
                    nextPoint = point.down.gameObject;
                }
                break;
            case 3:
                if (point.right)
                {
                    nextPoint = point.right.gameObject;
                }
                break;
            default:
                break;
        }
        EraseString();
    }
    void move(Transform start, Transform end)
    {
        transform.position = Vector3.SmoothDamp(start.position, end.position, ref velocity, 1f*Time.deltaTime, 1f*Time.deltaTime);
    }
}
