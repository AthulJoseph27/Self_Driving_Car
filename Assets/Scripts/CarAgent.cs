using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
   
    private CarController carController;
    private float maxTimeBetweenCheckPoints = 100f;
    [SerializeField] private List<CheckPoint> checkPoints;
    [SerializeField] private List<TrackWall> walls;
    private float timeLeft = 50f;
    //private float Reward = 0.0f;
    //private float previousDistance = 63f;
    private int localCollisionCount = 0;
    private int localCountOfPassedCheckPoints = 0;
    //private float previousDistance = 15f;

    private void Update()
    {
        //Debug.Log(Reward);
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            //AddReward(-(28.0f - carController.noOfCheckPointsPassed));
            AddReward(-1f);
            timeLeft = maxTimeBetweenCheckPoints;
            EndEpisode();
        }
        //Debug.Log(Award);
        if (carController.noOfCheckPointsPassed > 0)
        {
            //Debug.Log(carController.currentCheckPoint);
            checkPoints[carController.currentCheckPoint].gameObject.SetActive(false);
            localCountOfPassedCheckPoints += 1;
            AddReward(0.0025f * (float)localCountOfPassedCheckPoints);
            //Reward+= (0.25f * (float)localCountOfPassedCheckPoints);
            //AddReward(0.65f * Mathf.Pow((carController.noOfCheckPointsPassed+localCountOfPassedCheckPoints),1.5f)+ 0.1f * timeLeft/maxTimeBetweenCheckPoints);
            //Award += 0.65f * (float)(carController.noOfCheckPointsPassed + localCountOfPassedCheckPoints);
            timeLeft = maxTimeBetweenCheckPoints;
            carController.noOfCheckPointsPassed = 0;
            //previousDistance = 63f;
            //Debug.Log("Passed checkPoint , + 0.65f");
            //Debug.Log("Next Checkpoint"+(carController.currentCheckPoint+1));
        }

        if(carController.currentCheckPoint >= 36)
        {
            AddReward(0.015f * localCountOfPassedCheckPoints);
        }
        if(carController.currentCheckPoint == checkPoints.Count - 1)
        {
            AddReward(30f + timeLeft);
            //Reward += (30000f + Mathf.Max(timeLeft, 0f) * 300);
            //Award += (3f + Mathf.Max(timeLeft, 0f) * 3);
            //Debug.Log("Goal Reached");
            EndEpisode();
        }

        if (carController.noOfCollisions > 0)
        {
            localCollisionCount++;
            //Debug.Log("Collision");
            AddReward(-(5f + localCollisionCount));
            //Reward += (-0.4f);
            //Award += (-0.1f);
            carController.noOfCollisions = 0;
        }


        AddReward(-0.01f);
        //Reward += (-0.0001f);
    }

    public override void Initialize()
    {
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        carController.ResetCar();
        //Debug.Log(carController.checkPoints.Length);
        for (int i = 0; i < checkPoints.Count; i++)
        {
            carController.checkPoints[i] = false;
            checkPoints[i].gameObject.SetActive(true);
        }
            
        carController.currentCheckPoint = -1;
        carController.noOfCollisions = 0;
        carController.noOfCheckPointsPassed = 0;
        timeLeft = maxTimeBetweenCheckPoints;

    }

    public override void CollectObservations(VectorSensor sensor)
    {

        Vector3 distanceToNextCheckPoint;
        Vector3 distanceToNearByWall;

        if (carController.currentCheckPoint < checkPoints.Count-1)
        {
            distanceToNextCheckPoint = checkPoints[carController.currentCheckPoint + 1].transform.position - carController.transform.position;
            distanceToNearByWall = new Vector3(100, 100, 100);
            for (int i = 0; i < walls.Count; i++)
            {
                var temp = walls[i].transform.position - carController.transform.position;
                if (Mathf.Abs(temp.magnitude) < Mathf.Abs(distanceToNearByWall.magnitude))
                {
                    distanceToNearByWall = temp;
                }
            }

        }
        else
        {
            distanceToNextCheckPoint = new Vector3(0, 0, 0);
            distanceToNearByWall = new Vector3(10, 10, 10);
        }
        //Debug.Log(distanceToNearByWall.magnitude);
        //if (Mathf.Abs(distanceToNextCheckPoint.magnitude) < previousDistance)
        //{
        //    //Debug.Log("Positive value awarded");
        //    AddReward(0.00012f);
        //    previousDistance = Mathf.Abs(distanceToNextCheckPoint.magnitude);
        //}
        //else
        //{
        //    //Debug.Log("Value Deducted");
        //    //Award += (-0.002f);
        //    AddReward(-(2f+distanceToNextCheckPoint.magnitude * 10f));
        //}
        //Debug.Log(distanceToNearByWall.magnitude);
        if (distanceToNearByWall.magnitude < 3.3f)
        {
            AddReward(- (distanceToNearByWall.magnitude/100f));
            //Reward+= (-(distanceToNearByWall.magnitude/100f));
        }

        sensor.AddObservation(distanceToNextCheckPoint / 70f);
        //sensor.AddObservation(distanceToNearByWall / 10f);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        carController.Steer(vectorAction[0]);
        carController.Accelerate(vectorAction[1]);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

}
