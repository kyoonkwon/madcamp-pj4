using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgentsExamples;

public class GameDogAgent : Agent
{
    // The target the dog will run towards.
    public Transform target; 
    public Transform head;
    public Transform returnPoint;
    public ThrowStick throwcontroller;
    public GameObject stick;
    public GameObject mouseStick;

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
    public bool waitingThrow;
    public Transform item;
    public Rigidbody itemRB;
    public Collider itemCol;
    public bool runningItem;
    public AudioSource barksound;


    public override void Initialize () {
        Debug.Log("Initialize");
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

        // throwcontroller = GetComponent<ThrowStick>();
        Debug.Log(throwcontroller);
        throwcontroller.item.gameObject.SetActive(true);
        throwcontroller.canThrow = true;
        item = stick.transform;
        item.localPosition = returnPoint.localPosition - new Vector3(0f, 1.0f, -1.0f);
        itemRB = item.GetComponent<Rigidbody>();
        itemCol = item.GetComponent<Collider>();
        target = returnPoint;
        waitingThrow = true;
    }

    public override void OnEpisodeBegin()
    {
        if(throwcontroller.throwDone){
            waitingThrow = false;
            if(target == item){
                runningToItem = false;
            }
            else
                runningToItem = true;
        }
        // Move the target to a new spot
        if(waitingThrow){
            throwcontroller.canThrow = true;
            stick.SetActive(true);
            mouseStick.SetActive(false);
            //item.localPosition = returnPoint.localPosition - new Vector3(0f, 2.5f, -1.0f);
            target = returnPoint;
            runningToItem = false;
            Debug.Log("기다리는 중");
        }
        else if(!waitingThrow && runningToItem){
            target = item;
            throwcontroller.canThrow = false;
            stick.SetActive(true);
            mouseStick.SetActive(false);
            Debug.Log("타겟을 향해 달려가는 중");
            Debug.Log(target.localPosition);
            barksound.Play();
        } else if(!waitingThrow && !runningToItem)
        {
            //after picking up and returning
            target = returnPoint;
            throwcontroller.canThrow = false;
            stick.SetActive(false);
            mouseStick.SetActive(true);
            Debug.Log("막대를 주워 돌아오는 중");
            Debug.Log(target.localPosition);
            waitingThrow = true;
            throwcontroller.throwDone = false;
            item.localPosition = returnPoint.localPosition - new Vector3(0f, 1.0f, -1.0f);
        }    
    }
    /*
    void Update(){
        if (throwcontroller.throwDone){
            PickUpItem();

            while(dirToTarget.sqrMagnitude > 1f) //wait until we are close
            {
                //yield return null;
            }

            Return();

            while(dirToTarget.sqrMagnitude > 1f) //wait until we are close
            {
                //yield return null;
            }

            DropItem();

            throwcontroller.throwDone = false;
        }
    }*/

    void PickUpItem(){
        Debug.Log("PickUpItem");
        waitingThrow = false;
        runningItem = true;
        target = item;
        itemCol.enabled = false; //Disable the collider on the stick.
		itemRB.isKinematic = true; //Turn off physics
		item.position = mouthPosition.position; //Set stick position
		item.rotation = mouthPosition.rotation; //Set stick rotation
		item.SetParent(mouthPosition); //Parent the stick to the dog's mouth
        UpdateDirToTarget(); //update the direction vector

    }

    void Return(){
        Debug.Log("Return");
        waitingThrow = false;
        runningItem = false;
        target = returnPoint;
    }

    void DropItem(){
        Debug.Log("DropItem");
        itemRB.isKinematic = false; //Enable physics on the stick 
		item.parent = null; //Stick no longer parented to the dog
		itemCol.enabled = true; //Re-enable the collider on the stick
        waitingThrow = true;
        runningItem = false;
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
        Debug.Log("CollecObservations");
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
        Debug.Log("OnActionRecieved");
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
