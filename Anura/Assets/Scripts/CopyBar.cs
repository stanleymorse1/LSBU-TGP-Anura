using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyBar : MonoBehaviour
{
    public Slider bar;
    void Update()
    {
        if(bar)
            GetComponent<Slider>().value = bar.value;
    }
}
