using UnityEngine;

public class CAMERA_PlayerTracking : MonoBehaviour
{
    public Transform player;
    private PLAYER_CarControl playerControl;

    void Start()
    {
        playerControl = player.GetComponent<PLAYER_CarControl>();
    }

    void Update()
    {
        if (playerControl == null || !playerControl.carHasNotCrashed)
            return;    // Stop following when the car crashes or goes off-screen.

        transform.position = new Vector3(player.position.x + 10, 0, -10);
    }
}