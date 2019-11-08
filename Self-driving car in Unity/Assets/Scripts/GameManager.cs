using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private List<Node> sources;
    [SerializeField]
    private List<Node> destinations;
    [SerializeField]
    private GameObject carPrefab;
    [SerializeField]
    private GameObject pedestrianPrefab;
    [SerializeField]
    private float spawnTime = 1f;
    [SerializeField]
    private int carCounter = 0;
    [SerializeField]
    private int pedestrianCounter = 0;
    [SerializeField]
    private int maxCarsOnScreen = 10;
    [SerializeField]
    private List<Transform> pedestrianSources;
    [SerializeField]
    private int maxPedestriansOnScreen = 10;

    private float currentSpawnTime = 0f;
    private float currentPedestrianSpawnTime = 0f;
    private Object previousSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentPedestrianSpawnTime += Time.deltaTime;

        if (currentSpawnTime > spawnTime && carCounter < maxCarsOnScreen)
        {
            SpawnCar();
            currentSpawnTime = 0f;
        }

        if (currentPedestrianSpawnTime > spawnTime && pedestrianCounter < maxPedestriansOnScreen)
        {
            SpawnPedestrian();
            currentPedestrianSpawnTime = 0f;
        }
    }

    private void SpawnCar()
    {
        carPrefab.SetActive(false);

        Node randomSource = sources[Random.Range(0, sources.Count)];
        Node randomDestination = destinations[Random.Range(0, destinations.Count)];

        if (randomSource.Equals(previousSource))
        {
            randomSource = sources[Random.Range(0, sources.Count - 1)];
        }

        GameObject car = Instantiate(carPrefab, randomSource.transform.position, Quaternion.identity);
        CarController carController = car.GetComponent<CarController>();

        carController.Source = randomSource;
        carController.Destination = randomDestination;
        carController.OnDestroy.AddListener(CarDestroyed);

        switch (randomSource.gameObject.tag)
        {
            case "Source South":
                break;
            case "Source West":
                car.transform.Rotate(0f, 90f, 0f);
                break;
            case "Source East":
                car.transform.Rotate(0f, -90f, 0f);
                break;
            case "Source North":
                car.transform.Rotate(0f, 180f, 0f);
                break;
            default:
                break;
        }

        carCounter++;
        previousSource = randomSource;
        car.SetActive(true);
    }

    private void SpawnPedestrian()
    {
        pedestrianPrefab.SetActive(false);

        Transform randomSource = pedestrianSources[Random.Range(0, pedestrianSources.Count)];

        GameObject pedestrian = Instantiate(pedestrianPrefab, randomSource.position, Quaternion.identity);

        PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
        pedestrianController.OnDestroy.AddListener(PedestrianDestroyed);

        pedestrian.transform.position = new Vector3(randomSource.transform.position.x, 1.1f, randomSource.transform.position.z);

        switch (randomSource.gameObject.tag)
        {          
            case "Source West":
                pedestrian.transform.Rotate(0f, 90f, 0f);
                break;
            case "Source East":
                pedestrian.transform.Rotate(0f, -90f, 0f);
                break;
            default:
                break;
        }

        pedestrianCounter++;
        pedestrian.SetActive(true);
    }

    public void CarDestroyed()
    {
        carCounter--;
    }

    public void PedestrianDestroyed()
    {
        pedestrianCounter--;
    }
}
