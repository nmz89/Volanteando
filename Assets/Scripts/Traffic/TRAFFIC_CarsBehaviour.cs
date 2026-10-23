using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TrafficCar : MonoBehaviour
{
    [Header("Look")]
    public SpriteRenderer body;

    [Header("Following")]
    public float minGap = 6f;    // Minimum distance between spawned cars.
    public float planningBrake = 30f;    // Start planning to brake if getting too close to car in front.
    public float acceleration = 8f;    // Maximum acceleration speed per second.
    public float braking = 100f;    // Maximum breaking speed per second.
    public float despawnMargin = 4f;
    public float reactionTime = 0.2f;
    public float safetyMargin = 1f;

    public float CurrentSpeed { get; private set; }
    public float HalfLength
    {
        get
        {
            Collider2D col = GetComponent<Collider2D>();
            return col != null ? col.bounds.extents.x : 1f;
        }
    }
    float cruiseSpeed;
    TrafficCar leader;
    Rigidbody2D rb;
    Camera cam;
    LogicScript logic;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null) logic = logicObj.GetComponent<LogicScript>();

        if (body != null)
        {
            body.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }

    void Update()
    {
        float leftEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        if (transform.position.x < leftEdge - despawnMargin)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        float target = cruiseSpeed;

        if (leader != null)
        {
            float gap = transform.position.x - leader.transform.position.x
                      - HalfLength - leader.HalfLength;

            float leaderSpeed = Mathf.Max(0f, leader.CurrentSpeed);

            // Account for the distance travelled while the follower reacts.
            float reactionDistance = Mathf.Max(0f, CurrentSpeed)
                                   * reactionTime;

            float availableGap = gap - minGap
                               - safetyMargin
                               - reactionDistance;

            // Assume the leader may brake at the same rate as this car.
            float leaderStoppingDistance =
                leaderSpeed * leaderSpeed / (2f * braking);

            // Maximum follower speed that permits stopping behind the leader.
            float safeSpeed = Mathf.Sqrt(
                Mathf.Max(
                    0f,
                    2f * braking *
                    (Mathf.Max(0f, availableGap) + leaderStoppingDistance)
                )
            );

            target = Mathf.Min(target, safeSpeed);
        }

        if (logic != null && logic.gameIsOver)
            target = 1f;

        float rate = target < CurrentSpeed ? braking : acceleration;

        CurrentSpeed = Mathf.MoveTowards(
            CurrentSpeed,
            target,
            rate * Time.fixedDeltaTime
        );

        rb.linearVelocity = Vector2.left * CurrentSpeed;
    }

    // Called by the spawner right after Instantiate
    public void Init(float speed, TrafficCar carAhead)
    {
        cruiseSpeed = speed;
        leader = carAhead;

        CurrentSpeed = leader != null
            ? Mathf.Min(speed, leader.CurrentSpeed)
            : speed;
    }

    void LateUpdate()
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null) return;

        Collider2D[] hits = new Collider2D[10];
        ContactFilter2D filter = ContactFilter2D.noFilter;

        int count = myCollider.Overlap(filter, hits);
        HashSet<TrafficCar> currentOverlaps = new HashSet<TrafficCar>();

        for (int i = 0; i < count; i++)
        {
            Collider2D hit = hits[i];

            if (hit == null || hit == myCollider) continue;

            TrafficCar other = hit.GetComponentInParent<TrafficCar>();
            if (other == null || other == this) continue;

            currentOverlaps.Add(other);
        }
    }
}