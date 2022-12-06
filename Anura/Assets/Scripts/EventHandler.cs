using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    Battle battleScript;
    void Start()
    {
        battleScript = GameObject.FindWithTag("GameController").GetComponent<Battle>();
    }
    public void changeBool()
    {
        battleScript.applyFx = true;
        //Invoke("resetBool", Time.deltaTime);
    }
    void resetBool()
    {
        battleScript.applyFx = false;
        Debug.Log("AAAAAAAAAA");
    }
}
