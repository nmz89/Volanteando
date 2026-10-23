using UnityEngine;

public class WORLD_CurbSettings : MonoBehaviour
{
    void Awake()
    {
        bool collide = GAME_Settings.CurbCollisions;

        foreach (Collider2D c in GetComponentsInChildren<Collider2D>(true))
            c.enabled = collide;
    }
}