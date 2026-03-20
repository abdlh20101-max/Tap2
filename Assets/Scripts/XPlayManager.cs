using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// X-Play: مدير اللعبة الرئيسي
/// يدير: العملات (Coins)، الطاقة (Energy)، الإعدادات، والانتقال بين المشاهد، ونظام التقدم (XP/Level).
/// Developed by: Abdullah Al-husini
/// </summary>
public class XPlayManager : MonoBehaviour
{
    public static XPlayManager Instance { get; private set; }

    [Header("Player Data")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int energy = 100;
    [SerializeField] private int maxEnergy = 100;

    [Header("Player Progression")]
    [SerializeField] private int xp = 0;
    [SerializeField] private int level = 1;
    [SerializeField] private int xpToNextLevel = 100; // XP required for the next level
    [SerializeField] private float energyRechargeRate = 1f; // طاقة واحدة كل ثانية

    [Header("Audio Settings")]
    [SerializeField] private bool musicEnabled = true;
    [SerializeField] private bool soundEffectsEnabled = true;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 0.8f;

    [Header("Game Settings")]
    [SerializeField] private float deltaTime = 0f;

    private AudioSource backgroundMusic;
    private float energyRechargeTimer = 0f;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // تحميل البيانات المحفوظة
        LoadPlayerData();
        // تحديث XP المطلوب للمستوى الحالي عند التحميل
        CalculateXPToNextLevel();
        AudioManager.EnsureInstance();
    }

    private void Start()
    {
        // البحث عن AudioSource للموسيقى الخلفية
        backgroundMusic = GetComponent<AudioSource>();
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = musicVolume;
            backgroundMusic.loop = true;
            if (musicEnabled)
                backgroundMusic.Play();
        }

        AudioManager.Instance?.ApplySettings(musicEnabled, soundEffectsEnabled, musicVolume, sfxVolume);
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        // إعادة شحن الطاقة تدريجياً
        if (energy < maxEnergy)
        {
            energyRechargeTimer += Time.deltaTime;
            if (energyRechargeTimer >= 1f / energyRechargeRate)
            {
                energy = Mathf.Min(energy + 1, maxEnergy);
                energyRechargeTimer = 0f;
            }
        }
    }

    #region Coins Management
    /// <summary>
    /// إضافة عملات للاعب
    /// </summary>
    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative coins!");
            return;
        }
        coins += amount;
        SavePlayerData();
    }

    /// <summary>
    /// خصم عملات من اللاعب
    /// </summary>
    public bool SpendCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot spend negative coins!");
            return false;
        }

        if (coins >= amount)
        {
            coins -= amount;
            SavePlayerData();
            return true;
        }

        Debug.LogWarning($"Not enough coins! Required: {amount}, Available: {coins}");
        return false;
    }

    public int GetCoins() => coins;
    #endregion

    #region Energy Management
    /// <summary>
    /// استهلاك الطاقة
    /// </summary>
    public bool ConsumeEnergy(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot consume negative energy!");
            return false;
        }

        if (energy >= amount)
        {
            energy -= amount;
            SavePlayerData();
            return true;
        }

        Debug.LogWarning($"Not enough energy! Required: {amount}, Available: {energy}");
        return false;
    }

    /// <summary>
    /// استرجاع الطاقة
    /// </summary>
    public void RestoreEnergy(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot restore negative energy!");
            return;
        }
        energy = Mathf.Min(energy + amount, maxEnergy);
        SavePlayerData();
    }

    public int GetEnergy() => energy;
    public int GetMaxEnergy() => maxEnergy;
    #endregion

    #region Player Progression Management
    /// <summary>
    /// إضافة نقاط الخبرة (XP) للاعب
    /// </summary>
    public void AddXP(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative XP!");
            return;
        }
        xp += amount;
        Debug.Log($"Added {amount} XP. Current XP: {xp}");
        CheckForLevelUp();
        SavePlayerData();
    }

    /// <summary>
    /// التحقق من إمكانية رفع المستوى
    /// </summary>
    private void CheckForLevelUp()
    {
        while (xp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// رفع مستوى اللاعب
    /// </summary>
    private void LevelUp()
    {
        xp -= xpToNextLevel; // خصم XP المطلوب للمستوى الحالي
        level++;
        CalculateXPToNextLevel();
        Debug.Log($"Player Leveled Up to Level {level}! Next XP to level up: {xpToNextLevel}");
        // يمكن إضافة مكافآت للمستوى هنا (مثل عملات، طاقة، فتح ألعاب جديدة)
        // RestoreEnergy(maxEnergy); // مثال: استعادة الطاقة بالكامل عند رفع المستوى
    }

    /// <summary>
    /// حساب نقاط الخبرة المطلوبة للمستوى التالي
    /// يمكن تعديل هذه المعادلة لجعل التقدم أسرع أو أبطأ
    /// </summary>
    private void CalculateXPToNextLevel()
    {
        // مثال: XP المطلوب = 100 * (المستوى الحالي)^1.5
        xpToNextLevel = Mathf.RoundToInt(100 * Mathf.Pow(level, 1.5f));
        if (xpToNextLevel < 100) xpToNextLevel = 100; // ضمان حد أدنى للـ XP المطلوب
    }

    public int GetXP() => xp;
    public int GetLevel() => level;
    public int GetXPToNextLevel() => xpToNextLevel;
    #endregion

    #region Audio Settings
    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;
        if (backgroundMusic != null)
        {
            if (enabled)
                backgroundMusic.Play();
            else
                backgroundMusic.Pause();
        }
        AudioManager.Instance?.ApplySettings(musicEnabled, soundEffectsEnabled, musicVolume, sfxVolume);
        SavePlayerData();
    }

    public void SetSoundEffectsEnabled(bool enabled)
    {
        soundEffectsEnabled = enabled;
        AudioManager.Instance?.ApplySettings(musicEnabled, soundEffectsEnabled, musicVolume, sfxVolume);
        SavePlayerData();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (backgroundMusic != null)
            backgroundMusic.volume = musicVolume;
        AudioManager.Instance?.ApplySettings(musicEnabled, soundEffectsEnabled, musicVolume, sfxVolume);
        SavePlayerData();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        AudioManager.Instance?.ApplySettings(musicEnabled, soundEffectsEnabled, musicVolume, sfxVolume);
        SavePlayerData();
    }

    public bool IsMusicEnabled() => musicEnabled;
    public bool IsSoundEffectsEnabled() => soundEffectsEnabled;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    #endregion

    #region Scene Management
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    #region Data Persistence
    private void SavePlayerData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Energy", energy);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SoundEffectsEnabled", soundEffectsEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("XPToNextLevel", xpToNextLevel);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        energy = PlayerPrefs.GetInt("Energy", maxEnergy);
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        soundEffectsEnabled = PlayerPrefs.GetInt("SoundEffectsEnabled", 1) == 1;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        xp = PlayerPrefs.GetInt("XP", 0);
        level = PlayerPrefs.GetInt("Level", 1);
        xpToNextLevel = PlayerPrefs.GetInt("XPToNextLevel", 100);
    }

    public void ResetPlayerData()
    {
        coins = 0;
        energy = maxEnergy;
        musicEnabled = true;
        soundEffectsEnabled = true;
        musicVolume = 0.7f;
        sfxVolume = 0.8f;
        xp = 0;
        level = 1;
        xpToNextLevel = 100; // إعادة تعيين XP المطلوب للمستوى الأول
        SavePlayerData();
    }
    #endregion

    public float GetDeltaTime() => deltaTime;
}
