using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStick : MonoBehaviour
{
    public Transform item;
    public GameObject stick;
    public Rigidbody itemRB;
    public Collider itemCol;
    public Transform returnPoint;

    public Vector3 holdingPositionOffset; 
    Vector3 holdingPos; 
    public float holdingItemTargetVelocity; 
    public float holdingItemMaxVelocityChange; 
    public float throwSpeed; 
    private Vector3 throwDir;

    Vector3 startingPos; 
    Vector3 currentPos; 
    bool currentlyTouching; 
    Touch currentTouch; 
    bool usingTouchInput; 
    bool usingMouseInput;
    public bool canThrow;
    public bool throwDone;
    Camera cam;


    void Awake(){
        cam = Camera.main;
        item = stick.transform;
        itemRB = item.GetComponent<Rigidbody>();
        itemCol = item.GetComponent<Collider>();
        canThrow = true;
        throwSpeed = 2.5f;
        throwDone = false;
        
    }

    void StartSwipe(){
        startingPos = cam.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);
		usingTouchInput = true;
		currentlyTouching = true;
		currentTouch = Input.GetTouch(0);
        Debug.Log("StartSwipe");
    }

    void StartMouseDrag(){
        startingPos = cam.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);
        //item.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
		usingMouseInput = true;
		currentlyTouching = true;
        Debug.Log("StartMouseDrag");
    }

    void ThrowItem(){
        canThrow = false;
        itemRB.velocity *= .5f;
        itemRB.useGravity = true;
        throwDir = (currentPos - startingPos);
		var dir = cam.transform.TransformDirection(throwDir) + cam.transform.forward;
		dir.y = 2;
        Debug.Log(dir);
        item.localPosition += new Vector3(0f, 1f, 0f);
		itemRB.AddForce(dir * throwSpeed, ForceMode.VelocityChange);
        /*item.localPosition = new Vector3(Random.value * 16 - 8f,
                                            Random.value * 10 - 2f,
                                            1.13f);*/
        Debug.Log("ThrowItem");
        canThrow = true;
        throwDone = true;
    }


    void Update()
    {
        if(canThrow)
		{
			if (Input.touchCount > 0 && !currentlyTouching)
			{
				currentTouch = Input.GetTouch(0);
				if(currentTouch.phase == TouchPhase.Began)
				{
                    item.position = cam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1));
                    itemRB.useGravity = false;
					StartSwipe();
				}
			}

			if(usingTouchInput && currentlyTouching)
			{
				currentTouch = Input.GetTouch(0);
				if(currentTouch.phase == TouchPhase.Ended)
				{
                    currentlyTouching = false;
					ThrowItem();
				}
			}

			if (Input.GetMouseButtonDown(0) && !currentlyTouching)
			{
                item.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                itemRB.useGravity = false;
				StartMouseDrag();
			}

			if(usingMouseInput && currentlyTouching)
			{
				if(Input.GetMouseButtonUp(0))
				{
					currentlyTouching = false;
					ThrowItem();
				}
			}
		}
    }

}
