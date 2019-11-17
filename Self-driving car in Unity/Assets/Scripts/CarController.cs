using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    public float speedlimit
    {
        get
        {
            return _speedlimit;
        }

        set
        {
            if (value > maxVelocity)
            {
                _speedlimit = maxVelocity;
            }
            else
            {
                _speedlimit = value;
            }
        }
    }

    public float maxVelocity = 4.33f;
    public UnityEvent OnDestroy;
    public Node destination = null;
    public Node source = null;
    public bool isDecelerating = false;
    public bool isParking = false;
    public bool isBraking = false;

    [SerializeField]
    private float maxSteeringAngle = 38f;
    [SerializeField]
    private float motorTorque = 200f;
    [SerializeField]
    private float brakeTorque = 1000f;
    [SerializeField]
    private float velocity;

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
    private int currentIndex = 0;
    private float currentSteeringAngle;
    private float _speedlimit = 4.33f;

    private void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        sensor = transform.Find(Strings.distanceSensor);
        shortestPath = Dijkstra.Instance.CalculateShortestPath(Graph.Instance, source, destination);
        speedlimit = maxVelocity;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        Brake();

        if (velocity < speedlimit)
        {
            isDecelerating = false;
        }

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
        if (isBraking || isDecelerating || isParking)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;

            frontLeftWheelCollider.brakeTorque = brakeTorque;
            frontRightWheelCollider.brakeTorque = brakeTorque;
            rearLeftWheelCollider.brakeTorque = brakeTorque;
            rearRightWheelCollider.brakeTorque = brakeTorque;

            rigidB.drag = 0.4f;
        }
    }

    private void Accelerate()
    {
        velocity = rigidB.velocity.magnitude;
        rigidB.drag = 0.0f;

        if (velocity > speedlimit || isBraking || isDecelerating || isParking)
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
        if (shortestPath[currentIndex].transform.position != destination.transform.position && Vector3.Distance(transform.position, shortestPath[currentIndex].transform.position) < 2f)
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

    public void Destroy()
    {
        Destroy(gameObject);
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
    }
}
