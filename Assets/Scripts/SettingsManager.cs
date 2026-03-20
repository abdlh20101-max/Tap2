using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SettingsManager: مدير الإعدادات
/// يدير: الموسيقى، الصوت، والإعدادات الأخرى
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Text musicVolumeText;

    [Header("SFX Settings")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Text sfxVolumeText;

    [Header("Other Settings")]
    [SerializeField] private Button resetDataButton;
    [SerializeField] private Button closeButton;

    private XPlayManager gameManager;

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("XPlayManager not found!");
            return;
        }

        // إعداد الموسيقى
        if (musicToggle != null)
        {
            musicToggle.isOn = gameManager.IsMusicEnabled();
            musicToggle.onValueChanged.AddListener(OnMusicToggled);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = gameManager.GetMusicVolume();
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        // إعداد المؤثرات الصوتية
        if (sfxToggle != null)
        {
            sfxToggle.isOn = gameManager.IsSoundEffectsEnabled();
            sfxToggle.onValueChanged.AddListener(OnSFXToggled);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = gameManager.GetSFXVolume();
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        // إعداد الأزرار الأخرى
        if (resetDataButton != null)
            resetDataButton.onClick.AddListener(OnResetDataClicked);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);

        // تحديث عرض مستويات الصوت
        UpdateVolumeDisplays();
    }

    private void OnMusicToggled(bool isEnabled)
    {
        gameManager.SetMusicEnabled(isEnabled);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        gameManager.SetMusicVolume(volume);
        UpdateVolumeDisplays();
    }

    private void OnSFXToggled(bool isEnabled)
    {
        gameManager.SetSoundEffectsEnabled(isEnabled);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        gameManager.SetSFXVolume(volume);
        UpdateVolumeDisplays();
    }

    private void UpdateVolumeDisplays()
    {
        if (musicVolumeText != null)
            musicVolumeText.text = $"{(gameManager.GetMusicVolume() * 100):F0}%";

        if (sfxVolumeText != null)
            sfxVolumeText.text = $"{(gameManager.GetSFXVolume() * 100):F0}%";
    }

    private void OnResetDataClicked()
    {
        if (ConfirmReset())
        {
            gameManager.ResetPlayerData();
            Debug.Log("Player data has been reset!");
        }
    }

    private bool ConfirmReset()
    {
        // في الإنتاج، يجب عرض نافذة تأكيد
        return true;
    }

    private void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }
}
