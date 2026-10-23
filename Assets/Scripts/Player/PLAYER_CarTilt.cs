using UnityEngine;

public class PLAYER_CarTilt : MonoBehaviour
{
    PLAYER_AirborneCar air;

    public Rigidbody2D rb;
    float baseAngle;
    float tilt;

    [Header("Tilt limits (degrees relative to the car's neutral rotation)")]
    public float noseUpAngle = 10f;
    public float noseDownAngle = -30f;

    [Header("Rotation speed (degrees per second)")]
    public float fallRate = 40f;
    public float riseRate = 180f;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        baseAngle = transform.eulerAngles.z;

        air = GetComponent<PLAYER_AirborneCar>();
    }

    void Update()
    {
        bool airborne = air != null && air.IsAirborne;
        bool rising = airborne || rb.linearVelocity.y > 0.1f;

        float target = rising ? noseUpAngle : noseDownAngle;
        float rate = rising ? riseRate : fallRate;

        tilt = Mathf.MoveTowards(tilt, target, rate * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, baseAngle + tilt);
    }
}