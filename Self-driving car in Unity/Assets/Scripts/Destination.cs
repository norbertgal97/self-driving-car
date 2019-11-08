using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Car":
                other.GetComponent<CarController>().Destroy();
                break;
            case "Pedestrian":
                other.GetComponent<PedestrianController>().Destroy();
                break;
            default:
                break;
        }
    }
}
