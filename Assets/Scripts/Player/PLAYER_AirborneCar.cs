using System.Collections;
using UnityEngine;

public class PLAYER_AirborneCar : MonoBehaviour
{
    [Header("Visual")]
    public Transform visual;    // Scales the player's car sprite.
    public float peakScale = 2.5f;    // Maximum size for the player's car sprite.

    [Header("Collision Avoidance")]
    public string[] layersToIgnore = { "Traffic", "Obstacles", "Background" };    // Layers to ignore while the player's car is in the air.

    public bool IsAirborne { get; private set; }
    Vector3 baseScale;
    Coroutine flightRoutine;

    void Awake()
    {
        if (visual == null) visual = transform;
        baseScale = visual.localScale;
    }

    public void Launch(float duration)
    {
        if (!IsAirborne) StartCoroutine(Flight(duration));
        flightRoutine = StartCoroutine(Flight(duration));
    }

    IEnumerator Flight(float duration)
    {
        IsAirborne = true;
        SetIgnore(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float k = Mathf.Sin(Mathf.Clamp01(elapsed / duration) * Mathf.PI);    // Original Scale -> Big Boy -> Original Scale
            visual.localScale = baseScale * Mathf.Lerp(1f, peakScale, k);

            yield return null;
        }

        visual.localScale = baseScale;
        SetIgnore(false);
        IsAirborne = false;
    }

    void SetIgnore(bool ignore)
    {
        int me = gameObject.layer;

        foreach (string name in layersToIgnore)
        {
            int other = LayerMask.NameToLayer(name);

            if (other >= 0)
                Physics2D.IgnoreLayerCollision(me, other, ignore);
        }
    }

    void OnDisable()
    {
        if (flightRoutine != null)
        {
            StopCoroutine(flightRoutine);
            flightRoutine = null;
        }

        if (IsAirborne)
            SetIgnore(false);

        IsAirborne = false;

        if (visual != null)
            visual.localScale = baseScale;
    }
}