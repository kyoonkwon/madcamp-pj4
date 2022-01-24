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
        //객체 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void Logout(){
        Debug.Log("로그아웃");
        auth.SignOut();
        SceneManager.LoadScene("AuthScene");
    }
}
