using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class PortRead : MonoBehaviour
{
    public Cube cube;
    SerialPort stream;
    bool isStreaming = false;

    Dictionary<string, string> colours = new Dictionary<string, string>();
    void Start()
    {
        colours.Add("25005F4DBC8B", "green");
        colours.Add("25005ED9EE4C", "blue");
        colours.Add("25005E5B2E0E", "yellow");
        colours.Add("25005F2799C4", "red");


        stream = new SerialPort("COM5", 9600);//COM6 for laptop, COM3 for uni
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
        } else if (isStreaming) {
            string value = ReadSerialPort();
            if (value!=null)
            {
                print(value);
                if (colours.ContainsKey(value))
                {
                    cube.Trigger(colours[value]);
                }
            }
            
        }
        //print(stream.ReadLine());
    }

    string ReadSerialPort(int timeout =50)
    {
        string message;
        //stream.ReadTimeout = timeout;
        try
        {
            message = stream.ReadLine();
            return message;
        }
        catch(TimeoutException)
        {
            stream.BaseStream.Flush();
            stream.Close();
            isStreaming=false;
            return null;
        }
    }
}
