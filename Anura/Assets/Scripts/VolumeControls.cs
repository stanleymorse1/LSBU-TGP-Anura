using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeControls : MonoBehaviour
{
    public bool active;
    public int sound = 10;
    public int music = 10;
    public string changing;

    public GameObject musicSign;
    public GameObject soundSign;

    public loadVolume loadVolume;

    AudioSource step;
    private void Start()
    {
        sound = PlayerPrefs.GetInt("soundVol");
        music = PlayerPrefs.GetInt("musicVol");
        step = GetComponent<AudioSource>();
    }

    public void editMusic()
    {
        changing = "music";
        active = !active;
        step.Stop();
    }
    public void editSound()
    {
        changing = "sound";
        active = !active;
        step.Stop();
    }
    private void Update()
    {
        musicSign.GetComponent<TextMeshPro>().text = "Music: " + music;
        soundSign.GetComponent<TextMeshPro>().text = "SFX: " + sound;
        if (active)
        {
            Debug.Log("going, input is " + GetComponent<PlayerController>().input);
            if (GetComponent<PlayerController>().input == "left")
            {
                if(changing == "sound" && sound >0)
                {
                    sound -= 1;
                    step.time = 0.2f;
                    step.Play();
                    Invoke("cease", 0.5f);
                }
                else if (changing == "music" && music >0)
                {
                    music -= 1;
                    step.time = 0.2f;
                    step.Play();
                    Invoke("cease", 0.5f);
                }
            }
            else if (GetComponent<PlayerController>().input == "right")
            {
                if (changing == "sound" && sound <10)
                {
                    sound += 1;
                    step.Play();
                    Invoke("cease", 0.5f);
                }
                else if (changing == "music" && music <10)
                {
                    music += 1;
                    step.time = 0.2f;
                    step.Play();
                    Invoke("cease", 0.5f);
                }
            }
            if (GetComponent<PlayerController>().input != null)
            {
                PlayerPrefs.SetInt("soundVol", sound);
                PlayerPrefs.SetInt("musicVol", music);
                loadVolume.applyVolume();
            }
            GetComponent<PlayerController>().input = null;
        }
    }
    void cease()
    {
        step.Stop();
    }
}
