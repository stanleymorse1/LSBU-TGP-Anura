using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CamTrigger : MonoBehaviour
{
    [SerializeField]
    Vector3 triggerSize;
    public UnityEvent enter;
    public UnityEvent exit;
    public bool cam = true;
    bool debounce;
    int prev;

    private void FixedUpdate()
    {
        Collider[] nearColliders = Physics.OverlapBox(transform.position, triggerSize/2);
        if (nearColliders.Length > 0)
            foreach (Collider collider in nearColliders)
            {
                if (collider.CompareTag("Player") && !debounce)
                {
                    if(cam)
                        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
                    enter.Invoke();
                    debounce = true;
                }
                if (prev > nearColliders.Length)
                {
                    debounce = false;
                    exit.Invoke();
                    Debug.Log("Exited");
                }
                prev = nearColliders.Length;
            }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, triggerSize);
    }
}
