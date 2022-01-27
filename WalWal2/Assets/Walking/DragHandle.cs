using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragHandle : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private float Timer;
    public float speed=0.5f;

    public GameObject target;
    public GameObject Dog;
    public Transform ground;
    public Transform Empty;
    public Text packagetext;
    public Text bombtext;
    public Text scoretext;
    public GameObject ending;

    public int numpackage=0;
    public int numbomb=0;

    private float distance;

    void Start()
    {
        distance = Dog.transform.position.z - gameObject.transform.position.z;
        Timer = 0.0f;

    }

    void Update()
    {   
        Timer += Time.deltaTime;
        if(Timer<30){

            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, Dog.transform.position.z + distance);

            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

            if (pos.x < 0.1f) pos.x = 0.5f;
            if (pos.x > 0.9f) pos.x = 0.5f;
            pos.y=0.2f;
            transform.position = Camera.main.ViewportToWorldPoint(pos);


            transform.position = new Vector3(transform.position.x, transform.position.y, Dog.transform.position.z - distance);
            //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z - 0.5f);
            //RotateCam();
        } else if (Timer <35) {
            

            ending.SetActive(true);
             for (int i = 0; i < ending.transform.childCount; i++)
            {
                ending.transform.GetChild(i).gameObject.SetActive(true);
            }
            packagetext.text=numpackage.ToString();
            bombtext.text=numbomb.ToString();
            scoretext.text=(numpackage*3-numbomb*2).ToString();
        }

        else {   
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
        offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Debug.Log(offset);
 
    }
 
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = new Vector3(curPosition.x, transform.position.y,  Dog.transform.position.z-distance);
        //target.transform.position= new Vector3(target.transform.position.x + offset.x, target.transform.position.y, Dog.transform.position.z + distance);
    }

    void OnMouseUp()
    {   

        target.transform.position= new Vector3(target.transform.position.x + (transform.position.x - offset.x)*2, target.transform.position.y, Dog.transform.position.z + distance);
        transform.position = new Vector3(Dog.transform.position.x, transform.position.y,  Dog.transform.position.z-distance);
    }
}
