using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Persistent audio manager that keeps music continuous across scenes.
/// Supports seamless looping, crossfades, and short stingers.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Library")]
    [SerializeField] private AudioClip introTheme;
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] private AudioClip gameplayTheme;
    [SerializeField] private AudioClip highIntensityTheme;

    [Header("Stingers")]
    [SerializeField] private AudioClip levelCompleteStinger;
    [SerializeField] private AudioClip gameOverStinger;
    [SerializeField] private AudioClip specialEventStinger;

    [Header("Mix")]
    [SerializeField] private float defaultMusicVolume = 0.7f;
    [SerializeField] private float defaultSfxVolume = 0.8f;
    [SerializeField] private float crossfadeDuration = 0.75f;

    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource sfxSource;
    private bool usingA = true;
    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    private Coroutine crossfadeRoutine;
    private AudioClip currentClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureSources();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static AudioManager EnsureInstance()
    {
        if (Instance != null)
        {
            return Instance;
        }

        GameObject audioRoot = new GameObject("AudioManager");
        return audioRoot.AddComponent<AudioManager>();
    }

    private void EnsureSources()
    {
        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceB = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSourceA.loop = true;
        musicSourceB.loop = true;
        musicSourceA.playOnAwake = false;
        musicSourceB.playOnAwake = false;
        sfxSource.playOnAwake = false;

        musicSourceA.volume = defaultMusicVolume;
        musicSourceB.volume = 0f;
        sfxSource.volume = defaultSfxVolume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameConfig.Scenes.INTRO)
        {
            PlayMusicForState(introTheme);
            return;
        }

        if (scene.name == GameConfig.Scenes.MAIN_MENU)
        {
            PlayMusicForState(menuTheme != null ? menuTheme : gameplayTheme);
            return;
        }

        PlayMusicForState(gameplayTheme);
    }

    public void ApplySettings(bool enableMusic, bool enableSfx, float musicVolume, float sfxVolume)
    {
        musicEnabled = enableMusic;
        sfxEnabled = enableSfx;
        defaultMusicVolume = Mathf.Clamp01(musicVolume);
        defaultSfxVolume = Mathf.Clamp01(sfxVolume);

        musicSourceA.volume = musicEnabled ? defaultMusicVolume : 0f;
        musicSourceB.volume = musicEnabled ? defaultMusicVolume : 0f;
        sfxSource.volume = sfxEnabled ? defaultSfxVolume : 0f;

        if (!musicEnabled)
        {
            musicSourceA.Pause();
            musicSourceB.Pause();
        }
        else
        {
            if (musicSourceA.clip != null && !musicSourceA.isPlaying)
            {
                musicSourceA.UnPause();
            }

            if (musicSourceB.clip != null && !musicSourceB.isPlaying)
            {
                musicSourceB.UnPause();
            }
        }
    }

    public void PlayMusicForState(AudioClip clip)
    {
        if (clip == null || currentClip == clip)
        {
            return;
        }

        currentClip = clip;

        if (crossfadeRoutine != null)
        {
            StopCoroutine(crossfadeRoutine);
        }

        crossfadeRoutine = StartCoroutine(CrossfadeTo(clip));
    }

    private System.Collections.IEnumerator CrossfadeTo(AudioClip nextClip)
    {
        AudioSource from = usingA ? musicSourceA : musicSourceB;
        AudioSource to = usingA ? musicSourceB : musicSourceA;

        to.clip = nextClip;
        to.time = 0f;
        to.loop = true;
        to.volume = 0f;
        to.Play();

        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, crossfadeDuration);
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float target = musicEnabled ? defaultMusicVolume : 0f;
            from.volume = Mathf.Lerp(target, 0f, t);
            to.volume = Mathf.Lerp(0f, target, t);
            yield return null;
        }

        from.Stop();
        from.volume = musicEnabled ? defaultMusicVolume : 0f;
        to.volume = musicEnabled ? defaultMusicVolume : 0f;
        usingA = !usingA;
        crossfadeRoutine = null;
    }

    public void PlayGameOverStinger()
    {
        PlayStinger(gameOverStinger);
    }

    public void PlayLevelCompleteStinger()
    {
        PlayStinger(levelCompleteStinger);
    }

    public void PlaySpecialEventStinger()
    {
        PlayStinger(specialEventStinger);
    }

    private void PlayStinger(AudioClip clip)
    {
        if (!sfxEnabled || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, defaultSfxVolume);
    }

    /// <summary>
    /// Optional API for dynamic intensity layering.
    /// </summary>
    public void SetHighIntensityMusic(bool highIntensity)
    {
        if (highIntensity && highIntensityTheme != null)
        {
            PlayMusicForState(highIntensityTheme);
        }
        else if (SceneManager.GetActiveScene().name == GameConfig.Scenes.MAIN_MENU)
        {
            PlayMusicForState(menuTheme != null ? menuTheme : gameplayTheme);
        }
        else
        {
            PlayMusicForState(gameplayTheme);
        }
    }
}
