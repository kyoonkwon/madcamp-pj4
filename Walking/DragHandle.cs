using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragHandle : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public GameObject target;
    public GameObject Dog;
    public Transform ground;
    public Transform Empty;

    private float distance;

    void Start()
    {
        distance = Dog.transform.position.z - gameObject.transform.position.z;
    }

    void Update()
    {
        if(target.transform.position.z < 50){
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, Dog.transform.position.z + distance);
            transform.position = new Vector3(transform.position.x, transform.position.y, Dog.transform.position.z - distance);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z - 0.5f);
            RotateCam();
        } else {
            SceneManager.LoadScene("RestScene");
        }
    }

    void RotateCam()
    {
        float speed = 1.0f;
        Vector3 targetDirection = Dog.transform.position - Camera.main.transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(Camera.main.transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(Camera.main.transform.position, newDirection, Color.red);
        Camera.main.transform.rotation = Quaternion.LookRotation(newDirection);
    }
 
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
 
    }
 
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
        
    }
}
