  using UnityEngine;

  public class WORLD_ObstacleDespawning : MonoBehaviour
  {
      public float margin = 6f;

      void Update()
      {
          Camera cam = Camera.main;
          if (transform.position.x < cam.transform.position.x - cam.orthographicSize * cam.aspect - margin)
              Destroy(gameObject);
      }
  }