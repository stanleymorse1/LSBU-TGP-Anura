using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralPickup : MonoBehaviour
{
    Transform player;
    public UnityEvent collectEvent;
    float spd = 1;
    public void pickUp(Transform plr)
    {
        player = plr;
    }

    void Update()
    {
        if(player != null)
        {
            spd += 0.1f;
            // Magnetize pickup to player
            transform.Translate((player.position - this.transform.position).normalized * spd * Time.deltaTime);
            // if pickup is close enough, execute collect event
            if (Vector3.Distance(player.position, this.transform.position) < 0.2f)
            {
                Debug.Log("collected");
                collectEvent.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
