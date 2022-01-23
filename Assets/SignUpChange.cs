using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void Change()
    {
        SceneManager.LoadScene("SignupScene");
    }
}
