using System.Collections;

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DogManagerAR : MonoBehaviour
{
    public GameObject DogPrefab;
    public ReticleBehaviour Reticle;
    public DrivingSurfaceManager DrivingSurfaceManager;

    public DogWalkingAR Dog;

    private void Update()
    {
        if (Dog == null && WasTapped() && Reticle.CurrentPlane != null)
        {
            // Spawn our car at the reticle location.
            var obj = GameObject.Instantiate(DogPrefab);
            Dog = obj.GetComponent<DogWalkingAR>();
            Dog.Reticle = Reticle;
            Dog.transform.position = Reticle.transform.position;
            //for ai dog. set target, return point.
            var aiDog = obj.GetComponent<DogAgent>();
            aiDog.stick=Reticle.Child;

            DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);
        }
    }

    private bool WasTapped()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount == 0)
        {
            return false;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
        {
            return false;
        }

        return true;
    }
}
