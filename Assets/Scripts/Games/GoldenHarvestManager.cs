using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// GoldenHarvestManager: لعبة الحصاد الذهبي
/// نظام الزراعة والتجارة مع إنتاج المواد الخام والبيع
/// Developed by: Abdullah Al-husini
/// </summary>
public class GoldenHarvestManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameSessionDuration = 120f;
    [SerializeField] private int initialCoins = 100;

    [Header("Crop Settings")]
    [SerializeField] private float cropGrowthTime = 10f;
    [SerializeField] private int cropYield = 50;
    [SerializeField] private int cropSellPrice = 25;

    [Header("UI References")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text harvestCountText;
    [SerializeField] private Text timerText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button plantButton;
    [SerializeField] private Button harvestButton;

    [Header("Game Objects")]
    [SerializeField] private Image cropImage;
    [SerializeField] private Image cropGrowthBar;

    private XPlayManager gameManager;
    private int sessionCoins;
    private int harvestCount = 0;
    private float timeRemaining;
    private bool gameActive = true;

    private bool cropPlanted = false;
    private float cropGrowthTimer = 0f;
    private bool cropReady = false;

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        sessionCoins = initialCoins;
        timeRemaining = gameSessionDuration;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        if (plantButton != null)
            plantButton.onClick.AddListener(PlantCrop);

        if (harvestButton != null)
            harvestButton.onClick.AddListener(HarvestCrop);

        UpdateUI();
    }

    private void Update()
    {
        if (!gameActive)
            return;

        // تحديث الوقت
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            EndGame();
            return;
        }

        // تحديث نمو المحصول
        if (cropPlanted && !cropReady)
        {
            cropGrowthTimer += Time.deltaTime;
            float growthProgress = cropGrowthTimer / cropGrowthTime;

            if (growthProgress >= 1f)
            {
                cropReady = true;
                cropGrowthTimer = cropGrowthTime;
            }

            if (cropGrowthBar != null)
                cropGrowthBar.fillAmount = growthProgress;
        }

        UpdateUI();
    }

    private void PlantCrop()
    {
        if (cropPlanted)
        {
            Debug.LogWarning("Crop already planted!");
            return;
        }

        cropPlanted = true;
        cropReady = false;
        cropGrowthTimer = 0f;

        if (cropImage != null)
            cropImage.color = new Color(0.5f, 0.5f, 0.5f); // لون أفتح للمحصول الصغير
    }

    private void HarvestCrop()
    {
        if (!cropPlanted || !cropReady)
        {
            Debug.LogWarning("Crop is not ready to harvest!");
            return;
        }

        // إضافة الأرباح
        sessionCoins += cropSellPrice;
        harvestCount++;

        // إعادة تعيين
        cropPlanted = false;
        cropReady = false;
        cropGrowthTimer = 0f;

        if (cropImage != null)
            cropImage.color = Color.white;

        if (cropGrowthBar != null)
            cropGrowthBar.fillAmount = 0f;
    }

    private void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = $"💰 {sessionCoins}";

        if (harvestCountText != null)
            harvestCountText.text = $"الحصاد: {harvestCount}";

        if (timerText != null)
            timerText.text = $"الوقت: {Mathf.Max(0, timeRemaining):F1}";

        // تحديث حالة الأزرار
        if (plantButton != null)
            plantButton.interactable = !cropPlanted;

        if (harvestButton != null)
            harvestButton.interactable = cropPlanted && cropReady;
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(sessionCoins);
        Debug.Log($"Farm Game Over! Earned: {sessionCoins} coins");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
