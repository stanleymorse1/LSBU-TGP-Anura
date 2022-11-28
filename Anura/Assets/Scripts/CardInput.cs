using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class CardInput : MonoBehaviour
{
    public PlayerController player;
    SerialPort stream;
    bool isStreaming = false;

    Dictionary<string, string> inputs = new Dictionary<string, string>();
    void Start()
    {
        inputs.Add("25005F4DBC8B", "up");
        inputs.Add("25005ED9EE4C", "left");
        inputs.Add("25005E5B2E0E", "down");
        inputs.Add("25005F2799C4", "right");


        stream = new SerialPort("COM5", 9600);//COM6 for laptop, COM3 for uni FIX THIS TO BE AUTOMATIC
        stream.ReadTimeout = 100;
        stream.Open();
        isStreaming = true;
        //stream.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStreaming == false)
        {
            stream.Open();
            isStreaming = true;
        }
        else if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null)
            {
                print(value);
                if (inputs.ContainsKey(value))
                {
                    player.readCard(inputs[value]);
                }
            }

        }
        //print(stream.ReadLine());
    }

    string ReadSerialPort(int timeout = 60)
    {
        string message;
        stream.ReadTimeout = timeout;
        try
        {
            message = stream.ReadLine();
            return message;
        }
        catch (TimeoutException)
        {
            stream.BaseStream.Flush();
            stream.Close();
            isStreaming = false;
            return null;
        }
    }
}
