using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public DragHandle draghandle;
    
    void OnTriggerEnter(Collider collision) {
        Debug.Log("collision");
        if (collision.gameObject.tag == "bomb") {
            Debug.Log("package");
            draghandle.numpackage +=1;
        }

        else if (collision.gameObject.tag == "package") {
            draghandle.numbomb +=1;
        }

    }
}
