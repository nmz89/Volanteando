using UnityEngine;

public class AUDIO_Manager : MonoBehaviour
{
    [Header("Audio Source")]

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource engineSource;

    [Header("Audio Clips")]
    public AudioClip crash;
    public AudioClip engine;
    public AudioClip spacePress;
    public AudioClip rampJump;

    float sfxBase, engineBase;

    private void Awake()
    {
        sfxBase = sfxSource.volume;
        engineBase = engineSource.volume;
        ApplyVolumes();
    }

    private void Start()
    {
        StartEngine();
    }

    public void ApplyVolumes()
    {
        sfxSource.volume = sfxBase * GAME_Settings.SfxVolume;
        engineSource.volume = engineBase * GAME_Settings.SfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StartEngine()
    {
        engineSource.clip = engine;
        engineSource.loop = true;
        engineSource.Play();
    }

    public void SetEnginePitch(float pitch)
    {
        engineSource.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
    }

    public void StopEngine()
    {
        engineSource.Stop();
    }
}