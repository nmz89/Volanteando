using UnityEngine;

public class TRAFFIC_HazardSpawning : MonoBehaviour
{
    [Header("Available Obstacles")]
    public GameObject[] obstaclePrefabs;

    [Header("World units Obstacle Spacing")]
    public Vector2 gapRange = new Vector2(30f, 70f);
    public float spawnMargin = 3f;
    public float yJitter = 0f;

    Camera cam;
    LogicScript logic;
    float nextSpawnX;

    void Start()
    {
        cam = Camera.main;

        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");

        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();

        if (cam == null || obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            enabled = false;
            return;
        }
        gapRange *= GAME_Settings.CurrentDifficulty.hazardGapMultiplier;

        nextSpawnX = cam.transform.position.x
                    + cam.orthographicSize * cam.aspect
                    + spawnMargin
                    + Random.Range(gapRange.x, gapRange.y);
    }

    void Update()
    {
        if (logic != null && logic.gameIsOver)
            return;

        float rightEdge = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        while (rightEdge + spawnMargin >= nextSpawnX)
        {
            SpawnObstacle(nextSpawnX);
            nextSpawnX += Random.Range(gapRange.x, gapRange.y);
        }
    }

    void SpawnObstacle(float x)
    {
        float y = transform.position.y + Random.Range(-yJitter, yJitter);
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(
            prefab,
            new Vector3(x, y, 0f),
            prefab.transform.rotation
        );
    }
}