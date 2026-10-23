using UnityEngine;

public class TRAFFIC_LaneSpawning : MonoBehaviour
{
    [Header("Car Prefabs")]
    public TrafficCar[] carPrefabs;

    [Header("Spacing")]
    public Vector2 gapRange = new Vector2(15f, 25f);
    public Vector2 speedRange = new Vector2(3f, 7f);
    public float yJitter = 0f;
    public float spawnMargin = 3f;

    Camera cam;
    LogicScript logic;
    TrafficCar last;

    float nextSpawnX;

    void Start()
    {
        float m = GAME_Settings.CurrentDifficulty.trafficSpeedMultiplier;
        speedRange *= m;

        cam = Camera.main;

        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();

        nextSpawnX = cam.transform.position.x
                   + cam.orthographicSize * cam.aspect
                   + spawnMargin;
    }

    void Update()
    {
        if (logic != null && logic.gameIsOver)
            return;

        float rightEdge = cam.transform.position.x
                        + cam.orthographicSize * cam.aspect;

        while (rightEdge + spawnMargin >= nextSpawnX)
        {
            Spawn(nextSpawnX);
            nextSpawnX += Random.Range(gapRange.x, gapRange.y);
        }
    }

    void Spawn(float x)
    {
        if (carPrefabs == null || carPrefabs.Length == 0)
            return;

        float y = transform.position.y
                + Random.Range(-yJitter, yJitter);

        // Randomly select one of the car prefabs.
        TrafficCar selectedPrefab =
            carPrefabs[Random.Range(0, carPrefabs.Length)];

        TrafficCar car = Instantiate(
            selectedPrefab,
            new Vector3(x, y, 0f),
            Quaternion.identity
        );

        car.Init(Random.Range(speedRange.x, speedRange.y), last);
        last = car;
    }
}