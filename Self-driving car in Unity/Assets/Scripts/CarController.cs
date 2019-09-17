using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float maxSteeringAngle = 31f;
    public float motorTorque = 250f;     
    public float brakeTorque = 30000;

    private Rigidbody rigidB;
    private float verticalInput;
    private float horizontalInput;
    [SerializeField]
    private float currentSteeringAngle;
    [SerializeField]
    private float velocity;

    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Brake();

        UpdateWheelTransform(frontLeftWheelCollider, frontLeftTransform);
        UpdateWheelTransform(frontRightWheelCollider, frontRightTransform);
        UpdateWheelTransform(rearLeftWheelCollider, rearLeftTransform);
        UpdateWheelTransform(rearRightWheelCollider, rearRightTransform);
    }

    private void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform transform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    private void Brake()
    {
        if (Input.GetAxis("Jump") == 1)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;

            frontLeftWheelCollider.brakeTorque = brakeTorque;
            frontRightWheelCollider.brakeTorque = brakeTorque;
            rearLeftWheelCollider.brakeTorque = brakeTorque;
            rearRightWheelCollider.brakeTorque = brakeTorque;

            rigidB.drag = 0.2f;
        }
    }

    private void Accelerate()
    {
        velocity = rigidB.velocity.magnitude;
        rigidB.drag = 0.0f;

        if (velocity > 14f)
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

            frontLeftWheelCollider.motorTorque = motorTorque * verticalInput;
            frontRightWheelCollider.motorTorque = motorTorque * verticalInput;
            rearRightWheelCollider.motorTorque = motorTorque * verticalInput;
            rearLeftWheelCollider.motorTorque = motorTorque * verticalInput;
        }
    }

    private void Steer()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }
}
