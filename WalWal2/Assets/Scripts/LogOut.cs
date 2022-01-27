using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LogOut : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;

    void Start()
    {
    }

    public void Logout(){
        Debug.Log("로그아웃");
        DBManager.auth.SignOut();
        DBManager.username=null;
        SceneManager.LoadScene("AuthScene");
    }
}
