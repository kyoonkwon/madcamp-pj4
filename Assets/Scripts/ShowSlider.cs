using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSlider : MonoBehaviour
{   
    public Slider loveslider;
    public Slider hpslider;
    public Slider hungerslider;
    int full;
    int love;
    int hp;
    int hunger;
    // Start is called before the first frame update
    void Start()
    {
        full=100;
        love=30;
        hp=80;
        hunger=30;
    }

    // Update is called once per frame
    void Update()
    {
        loveslider.value=(float)love/full;
        hpslider.value=(float)hp/full;
        hungerslider.value=(float)hunger/full;
    }

}
