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
            if (car.GetComponent<Rigidbody>().velocity.magnitude > otherCar.GetComponent<Rigidbody>().velocity.magnitude)
            {
                carIsInBrakingDistance = true;
            }
            else
            {
                carIsInBrakingDistance = false;
            }
        }

        if (crossing != null)
        {
            if (crossing.GetComponent<PedestrianCrossingController>().pedestrianCounter > 0)
            {
                crossingIsInBrakingDistance = true;
            }
            else
            {
                crossingIsInBrakingDistance = false;
            }
        }

        if (barrier != null)
        {
            barrierIsInBrakingDistance = true;
        }

        CarController carController = car.GetComponent<CarController>();

        if (carIsInBrakingDistance || crossingIsInBrakingDistance || barrierIsInBrakingDistance)
        {
            carController.isBraking = true;
        }
        else
        {
            carController.isBraking = false;
        }

        CalculateSensorLength();
    }

    private void CalculateSensorLength()
    {
        carVelocity = car.GetComponent<Rigidbody>().velocity.magnitude;

        float normalized = carVelocity / car.GetComponent<CarController>().maxVelocity;

        transform.localScale = new Vector3(3, 2, length * normalized + 1.5f);
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
                carController.isBraking = false;
                break;
            case Strings.pedestrianCrossing:
                crossing = null;
                carController.isBraking = false;
                break;
            case Strings.barrier:
                barrier = null;
                carController.isBraking = false;
                break;
            default:
                break;
        }
    }
}
