using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]

public class PLAYER_TireMarks : MonoBehaviour
{
    public PLAYER_AirborneCar air;
    TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        trail.emitting = air == null || !air.IsAirborne;
    }
}