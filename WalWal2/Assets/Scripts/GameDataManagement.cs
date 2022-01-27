using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class GameDataManagement : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference reference;
    public TextMeshProUGUI Name;
    //public Text Affection;
    //public Text Hungry;
    //public Text Health;
    public Slider loveslider;
    public Slider hpslider;
    public Slider hungerslider;

    Dictionary <string, object> dic;
    object userData;
    object userId;

    string username;
    int hungry;
    int health;
    public int affection;
    string time;


    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Name = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
        //Affection = GameObject.Find("Affection").GetComponent<Text>();
        //Hungry = GameObject.Find("Hungry").GetComponent<Text>();
        //Health = GameObject.Find("Health").GetComponent<Text>();

        dic = new Dictionary<string, object>();
        userId = auth.CurrentUser.UserId;
        Debug.Log(userId);
        InitializeData();
        StartCoroutine(Timeflow());
    }

    IEnumerator Timeflow(){
        WaitForSeconds ws = new WaitForSeconds(10.0F);
        while(true){
            yield return ws;

            if(health < 100){ 
                health += 2;
            }
            if(hungry > 0){
                hungry -= 2;
            }
            if(affection > 0){
                affection -= 2;
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
        Name.text = username;
        /*Affection.text = "Affection: " + affection;
        Hungry.text = "Hungry: " + hungry;
        Health.text = "Health: " + health;*/
        loveslider.value=(float)affection/100;
        hpslider.value=(float)health/100;
        hungerslider.value=(float)hungry/100;


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

    public void IncreaseAffection(int delta){
        if(affection+delta > 100) {
            affection =100;
        }

        else if(affection+delta<0) {
            affection =0;
        }

        else {
            affection += delta;
        }
        reference.Child("users").Child(userId.ToString()).Child("affection").SetValueAsync(affection);
    }

    public void IncreaseHungry(int delta){
        if(hungry+delta > 100) {
            hungry =100;
        }

        else if(hungry+delta<0) {
            hungry =0;
        }

        else {
            hungry += delta;
        }
        reference.Child("users").Child(userId.ToString()).Child("hungry").SetValueAsync(hungry);
    }

    public void IncreaseHealth(int delta) {
        if(health+delta > 100) {
            health =100;
        }

        else if(health+delta<0) {
            health =0;
        }

        else {
            health += delta;
        }
        reference.Child("users").Child(userId.ToString()).Child("health").SetValueAsync(health);
    }
}