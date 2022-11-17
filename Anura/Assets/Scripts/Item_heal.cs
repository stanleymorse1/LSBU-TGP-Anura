using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_heal : MonoBehaviour
{
    public void heal(Transform player)
    {
        player.GetComponent<PlayerController>().hurtheal(30);
    }
}
