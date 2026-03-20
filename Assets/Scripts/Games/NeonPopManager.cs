using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// NeonPopManager: لعبة النيون المتفجر (Neon Pop)
/// نظام تفجير الفقاعات بألوان مختلفة وتحقيق مستويات
/// Developed by: Abdullah Al-husini
/// </summary>
public class NeonPopManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private int bubblesPerWave = 5;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Scoring")]
    [SerializeField] private int coinsPerBubble = 15;
    [SerializeField] private int comboMultiplier = 2;

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text comboText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private Transform bubblesContainer;

    private XPlayManager gameManager;
    private int score = 0;
    private int combo = 0;
    private float timeRemaining;
    private bool gameActive = true;
    private float spawnTimer = 0f;
    private int bubblesSpawned = 0;

    private List<GameObject> activeBubbles = new List<GameObject>();

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        timeRemaining = gameDuration;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إنشاء Prefab الفقاعة إذا لم يكن موجوداً
        if (bubblePrefab == null)
            CreateBubblePrefab();
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

        // توليد الفقاعات
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && bubblesSpawned < bubblesPerWave * 10)
        {
            SpawnBubble();
            spawnTimer = 0f;
        }

        UpdateUI();
    }

    private void SpawnBubble()
    {
        if (bubblePrefab == null || bubblesContainer == null)
            return;

        GameObject bubble = Instantiate(bubblePrefab, bubblesContainer);
        RectTransform rectTransform = bubble.GetComponent<RectTransform>();

        // موضع عشوائي
        rectTransform.anchoredPosition = new Vector2(
            Random.Range(-400, 400),
            Random.Range(-600, 600)
        );

        // لون عشوائي
        Image bubbleImage = bubble.GetComponent<Image>();
        if (bubbleImage != null)
        {
            bubbleImage.color = new Color(Random.value, Random.value, Random.value);
        }

        // إضافة زر للنقر
        Button bubbleButton = bubble.GetComponent<Button>();
        if (bubbleButton != null)
        {
            bubbleButton.onClick.AddListener(() => OnBubblePopped(bubble));
        }

        activeBubbles.Add(bubble);
        bubblesSpawned++;
    }

    private void OnBubblePopped(GameObject bubble)
    {
        if (bubble == null)
            return;

        // إضافة النقاط
        combo++;
        int earnedCoins = coinsPerBubble * combo;
        score += earnedCoins;
        gameManager.AddCoins(earnedCoins);

        // حذف الفقاعة
        activeBubbles.Remove(bubble);
        Destroy(bubble);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"النقاط: {score}";

        if (timerText != null)
            timerText.text = $"الوقت: {Mathf.Max(0, timeRemaining):F1}";

        if (comboText != null)
            comboText.text = $"Combo: {combo}x";
    }

    private void CreateBubblePrefab()
    {
        GameObject bubble = new GameObject("Bubble");
        bubble.AddComponent<Image>();
        bubble.AddComponent<Button>();
        bubble.AddComponent<LayoutElement>().preferredWidth = 80;
        bubble.GetComponent<LayoutElement>().preferredHeight = 80;

        bubblePrefab = bubble;
        Instantiate(bubblePrefab); // إنشاء نسخة للاستخدام
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(score);
        Debug.Log($"Bubble Game Over! Final Score: {score}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
