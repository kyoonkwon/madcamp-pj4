using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;


public class SignUp : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;
    [SerializeField] InputField passwordconfirmField;
    [SerializeField] InputField nameField;

    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference reference;
    bool IsSignedUp;

    
    private class User {
        public string name;
        public int hungry;
        public int health;
        public int affection;
        public string time;

        public User() {
            this.name= "";
            this.hungry = 100;
            this.health = 0;
            this.affection = 50;
            this.time = DateTime.Now.ToLocalTime().ToString();
        }

        public User(string name, int hungry, int health, int affection, string time) {
            this.name = name;
            this.hungry = hungry;
            this.health = health;
            this.affection = affection;
            this.time = time;
        }
    }
    

    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IsSignedUp = false;
    }

    void Update()
    {
        if(IsSignedUp){
            SceneManager.LoadScene("AuthScene");
        }
    }

    public void signup(){
        if(!passwordField.text.Equals(passwordconfirmField.text)){
            Debug.Log("password Missmatch");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task => {
                if(!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(emailField.text + " 로 회원가입\n");
                    IsSignedUp = true;
                }
                else
                {
                    Debug.LogError("회원가입 에러: " + task.Exception);
                    Debug.Log("회원가입 실패\n");
                    return;
                }

                string newUserId = task.Result.UserId;
                Debug.Log(DateTime.Now.ToLocalTime());
                User newUser = new User(nameField.text, 100, 0, 50, DateTime.Now.ToLocalTime().ToString());
                string json = JsonUtility.ToJson(newUser);

                Debug.Log(json);

                reference.Child("users").Child(newUserId).SetRawJsonValueAsync(json);
                SceneManager.LoadScene("AuthScene");
                
            }
        );
    }
}
