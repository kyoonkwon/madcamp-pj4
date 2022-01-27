using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkingSlider : MonoBehaviour
{
    Slider slider;
    private float Timer;
    public Transform corgi;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        Timer=0.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        Timer+=Time.deltaTime;
        slider.value = (float)Timer;
    }
}
