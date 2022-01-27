using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class HandButton : MonoBehaviour
{   

    public Image image;
    public GameObject hand;
    public Animator animator;
    public Animator bapanimator;
    public Light backlight;
    public GameObject bowl;
    public AudioSource heartsound;


    //하트 버튼 클릭시 이벤트 처리
    public void OnclickHeart() {
        //if (EventSystem.current.IsPointerOverGameObject(-1)==false)

        if(animator.GetBool("isclick")==false) {
            hand.transform.Translate(new Vector3(-130,0,140));
            animator.SetBool("isclick",true);
            Color icolor = image.color;
            icolor.a = 1f;
            image.color=icolor;
            StartCoroutine("FadeIn");

            heartsound.Play();
            DBManager.IncreaseAffection(20);
            //reference.Child("users").Child(userId.ToString()).Child("affection").SetValueAsync(affection);

        }
    }

    public void OnclickBone() {
        // 밥 먹이는 코드 블라블라
        if(bapanimator.GetBool("isbap")==false) {
            bowl.SetActive(true);
            bapanimator.SetBool("isbap",true);

            DBManager.IncreaseHungry(20);
        }

    }

        public IEnumerator FadeIn()              
    {
        Color color = image.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / 5;
            image.color = color;
            yield return null;
        }
    }
    
        public IEnumerator FadeOut()
    {
        Color color = image.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / 2;
            image.color = color;
            yield return null;
        }
    }

    public void OnClickAR() {
        SceneManager.LoadScene("WalkingAR");
    }

    public void OnLight() {
        //불 키고 끄기
        if(backlight.intensity<=0.3f){
            backlight.intensity=1f;
        }    

        else {
            backlight.intensity=0.3f;
        }
    }

}
