using UnityEngine;

public class UI_Speedometer : MonoBehaviour
{
    public PLAYER_CarControl player;
    public LogicScript logic;
    public RectTransform needle;

    [Header("Speed Indicator")]
    public float minAngle = 135f;   // Needle position at min speed
    public float maxAngle = -135f;   // Needle position at max speed

    public bool useDifficultyMax = true;   // Max needle value/position based on selected speed

    [Header("Motion Smoothing")]
    public float smoothing = 8f;

    float shown;   // Needle position, from 0 to 1

    void Update()
    {
        float max = (useDifficultyMax && logic != null) ? logic.maxSpeed : 0f;
        float speed = player.carHasNotCrashed ? player.playerSpeed : 0f;   // Back to 0 on crash

        float target = Mathf.Clamp01(speed / Mathf.Max(1f, max));
        shown = Mathf.Lerp(shown, target, 1f - Mathf.Exp(-smoothing * Time.unscaledDeltaTime));

        float angle = Mathf.Lerp(minAngle, maxAngle, shown);
        needle.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}