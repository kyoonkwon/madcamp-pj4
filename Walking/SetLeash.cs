using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetLeash : MonoBehaviour
{
    public GameObject Dog;
    public GameObject Handle;
    public int lengthOfLineRenderer = 20;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.positionCount = (lengthOfLineRenderer+1);
    }

    // Update is called once per frame
    void Update()
    {

        if(Dog != null)
        {
            LineRenderer line = GetComponent<LineRenderer>();

            Vector3 dogPos = Dog.transform.position + new Vector3(0f, 0.2f, 0f);
            Vector3 handlePos = Handle.transform.position;

            line.SetPosition(0, dogPos);
            line.SetPosition(lengthOfLineRenderer, handlePos);

            for(var i=1;i<lengthOfLineRenderer;i++){

                var r = ((float) i / lengthOfLineRenderer);
                Vector3 pos = handlePos * r + dogPos * (1 - r);

                pos = pos - new Vector3(0, (float) Math.Sin(r * Math.PI) * 0.6f, 0);

                line.SetPosition(i, pos);

            }
        } else
        {
            Dog = GameObject.Find("Dog Prefab(Clone)");
            //Debug.Log(dog);
        }

        

    }
}
