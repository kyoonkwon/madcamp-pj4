using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHandle : MonoBehaviour
{
    public GameObject handle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        centerGameObject(handle, Camera.main);
    }

    void centerGameObject(GameObject gameOBJToCenter, Camera cameraToCenterOBjectTo, float zOffset = 0.5f)
    {
        gameOBJToCenter.transform.position = cameraToCenterOBjectTo.ViewportToWorldPoint(new Vector3(0.5f, 0.2f, cameraToCenterOBjectTo.nearClipPlane + zOffset));
    }
}
