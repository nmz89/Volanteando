using UnityEngine;
public class AUDIO_MenuMusic : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;
    private static AUDIO_MenuMusic instance;
    float musicBase;

    private void Awake()
    {
        musicBase = musicSource.volume;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    void Update()
    {
        ApplyVolumes();
    }
    public void ApplyVolumes()
    {
        musicSource.volume = musicBase * GAME_Settings.MusicVolume;
    }
}