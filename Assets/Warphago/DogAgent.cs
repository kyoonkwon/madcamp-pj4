using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgentsExamples;

public class DogAgent : Agent
{

    // The target the dog will run towards.
    public Transform target;
    public Transform head;

    public GameObject stick;
    public GameObject mouseStick;
    public Transform returnPoint;

    // These items should be set in the inspector
    [Header("Body Parts")] 
    public Transform mouthPosition;
    public Transform body;
    public Transform leg0_upper;
    public Transform leg1_upper;
    public Transform leg2_upper;
    public Transform leg3_upper;
    public Transform leg0_lower;
    public Transform leg1_lower;
    public Transform leg2_lower;
    public Transform leg3_lower;

    // These determine how the dog should be able to rotate around the y axis
    [Header("Body Rotation")] 
    public float maxTurnSpeed;
    public ForceMode turningForceMode;

    JointDriveController jdController;
    
    // This vector gives the position of the target relative to the position of the dog
	public Vector3 dirToTarget;
    // This float determines how much the dog will be rotating around the y axis
    float rotateBodyActionValue; 
    // Counts the number of steps until the next agent's decision will be made
    int decisionCounter;

    // [HideInInspector]
    public bool runningToItem;
    // [HideInInspector]
    public bool returningItem;

    public override void Initialize () {
        jdController = GetComponent<JointDriveController>();
        jdController.SetupBodyPart(body);
        jdController.SetupBodyPart(leg0_upper);
        jdController.SetupBodyPart(leg0_lower);
        jdController.SetupBodyPart(leg1_upper);
        jdController.SetupBodyPart(leg1_lower);
        jdController.SetupBodyPart(leg2_upper);
        jdController.SetupBodyPart(leg2_lower);
        jdController.SetupBodyPart(leg3_upper);
        jdController.SetupBodyPart(leg3_lower);

        //runningToItem = false;
    }

    public override void OnEpisodeBegin()
    {

        // Move the target to a new spot
        if(!runningToItem){
            stick.SetActive(true);
            mouseStick.SetActive(false);
            target = stick.transform;
            target.localPosition = new Vector3(Random.value * 16 - 8f,
                                            Random.value * 10 - 2f,
                                            1.13f);
            runningToItem = true;
        } else
        {
            stick.SetActive(false);
            mouseStick.SetActive(true);
            target = returnPoint;
            runningToItem = false;
        }
    }

    public void CollectObservationBodyPart(BodyPart bp, VectorSensor sensor)
    {
        var rb = bp.rb;
        sensor.AddObservation(bp.groundContact.touchingGround ? 1 : 0); // Is this bp touching the ground
        if(bp.rb.transform != body)
        {
            sensor.AddObservation(bp.currentXNormalizedRot);
            sensor.AddObservation(bp.currentYNormalizedRot);
            sensor.AddObservation(bp.currentZNormalizedRot);
            sensor.AddObservation(bp.currentStrength/jdController.maxJointForceLimit);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(dirToTarget.normalized);
        sensor.AddObservation(body.localPosition);
        sensor.AddObservation(jdController.bodyPartsDict[body].rb.velocity);
        sensor.AddObservation(jdController.bodyPartsDict[body].rb.angularVelocity);
        sensor.AddObservation(body.forward); //the capsule is rotated so this is local forward
        sensor.AddObservation(body.up); //the capsule is rotated so this is local forward
        foreach (var bodyPart in jdController.bodyPartsDict.Values)
        {
            CollectObservationBodyPart(bodyPart, sensor);
        }
    }

    void RotateBody(float act)
    {
        float speed = Mathf.Lerp(0, maxTurnSpeed, Mathf.Clamp(act, 0, 1));
        Vector3 rotDir = dirToTarget; 
        rotDir.y = 0;
        // Adds a force on the front of the body
        jdController.bodyPartsDict[body].rb.AddForceAtPosition(
            rotDir.normalized * speed * Time.deltaTime, body.forward, turningForceMode); 
        // Adds a force on the back od the body
        jdController.bodyPartsDict[body].rb.AddForceAtPosition(
            -rotDir.normalized * speed * Time.deltaTime, -body.forward, turningForceMode); 
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        var bpDict = jdController.bodyPartsDict;
      
        // Update joint drive target rotation
        bpDict[leg0_upper].SetJointTargetRotation(vectorAction[0], vectorAction[1], 0);
        bpDict[leg1_upper].SetJointTargetRotation(vectorAction[2], vectorAction[3], 0);
        bpDict[leg2_upper].SetJointTargetRotation(vectorAction[4], vectorAction[5], 0);
        bpDict[leg3_upper].SetJointTargetRotation(vectorAction[6], vectorAction[7], 0);
        bpDict[leg0_lower].SetJointTargetRotation(vectorAction[8], 0, 0);
        bpDict[leg1_lower].SetJointTargetRotation(vectorAction[9], 0, 0);
        bpDict[leg2_lower].SetJointTargetRotation(vectorAction[10], 0, 0);
        bpDict[leg3_lower].SetJointTargetRotation(vectorAction[11], 0, 0);

        // Update joint drive strength
        bpDict[leg0_upper].SetJointStrength(vectorAction[12]);
        bpDict[leg1_upper].SetJointStrength(vectorAction[13]);
        bpDict[leg2_upper].SetJointStrength(vectorAction[14]);
        bpDict[leg3_upper].SetJointStrength(vectorAction[15]);
        bpDict[leg0_lower].SetJointStrength(vectorAction[16]);
        bpDict[leg1_lower].SetJointStrength(vectorAction[17]);
        bpDict[leg2_lower].SetJointStrength(vectorAction[18]);
        bpDict[leg3_lower].SetJointStrength(vectorAction[19]);

        rotateBodyActionValue = vectorAction[20];

        UpdateDirToTarget();

        // Reached target
        if (dirToTarget.magnitude < 1.5f)
        {
            AddReward(1.0f);
            EndEpisode();
        } 
        
        RotateBody(rotateBodyActionValue);
        RotateHead();

        // Energy Conservation
        // The dog is penalized by how strongly it rotates towards the target.
        // Without this penalty the dog tries to rotate as fast as it can at all times.
        var bodyRotationPenalty = -0.001f * rotateBodyActionValue;
        AddReward(bodyRotationPenalty);

        // Reward for moving towards the target
        RewardFunctionMovingTowards();
        // Penalty for time
        RewardFunctionTimePenalty();
    }

    public void UpdateDirToTarget()
    {
        dirToTarget = target.position - jdController.bodyPartsDict[body].rb.position;

    }


    void RewardFunctionMovingTowards()
    {

		float movingTowardsDot = Vector3.Dot(
		    jdController.bodyPartsDict[body].rb.velocity, dirToTarget.normalized); 
        AddReward(0.01f * movingTowardsDot);
    }

    void RewardFunctionTimePenalty()
    {
        AddReward(- 0.001f);  //-0.001f chosen by experimentation.
    }

    void RotateHead()
    {
        float speed = 2.0f;
        Vector3 targetDirection = target.position - head.transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(head.transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(head.transform.position, newDirection, Color.red);
        head.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public override void Heuristic(float[] actionsOut)
    {
        for(var i=0;i<21;i++)
            actionsOut[i] = Random.value;
    }
}