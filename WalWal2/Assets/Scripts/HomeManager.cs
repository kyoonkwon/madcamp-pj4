using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeManager : MonoBehaviour
{   

    public Slider affectionslider;
    public Slider healthslider;
    public Slider hungryslider;
    public TextMeshProUGUI Name;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DBManager.Timeflow());
    }

    // Update is called once per frame
    void Update()
    {   
        hungryslider.value = (float) DBManager.hungry/100;
        healthslider.value = (float) DBManager.health/100;
        affectionslider.value = (float) DBManager.affection/100;
        Name.text = DBManager.username;
    }
}
