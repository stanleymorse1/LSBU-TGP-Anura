using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadVolume : MonoBehaviour
{
    public AudioSource[] Music;
    public AudioSource[] Sound;
    void Start()
    {
        applyVolume();
    }

    public void applyVolume()
    {
        foreach (AudioSource a in Music)
        {
            a.volume = (float)PlayerPrefs.GetInt("musicVol") / 10;
        }
        foreach (AudioSource a in Sound)
        {
            a.volume = (float)PlayerPrefs.GetInt("soundVol") / 10;
        }
    }
}
