using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;
    
    Firebase.Auth.FirebaseAuth auth;
    bool IsSignedIn;
    //인증을 관리할 객체

    void Start()
    {
        //객체 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        IsSignedIn = false;
    }

    void Update()
    {
        if(IsSignedIn){
            SceneManager.LoadScene("RestScene");
        }
    }
    public void signin()
    {
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task => {
                if(task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(emailField.text + " 로 로그인 하셨습니다 ! ");
                    IsSignedIn = true;
                }
                else
                {
                    Debug.Log("로그인에 실패하셨습니다 !");
                }
            }
        );
    }

    public void signup(){
        SceneManager.LoadScene("SignupScene");

    }
}
