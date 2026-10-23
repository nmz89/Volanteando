using UnityEngine;

public class VFX_CarSpriteColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bodyRenderer;

    private Color32[] colors =
    {
    new Color32(0,   120, 0,   255), // #007800
    new Color32(0,   64,  88,  255), // #004058
    new Color32(136, 20,  0,   255), // #881400
    new Color32(248, 88,  152, 255), // #F85898
    new Color32(164, 228, 252, 255), // #A4E4FC
    new Color32(172, 124, 0,   255), // #AC7C00
    new Color32(148, 0,   132, 255), // #940084
    new Color32(72,  92,  196, 255), // #485CC4
    new Color32(0,   164, 144, 255), // #00A490
    new Color32(244, 152, 48,  255), // #F49830
    new Color32(188, 196, 208, 255), // #BCC4D0
    new Color32(116, 168, 56,  255), // #74A838
    new Color32(196, 100, 76,  255), // #C4644C
    new Color32(104, 52,  148, 255), // #683494
    };

    void Start()
    {
        if (colors.Length > 0 && bodyRenderer != null)
        {
            bodyRenderer.color =
                colors[Random.Range(0, colors.Length)];
        }
    }
}