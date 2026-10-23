using UnityEngine;
using UnityEngine.UI;

public class UI_LanguageSelection : MonoBehaviour
{
    [Header("Buttons")]
    public Button englishButton;
    public Button spanishButton;

    [Header("English Sprites")]
    public Sprite englishPressed;
    public Sprite englishUnpressed;

    [Header("Spanish Sprites")]
    public Sprite spanishPressed;
    public Sprite spanishUnpressed;

    [Header("Main Menu")]
    public UI_MenuSystem_MainMenu mainMenu;

    private bool isSpanishSelected;

    void Awake()
    {
        englishButton.transition = Selectable.Transition.None;
        spanishButton.transition = Selectable.Transition.None;
    }

    void Start()
    {
        // Match the buttons to the currently active language.
        isSpanishSelected = UI_Language.Current == 0;
        UpdateButtons();
    }

    public void SelectEnglish()
    {
        isSpanishSelected = false;
        UpdateButtons();

        // Change the actual game language.
        mainMenu.SetLanguage(1);
    }

    public void SelectSpanish()
    {
        isSpanishSelected = true;
        UpdateButtons();

        // Change the actual game language.
        mainMenu.SetLanguage(0);
    }

    void UpdateButtons()
    {
        englishButton.image.sprite = isSpanishSelected
            ? englishUnpressed
            : englishPressed;

        spanishButton.image.sprite = isSpanishSelected
            ? spanishPressed
            : spanishUnpressed;
    }
}