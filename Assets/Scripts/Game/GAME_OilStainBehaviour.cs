using UnityEngine;

public class GAME_OilStainBehaviour : MonoBehaviour
{
    public float sidewaysForce = 12f;
    PLAYER_AirborneCar air;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PLAYER_CarControl player = other.GetComponentInParent<PLAYER_CarControl>();

        if (air != null && air.IsAirborne) return;

        if (player == null || !player.carHasNotCrashed)
            return;

        if (player.isSpinning)
            return;

        Rigidbody2D rb = player.carRigidBody2D;

        // Push the car sideways.
        rb.AddForce(
            Vector2.up * sidewaysForce,
            ForceMode2D.Impulse
        );

        // Start two full rotations.
        player.StartOilSpin(1f);

        Destroy(gameObject);
    }
}