using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEvent : MonoBehaviour
{
    public GameObject hand;
    public GameObject bowl;
    public Animator animator;
    public Animator bapanimator;

    public void AfterMove() {
        animator.SetBool("isclick",false);
        hand.transform.Translate(new Vector3(130,0,-140));
    }

    public void AfterEat() {
        bapanimator.SetBool("isbap",false);
        bowl.SetActive(false);
    }
}
