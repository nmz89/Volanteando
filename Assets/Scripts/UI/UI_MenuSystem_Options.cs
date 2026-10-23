using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_MenuSystem_Options : MonoBehaviour
{
    [Header("Difficulty (E, N, H, I)")]
    public Toggle[] difficultyToggles;

    [Header("Volume Control")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public UnityEvent onClose;

    [Header("Curbs")]
    public Toggle curbToggle;
    public Toggle hazardsToggle;

    void Awake()
    {
        if (curbToggle != null)
            curbToggle.onValueChanged.AddListener(on => GAME_Settings.CurbCollisions = on);

        if (hazardsToggle != null)
            hazardsToggle.onValueChanged.AddListener(on => GAME_Settings.RoadHazards = on);

        for (int i = 0; i < difficultyToggles.Length; i++)
        {
            int index = i;   // copy for the lambda
            difficultyToggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn) SetDifficulty(index);
            });
        }

        musicSlider.onValueChanged.AddListener(v => { GAME_Settings.MusicVolume = v; RefreshAudio(); });
        sfxSlider.onValueChanged.AddListener(v => { GAME_Settings.SfxVolume = v; RefreshAudio(); });
    }

    void OnEnable()
    {
        if (curbToggle != null)
            curbToggle.SetIsOnWithoutNotify(GAME_Settings.CurbCollisions);

        if (hazardsToggle != null)
            hazardsToggle.SetIsOnWithoutNotify(GAME_Settings.RoadHazards);

        difficultyToggles[GAME_Settings.DifficultyIndex].SetIsOnWithoutNotify(true);
        musicSlider.SetValueWithoutNotify(GAME_Settings.MusicVolume);
        sfxSlider.SetValueWithoutNotify(GAME_Settings.SfxVolume);
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    void SetDifficulty(int index)
    {
        GAME_Settings.DifficultyIndex = index;
    }

    void RefreshAudio()
    {
        AUDIO_Manager am = FindAnyObjectByType<AUDIO_Manager>();
        if (am != null) am.ApplyVolumes();
    }
    public void Close()
    {
        PlayerPrefs.Save();
        onClose.Invoke();
        gameObject.SetActive(false);
    }
}