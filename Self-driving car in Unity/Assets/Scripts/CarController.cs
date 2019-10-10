using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSteeringAngle = 38f;
    public float motorTorque = 200f;
    public float brakeTorque = 1000f;

    public Node Destination { get; set; }
    public Node Source { get; set; }

    public bool isBraking;

    [SerializeField]
    private WheelCollider frontLeftWheelCollider;
    [SerializeField]
    private WheelCollider frontRightWheelCollider;
    [SerializeField]
    private WheelCollider rearLeftWheelCollider;
    [SerializeField]
    private WheelCollider rearRightWheelCollider;

    [SerializeField]
    private Transform frontLeftTransform;
    [SerializeField]
    private Transform frontRightTransform;
    [SerializeField]
    private Transform rearLeftTransform;
    [SerializeField]
    private Transform rearRightTransform;

    private Transform sensor;
    private List<Node> shortestPath;
    private Rigidbody rigidB;
    private float velocity;
    private int currentIndex = 0;
    private float currentSteeringAngle;

    private void Start()
    {
        rigidB = GetComponent<Rigidbody>();  
        sensor = transform.Find("Distance Sensor");
        shortestPath = Dijkstra.Instance.CalculateShortestPath(Graph.Instance, Source, Destination);
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        Brake();

        UpdateWheelTransform(frontLeftWheelCollider, frontLeftTransform);
        UpdateWheelTransform(frontRightWheelCollider, frontRightTransform);
        UpdateWheelTransform(rearLeftWheelCollider, rearLeftTransform);
        UpdateWheelTransform(rearRightWheelCollider, rearRightTransform);

        Sensor();
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform transform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    private void Brake()
    {
        if (Input.GetAxis("Jump") == 1 || isBraking)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;

            frontLeftWheelCollider.brakeTorque = brakeTorque;
            frontRightWheelCollider.brakeTorque = brakeTorque;
            rearLeftWheelCollider.brakeTorque = brakeTorque;
            rearRightWheelCollider.brakeTorque = brakeTorque;

            rigidB.drag = 0.3f;
        }
    }

    private void Accelerate()
    {
        velocity = rigidB.velocity.magnitude;
        rigidB.drag = 0.0f;

        if (velocity > 5f)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque = 0;
            rearLeftWheelCollider.brakeTorque = 0;
            rearRightWheelCollider.brakeTorque = 0;

            frontLeftWheelCollider.motorTorque = motorTorque;
            frontRightWheelCollider.motorTorque = motorTorque;
            rearRightWheelCollider.motorTorque = motorTorque;
            rearLeftWheelCollider.motorTorque = motorTorque;
        }
    }

    private void Steer()
    {
        if (shortestPath[currentIndex].transform.position != Destination.transform.position && Vector3.Distance(transform.position, shortestPath[currentIndex].transform.position) < 2f)
        {
            currentIndex++;
        }

        Vector3 localVector = transform.InverseTransformPoint(shortestPath[currentIndex].transform.position);
        localVector = localVector.normalized;

        currentSteeringAngle = maxSteeringAngle * localVector.x;
        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }

    private void Sensor()
    {
        sensor.localRotation = Quaternion.Euler(new Vector3(0f, currentSteeringAngle, 0f));
    }
}
