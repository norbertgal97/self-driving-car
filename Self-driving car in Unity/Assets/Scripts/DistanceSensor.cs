using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    public GameObject car;
    private GameObject obstacle = null;
    public float length = 10f;
    public float carVelocity = 0f;

    private void Start()
    {
        transform.localScale = new Vector3(3, 2, 2);
    }

    void Update()
    {
        if(obstacle != null)
        {
            if (car.GetComponent<Rigidbody>().velocity.magnitude > obstacle.GetComponent<Rigidbody>().velocity.magnitude)
            {
                car.GetComponent<CarController>().isBraking = true;              
            }
            else
            {
                car.GetComponent<CarController>().isBraking = false;          
            }
            car.GetComponent<CarController>().maxVelocity = obstacle.GetComponent<CarController>().maxVelocity;
        }

        CalculateSensorLength();
    }

    private void CalculateSensorLength()
    {
        carVelocity = car.GetComponent<Rigidbody>().velocity.magnitude;

        float normalized = carVelocity / car.GetComponent<CarController>().maxVelocity;

        transform.localScale = new Vector3(3, 2, length * normalized + 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car") { 
            obstacle = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            obstacle = null;
            car.GetComponent<CarController>().isBraking = false;
            car.GetComponent<CarController>().maxVelocity = 8.33f;
        } 
    }
}
