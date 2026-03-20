using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager: مدير واجهة المستخدم
/// يدير: عرض البيانات، الإعدادات، والانتقالات
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Top Bar References")]
    [SerializeField] private Text coinsDisplayText;
    [SerializeField] private Text energyDisplayText;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Button settingsButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Canvas uiCanvas;

    [Header("Update Interval")]
    [SerializeField] private float updateInterval = 0.5f;

    private XPlayManager gameManager;
    private GameObject settingsPanel;
    private float updateTimer = 0f;
    private bool settingsPanelOpen = false;

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("XPlayManager not found!");
            return;
        }

        // إعداد الأزرار
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ToggleSettingsPanel);

        // تحديث الـ UI الأول
        UpdateUIDisplay();
    }

    private void Update()
    {
        // تحديث الـ UI بفترات منتظمة
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            UpdateUIDisplay();
            updateTimer = 0f;
        }
    }

    private void UpdateUIDisplay()
    {
        if (gameManager == null)
            return;

        // تحديث العملات
        if (coinsDisplayText != null)
        {
            coinsDisplayText.text = $"💰 {gameManager.GetCoins()}";
        }

        // تحديث الطاقة
        if (energyDisplayText != null)
        {
            int currentEnergy = gameManager.GetEnergy();
            int maxEnergy = gameManager.GetMaxEnergy();
            energyDisplayText.text = $"⚡ {currentEnergy} / {maxEnergy}";
        }

        // تحديث شريط الطاقة
        if (energySlider != null)
        {
            energySlider.maxValue = gameManager.GetMaxEnergy();
            energySlider.value = gameManager.GetEnergy();
        }
    }

    private void ToggleSettingsPanel()
    {
        settingsPanelOpen = !settingsPanelOpen;

        if (settingsPanelOpen)
        {
            OpenSettingsPanel();
        }
        else
        {
            CloseSettingsPanel();
        }
    }

    private void OpenSettingsPanel()
    {
        if (settingsPanel == null && settingsPanelPrefab != null && uiCanvas != null)
        {
            settingsPanel = Instantiate(settingsPanelPrefab, uiCanvas.transform);
            
            // إعداد لوحة الإعدادات
            SetupSettingsPanel(settingsPanel);
        }
        else if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    private void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    private void SetupSettingsPanel(GameObject panel)
    {
        // البحث عن الأزرار والـ Toggles في لوحة الإعدادات
        Toggle musicToggle = panel.GetComponentInChildren<Toggle>();
        if (musicToggle != null)
        {
            musicToggle.isOn = gameManager.IsMusicEnabled();
            musicToggle.onValueChanged.AddListener((value) => 
            {
                gameManager.SetMusicEnabled(value);
            });
        }

        // إضافة زر الإغلاق
        Button closeButton = panel.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseSettingsPanel);
        }
    }

    public void ShowNotification(string message, float duration = 2f)
    {
        // إنشاء إشعار مؤقت
        GameObject notificationObj = new GameObject("Notification");
        notificationObj.transform.SetParent(uiCanvas.transform);

        Text notificationText = notificationObj.AddComponent<Text>();
        notificationText.text = message;
        notificationText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        notificationText.fontSize = 30;
        notificationText.alignment = TextAnchor.MiddleCenter;

        RectTransform rectTransform = notificationObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(800, 100);

        // حذف الإشعار بعد المدة المحددة
        Destroy(notificationObj, duration);
    }

    public void UpdateCoinsDisplay(int coins)
    {
        if (coinsDisplayText != null)
            coinsDisplayText.text = $"💰 {coins}";
    }

    public void UpdateEnergyDisplay(int current, int max)
    {
        if (energyDisplayText != null)
            energyDisplayText.text = $"⚡ {current} / {max}";

        if (energySlider != null)
        {
            energySlider.maxValue = max;
            energySlider.value = current;
        }
    }
}
