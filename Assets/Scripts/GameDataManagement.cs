using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;

public class GameDataManagement : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference reference;
    public Text Name;
    public Text Affection;
    public Text Hungry;
    public Text Health;

    Dictionary <string, object> dic;
    object userData;
    object userId;

    string username;
    int hungry;
    int health;
    int affection;
    string time;


    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Name = GameObject.Find("Name").GetComponent<Text>();
        Affection = GameObject.Find("Affection").GetComponent<Text>();
        Hungry = GameObject.Find("Hungry").GetComponent<Text>();
        Health = GameObject.Find("Health").GetComponent<Text>();
        Affection = GameObject.Find("Affection").GetComponent<Text>();

        dic = new Dictionary<string, object>();
        userId = auth.CurrentUser.UserId;
        Debug.Log(userId);
        InitializeData();
        StartCoroutine(Timeflow());
    }

    IEnumerator Timeflow(){
        WaitForSeconds ws = new WaitForSeconds(2.0F);
        while(true){
            yield return ws;

            if(health < 100){
                health += 1;
            }
            if(hungry > 0){
                hungry -= 1;
            }
            if(affection > 0){
                affection -= 1;
            }

            reference.Child("users").Child(userId.ToString()).Child("affection").SetValueAsync(affection);
            reference.Child("users").Child(userId.ToString()).Child("hungry").SetValueAsync(hungry);
            reference.Child("users").Child(userId.ToString()).Child("health").SetValueAsync(health);
            reference.Child("users").Child(userId.ToString()).Child("time").SetValueAsync(DateTime.Now.ToLocalTime().ToString());
        }

    }
    void Update(){
        ViewData();
    }

    public void InitializeData(){
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted) {
                Debug.Log("Read Data Error");
            }
            else {
               DataSnapshot snapshot = task.Result;
               
               foreach(DataSnapshot data in snapshot.Children){
                   if(userId.Equals(data.Key)){
                        IDictionary userdata = (IDictionary)data.Value;
                        Debug.Log(data.Key);
                        username = userdata["name"].ToString();
                        Debug.Log(username);
                        affection = Convert.ToInt32(userdata["affection"].ToString());
                        Debug.Log(affection);
                        hungry = Convert.ToInt32(userdata["hungry"].ToString());
                        Debug.Log(hungry);
                        health = Convert.ToInt32(userdata["health"].ToString());
                        Debug.Log(health);
                        time = userdata["time"].ToString();
                        Debug.Log(time);
                        ElapsedTime();
                   }
               }
            }
        });
    }

    public void ViewData(){
        Name.text = "Name: " + username;
        Affection.text = "Affection: " + affection;
        Hungry.text = "Hungry: " + hungry;
        Health.text = "Health: " + health;

        /*
        if(username != null)
        {
            Name.text = "Name: " + username;
            Affection.text = "Affection: " + affection;
            Hungry.text = "Hungry: " + hungry;
            Health.text = "Health: " + health;
        }
        */
    }

    public void ElapsedTime(){
        DateTime lastTime = DateTime.Parse(time);
        Debug.Log(lastTime);
        DateTime now = DateTime.Now.ToLocalTime();
        Debug.Log(now);
        var span = now - lastTime;
        Debug.Log(span);
        int timestamp = (int)span.TotalSeconds;
        Debug.Log(timestamp);

    }

    public void IncreaseAffection(){
        if(affection < 100) {
            affection += 1;
        }
        reference.Child("users").Child(userId.ToString()).Child("affection").SetValueAsync(affection);
    }

    public void IncreaseHungry(){
        if(hungry < 100) {
            hungry += 1;
        }
        reference.Child("users").Child(userId.ToString()).Child("hungry").SetValueAsync(hungry);
    }

    public void IncreaseHealth() {
        if(health < 100) {
            health += 1;
        }
        reference.Child("users").Child(userId.ToString()).Child("health").SetValueAsync(health);
    }
}