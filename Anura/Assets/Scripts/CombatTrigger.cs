using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class CombatTrigger : MonoBehaviour
{
    public GameObject combatCam;
    GameObject player;
    GameObject playerCam;
    CinemachineBrain brain;
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            playerCam = brain.ActiveVirtualCamera.VirtualCameraGameObject;
            player = other.gameObject;
            GameObject midPoint = new GameObject("MidPoint");
            midPoint.transform.position = transform.position + player.transform.position / 2;
            combatCam.GetComponent<CinemachineVirtualCamera>().Follow = midPoint.transform;
            combatCam.GetComponent<CinemachineVirtualCamera>().LookAt = midPoint.transform;

            gameObject.GetComponent<AimConstraint>().constraintActive = true;
            player.GetComponent<AimConstraint>().constraintActive = true;

            combatCam.SetActive(true);
            playerCam.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            exitBattle();
        if (!brain.IsBlending && playerCam.activeSelf)
        {
            gameObject.GetComponent<AimConstraint>().constraintActive = false;
            player.GetComponent<AimConstraint>().constraintActive = false;
        }
        else
        {
            gameObject.GetComponent<AimConstraint>().constraintActive = true;
            player.GetComponent<AimConstraint>().constraintActive = true;
        }
    }
    void exitBattle()
    {
        combatCam.SetActive(false);
        playerCam.SetActive(true);
    }
}