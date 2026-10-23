using UnityEngine;

public class WORLD_RoadHazards : MonoBehaviour
{
    public GameObject hazardSpawner;
    void Awake()
    {
        if (hazardSpawner != null)
            hazardSpawner.SetActive(GAME_Settings.RoadHazards);
    }
}