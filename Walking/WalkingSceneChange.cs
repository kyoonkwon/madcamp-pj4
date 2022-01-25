using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkingSceneChange : MonoBehaviour
{
    public void SceneChangeToNormal()
    {
        SceneManager.LoadScene("Walking");
    }

    public void SceneChangeToAR()
    {
        SceneManager.LoadScene("WalkingAR");
    }
}
