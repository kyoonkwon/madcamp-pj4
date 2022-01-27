using System.Collections;
using UnityEngine;

public class DogWalkingAR : MonoBehaviour
{
    public ReticleBehaviour Reticle;
    public float Speed = 1.2f;

    private void Update()
    {
        if(Reticle != null){
            var trackingPosition = Reticle.transform.position;
            if (Vector3.Distance(trackingPosition, transform.position) < 0.1)
            {
                return;
            }

            var lookRotation = Quaternion.LookRotation(trackingPosition - transform.position);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            transform.position =
                Vector3.MoveTowards(transform.position, trackingPosition, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var Package = other.GetComponent<PackageBehaviour>();
        if (Package != null)
        {
            Destroy(other.gameObject);
        }
    }
}
