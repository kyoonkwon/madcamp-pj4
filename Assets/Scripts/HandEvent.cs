using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEvent : MonoBehaviour
{
    public GameObject hand;
    public Animator animator;

    public void AfterMove() {
        animator.SetBool("isclick",false);
        hand.transform.Translate(new Vector3(130,0,-140));
    }
}
