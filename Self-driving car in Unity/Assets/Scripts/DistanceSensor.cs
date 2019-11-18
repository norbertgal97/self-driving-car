using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    public GameObject car;
    public float length = 10f;
    public float carVelocity = 0f;

    private GameObject otherCar = null;
    private GameObject crossing = null;
    private GameObject barrier = null;

    private void Start()
    {
        transform.localScale = new Vector3(3, 2, 1.5f);
    }

    private void Update()
    {
        bool carIsInBrakingDistance = false;
        bool crossingIsInBrakingDistance = false;
        bool barrierIsInBrakingDistance = false;

        if (otherCar != null)
        {
            carIsInBrakingDistance = car.GetComponent<Rigidbody>().velocity.magnitude > otherCar.GetComponent<Rigidbody>().velocity.magnitude;
        }

        if (crossing != null)
        {
            crossingIsInBrakingDistance = crossing.GetComponent<PedestrianCrossingController>().pedestrianCounter > 0;
        }

        barrierIsInBrakingDistance = barrier != null;

        car.GetComponent<CarController>().hasObstacle = carIsInBrakingDistance || crossingIsInBrakingDistance || barrierIsInBrakingDistance;

        CalculateSensorLength();
    }

    private void CalculateSensorLength()
    {
        carVelocity = car.GetComponent<Rigidbody>().velocity.magnitude;

        float normalized = carVelocity / car.GetComponent<CarController>().maxVelocity;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length * normalized + 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case Strings.car:
                otherCar = other.gameObject;
                break;
            case Strings.pedestrianCrossing:
                crossing = other.gameObject;
                break;
            case Strings.barrier:
                barrier = other.gameObject;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarController carController = car.GetComponent<CarController>();

        switch (other.gameObject.tag)
        {
            case Strings.car:
                otherCar = null;
                carController.hasObstacle = false;
                break;
            case Strings.pedestrianCrossing:
                crossing = null;
                carController.hasObstacle = false;
                break;
            case Strings.barrier:
                barrier = null;
                carController.hasObstacle = false;
                break;
            default:
                break;
        }
    }
}
