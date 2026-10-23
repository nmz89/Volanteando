using UnityEngine;

public class WORLD_RoadLoopCycle : MonoBehaviour
{
    public SpriteRenderer roadA;
    public SpriteRenderer roadB;
    public SpriteRenderer roadC;
    public Camera cam;

    void Start()
    {
        PlaceAfter(roadB, roadA);
        PlaceAfter(roadC, roadB);
    }

    void LateUpdate()
    {
        float cameraLeft = cam.ViewportToWorldPoint(
            new Vector3(0f, 0.5f, 0f)
        ).x;

        // Recycle the segment furthest to the left.
        for (int i = 0; i < 3; i++)
        {
            SpriteRenderer leftmost = roadA;
            SpriteRenderer rightmost = roadA;

            if (roadB.bounds.min.x < leftmost.bounds.min.x)
                leftmost = roadB;
            if (roadC.bounds.min.x < leftmost.bounds.min.x)
                leftmost = roadC;

            if (roadB.bounds.max.x > rightmost.bounds.max.x)
                rightmost = roadB;
            if (roadC.bounds.max.x > rightmost.bounds.max.x)
                rightmost = roadC;

            if (leftmost.bounds.max.x > cameraLeft)
                break;

            PlaceAfter(leftmost, rightmost);
        }
    }

    void PlaceAfter(SpriteRenderer target, SpriteRenderer previous)
    {
        Vector3 p = target.transform.position;
        p.x = previous.bounds.max.x + target.bounds.extents.x;
        target.transform.position = p;
    }
}