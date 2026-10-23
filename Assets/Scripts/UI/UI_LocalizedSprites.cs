using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_LocalizedSprites : MonoBehaviour
{
    [Header("Spanish")]
    public Sprite spanish;
    public Sprite spanishPressed;

    [Header("English")]
    public Sprite english;
    public Sprite englishPressed;

    private UI_Buttons buttons;

    void Awake()
    {
        buttons = GetComponent<UI_Buttons>();
    }

    void OnEnable()
    {
        UI_Language.Changed += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        UI_Language.Changed -= Refresh;
    }

    void Refresh()
    {
        if (buttons == null)
            return;

        if (UI_Language.Current == 0)
            buttons.SetSprites(spanish, spanishPressed);
        else
            buttons.SetSprites(english, englishPressed);
    }
}