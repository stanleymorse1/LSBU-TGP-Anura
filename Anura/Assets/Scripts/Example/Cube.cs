using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Cube : MonoBehaviour
{
    public Material red;

    public Material yellow;

    public Material blue;

    public Material green;

    private MeshRenderer mr;

    private bool normalMaterial = true;

    private float coolDown = 1f;

    private float nextTriggerTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        var d = Time.time;
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    public void Trigger(string colour)
    {
        print(colour);
        Material currentColor = null;
        if (colour.Contains("yellow")) currentColor = yellow;
        if (colour.Contains("red")) currentColor = red;
        if (colour.Contains("blue")) currentColor = blue;
        if (colour.Contains("green")) currentColor = green;
        if (Time.time > nextTriggerTime)
        {
            mr.material = currentColor;

            nextTriggerTime = Time.time + coolDown;
        }
    }
}
