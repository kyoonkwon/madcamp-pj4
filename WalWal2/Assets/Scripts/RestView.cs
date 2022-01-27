using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestView : MonoBehaviour
{

    private float Speed = 0.25f;
    public Camera mainCam;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCam= GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                mainCam.transform.Translate(movePos); 
                prePos = touch.position - touch.deltaPosition;
            }
        }
    }
}
