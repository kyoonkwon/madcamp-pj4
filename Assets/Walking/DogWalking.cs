using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogWalking : MonoBehaviour
{
    public GameObject handle;
    private Vector3 startHandlePos;

    // Start is called before the first frame update
    void Start()
    {
        startHandlePos = handle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(startHandlePos.x - handle.transform.position.x < -0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(1f, transform.position.y, transform.position.z), 0.5f * Time.deltaTime);
        } else if(startHandlePos.x - handle.transform.position.x > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1f, transform.position.y, transform.position.z), 0.5f * Time.deltaTime);
        }
    }
}
