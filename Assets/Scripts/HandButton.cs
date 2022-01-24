using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandButton : MonoBehaviour
{   
    public Image image;
    public GameObject hand;
    public Animator animator;

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
        }
    }

    public void OnclickBone() {
        // 밥 먹이는 코드 블라블라
        

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
}
