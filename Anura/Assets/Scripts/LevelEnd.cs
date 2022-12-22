using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public Animator canvasAnim;
    public Animator doorAnim;
    public int scene;

    public void openDoor()
    {
        doorAnim.SetTrigger("OpenDoor");
        Invoke("fade", 3);
    }
    void fade()
    {
        canvasAnim.SetTrigger("EnterBattle");
        Invoke("sceneTransition", 1.1f);
    }
    void sceneTransition()
    {
        SceneManager.LoadScene(scene);
    }
}
