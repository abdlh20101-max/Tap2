using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// DashEscapeManager: لعبة الهروب السريع (Dash Escape)
/// الجري والقفز والانزلاق لتجاوز العقبات وجمع العملات
/// Developed by: Abdullah Al-husini
/// </summary>
public class DashEscapeManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameSpeed = 5f;
    [SerializeField] private float maxGameSpeed = 15f;
    [SerializeField] private float speedIncrement = 0.01f;

    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 20f;

    [Header("Scoring")]
    [SerializeField] private int coinsPerCoin = 10;
    [SerializeField] private int coinsPerMeter = 5;

    [Header("UI References")]
    [SerializeField] private Text distanceText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private RectTransform player;
    [SerializeField] private Transform obstaclesContainer;
    [SerializeField] private Transform coinsContainer;

    private XPlayManager gameManager;
    private float distance = 0f;
    private int collectedCoins = 0;
    private bool gameActive = true;
    private bool isJumping = false;
    private float playerVelocityY = 0f;
    private float playerY = 0f;

    private float obstacleSpawnTimer = 0f;
    private float obstacleSpawnInterval = 2f;
    private float coinSpawnTimer = 0f;
    private float coinSpawnInterval = 3f;

    private List<GameObject> activeObstacles = new List<GameObject>();
    private List<GameObject> activeCoins = new List<GameObject>();

    private void Start()
    {
        gameManager = XPlayManager.Instance;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        playerY = player.anchoredPosition.y;
    }

    private void Update()
    {
        if (!gameActive)
            return;

        // زيادة السرعة تدريجياً
        gameSpeed = Mathf.Min(gameSpeed + speedIncrement * Time.deltaTime, maxGameSpeed);

        // زيادة المسافة
        distance += gameSpeed * Time.deltaTime;

        // معالجة حركة اللاعب
        HandlePlayerMovement();

        // توليد العقبات والعملات
        SpawnObstacles();
        SpawnCoins();

        // تحديث العقبات والعملات
        UpdateObstacles();
        UpdateCoins();

        UpdateUI();
    }

    private void HandlePlayerMovement()
    {
        // معالجة القفز
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isJumping)
        {
            isJumping = true;
            playerVelocityY = jumpForce;
        }

        // تطبيق الجاذبية
        if (isJumping)
        {
            playerVelocityY -= gravity * Time.deltaTime;
            playerY += playerVelocityY * Time.deltaTime;

            if (playerY <= 0)
            {
                playerY = 0;
                playerVelocityY = 0;
                isJumping = false;
            }

            player.anchoredPosition = new Vector2(player.anchoredPosition.x, playerY);
        }
    }

    private void SpawnObstacles()
    {
        obstacleSpawnTimer += Time.deltaTime;
        if (obstacleSpawnTimer >= obstacleSpawnInterval)
        {
            GameObject obstacle = new GameObject("Obstacle");
            obstacle.transform.SetParent(obstaclesContainer);
            
            Image obstacleImage = obstacle.AddComponent<Image>();
            obstacleImage.color = Color.red;

            RectTransform rectTransform = obstacle.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 100);
            rectTransform.anchoredPosition = new Vector2(500, 0);

            activeObstacles.Add(obstacle);
            obstacleSpawnTimer = 0f;
        }
    }

    private void SpawnCoins()
    {
        coinSpawnTimer += Time.deltaTime;
        if (coinSpawnTimer >= coinSpawnInterval)
        {
            GameObject coin = new GameObject("Coin");
            coin.transform.SetParent(coinsContainer);
            
            Image coinImage = coin.AddComponent<Image>();
            coinImage.color = Color.yellow;

            RectTransform rectTransform = coin.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(30, 30);
            rectTransform.anchoredPosition = new Vector2(500, Random.Range(50, 300));

            activeCoins.Add(coin);
            coinSpawnTimer = 0f;
        }
    }

    private void UpdateObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];
            if (obstacle == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            RectTransform rectTransform = obstacle.GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(gameSpeed * 100 * Time.deltaTime, 0);

            // التحقق من الاصطدام
            if (CheckCollision(rectTransform, player))
            {
                EndGame();
                return;
            }

            // حذف العقبة عند الخروج من الشاشة
            if (rectTransform.anchoredPosition.x < -500)
            {
                Destroy(obstacle);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    private void UpdateCoins()
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = activeCoins[i];
            if (coin == null)
            {
                activeCoins.RemoveAt(i);
                continue;
            }

            RectTransform rectTransform = coin.GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(gameSpeed * 100 * Time.deltaTime, 0);

            // التحقق من التقاط العملة
            if (CheckCollision(rectTransform, player))
            {
                collectedCoins++;
                gameManager.AddCoins(coinsPerCoin);
                Destroy(coin);
                activeCoins.RemoveAt(i);
            }
            else if (rectTransform.anchoredPosition.x < -500)
            {
                Destroy(coin);
                activeCoins.RemoveAt(i);
            }
        }
    }

    private bool CheckCollision(RectTransform obj1, RectTransform obj2)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(obj1, obj2.position) ||
               RectTransformUtility.RectangleContainsScreenPoint(obj2, obj1.position);
    }

    private void UpdateUI()
    {
        if (distanceText != null)
            distanceText.text = $"المسافة: {(int)distance}m";

        if (coinsText != null)
            coinsText.text = $"💰 {collectedCoins}";
    }

    private void EndGame()
    {
        gameActive = false;
        int totalCoins = collectedCoins + (int)(distance / 10) * coinsPerMeter;
        gameManager.AddCoins(totalCoins);
        Debug.Log($"Runner Game Over! Distance: {distance}m, Coins: {totalCoins}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
