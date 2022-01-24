using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{   

    public string restScene = "RestScene";

    public void OnClick1() {
        SceneManager.LoadScene("warphago");
        Debug.Log("play click");
    }

    public void OnClick2() {
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
