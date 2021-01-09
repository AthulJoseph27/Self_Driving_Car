using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool[] checkPoints = new bool[56];
    public int currentCheckPoint = -1;
    public int noOfCollisions = 0;
    public int noOfCheckPointsPassed = 0;
    //private float horizontalInput, verticalInput;
    //private bool isBreaking, resetCarRotation, resetCar;
    private bool isRespawned = false;

    [SerializeField] private float maxTorque;
    [SerializeField] private float maxBreakingTorque;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRigthWheel;

    [SerializeField] private Transform car;
    [SerializeField] private Rigidbody _rigidbody;

    public void ResetCar()
    {

        car.position = new Vector3(0, 0, 0);
        car.rotation = new Quaternion(0, 0, 0, 0);
        _rigidbody.velocity = new Vector3(0,0,0) * 0f;
        isRespawned = true;
        _rigidbody.isKinematic = true;
    }

    public void Accelerate(float value)
    {
        frontRightWheelCollider.motorTorque = value * maxTorque;
        frontLeftWheelCollider.motorTorque = value * maxTorque;
    }

    public void Steer(float value)
    {
        float steerAngle = maxSteeringAngle * value;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;

    }

    // AI Controll
    private void Update()
    {
        if (isRespawned)
        {
            isRespawned = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = new Vector3(0, 0, 0) * 0f;
        }

        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheel);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheel);
        UpdateSingleWheel(rearRightWheelCollider, rearRigthWheel);
    }
    // For manual Contorl
    //private void FixedUpdate()
    //{
        
        // if (isRespawned)
        //{
        //    isRespawned = false;
        //    _rigidbody.isKinematic = false;
        //}
        

        

        //if (isRespawned)
        //{
        //    isRespawned = false;
        //    _rigidbody.isKinematic = false;
        //}
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        //isBreaking = Input.GetKey(KeyCode.Space);
        //resetCarRotation = Input.GetKey(KeyCode.B);
        //resetCar = Input.GetKey(KeyCode.R);

        //if (resetCarRotation)
        //{
        //    car.rotation = new Quaternion(0, 0, 0, 0);
        //    isRespawned = true;
        //    _rigidbody.isKinematic = true;
        //    return;
        //}

        //if (resetCar)
        //{
        //    ResetCar();
        //    return;
        //}

        //if (isBreaking)
        //{
        //    frontLeftWheelCollider.brakeTorque = maxBreakingTorque;
        //    frontRightWheelCollider.brakeTorque = maxBreakingTorque;
        //    rearLeftWheelCollider.brakeTorque = maxBreakingTorque;
        //    rearRightWheelCollider.brakeTorque = maxBreakingTorque;
        //}
        //else
        //{
        //    if (frontLeftWheelCollider.brakeTorque != 0)
        //    {
        //        frontLeftWheelCollider.brakeTorque = 0f;
        //        frontRightWheelCollider.brakeTorque = 0f;
        //        rearLeftWheelCollider.brakeTorque = 0f;
        //        rearRightWheelCollider.brakeTorque = 0f;
        //    }
        //    frontRightWheelCollider.motorTorque = verticalInput * maxTorque;
        //    frontLeftWheelCollider.motorTorque = verticalInput * maxTorque;

        //}

        //float steerAngle = maxSteeringAngle * horizontalInput;

        //frontLeftWheelCollider.steerAngle = steerAngle;
        //frontRightWheelCollider.steerAngle = steerAngle;

        //UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheel);
        //UpdateSingleWheel(frontRightWheelCollider, frontRightWheel);
        //UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheel);
        //UpdateSingleWheel(rearRightWheelCollider, rearRigthWheel);

    //}

    private void UpdateSingleWheel(WheelCollider collider, Transform wheel)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheel.position = pos;
        wheel.rotation = rot;
    }

    
}
