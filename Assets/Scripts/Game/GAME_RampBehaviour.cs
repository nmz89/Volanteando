using UnityEngine;

public class GAME_RampBehaviour : MonoBehaviour
{
    AUDIO_Manager audioManager;
    public float flightDistance = 12f;    // Air-travel distance

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AUDIO_Manager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PLAYER_AirborneCar air = other.GetComponentInParent<PLAYER_AirborneCar>();
        PLAYER_CarControl player = other.GetComponentInParent<PLAYER_CarControl>();

        if (air != null && player != null && player.carHasNotCrashed)
        {
            audioManager.PlaySFX(audioManager.rampJump);
            air.Launch(flightDistance / Mathf.Max(1f, player.playerSpeed));    // Higher speed means less air time. 
        }
    }
}