﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{   

    public string restScene = "RestScene";

    public void OnClick1() {

        DBManager.IncreaseAffection(10);
        DBManager.IncreaseHungry(-5);
        DBManager.IncreaseHealth(-5); 
        SceneManager.LoadScene("warphago");
        Debug.Log("play click");
    }

    public void OnClick2() {
        DBManager.IncreaseAffection(15);
        DBManager.IncreaseHungry(-10);
        DBManager.IncreaseHealth(-5); 
        SceneManager.LoadScene("Walking2");
        Debug.Log("walk click");
    }
 
    public void Onclick3() {
        SceneManager.LoadScene(restScene);
        Debug.Log("rest click");
    }

    public void ExitClick() {
        Debug.Log("exit click");
        Application.Quit();
    
    }


}
