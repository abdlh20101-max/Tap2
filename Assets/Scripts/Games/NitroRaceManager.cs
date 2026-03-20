using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// NitroRaceManager: لعبة سباق النيترو (Nitro Race)
/// سباق سيارات بسيط مع جمع العملات والتجاوز
/// Developed by: Abdullah Al-husini
/// </summary>
public class NitroRaceManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameSpeed = 5f;
    [SerializeField] private float maxGameSpeed = 20f;
    [SerializeField] private float speedIncrement = 0.02f;

    [Header("Player Settings")]
    [SerializeField] private float playerSpeed = 300f;
    [SerializeField] private float playerBoundaryX = 150f;

    [Header("Scoring")]
    [SerializeField] private int coinsPerCoin = 15;
    [SerializeField] private int coinsPerEnemy = 25;

    [Header("UI References")]
    [SerializeField] private Text distanceText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private RectTransform playerCar;
    [SerializeField] private Transform enemyCarsContainer;
    [SerializeField] private Transform coinsContainer;

    private XPlayManager gameManager;
    private float distance = 0f;
    private int collectedCoins = 0;
    private bool gameActive = true;

    private float playerX = 0f;
    private float enemySpawnTimer = 0f;
    private float enemySpawnInterval = 1.5f;
    private float coinSpawnTimer = 0f;
    private float coinSpawnInterval = 2f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activeCoins = new List<GameObject>();

    private void Start()
    {
        gameManager = XPlayManager.Instance;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        playerX = playerCar.anchoredPosition.x;
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

        // توليد السيارات والعملات
        SpawnEnemyCars();
        SpawnCoins();

        // تحديث السيارات والعملات
        UpdateEnemyCars();
        UpdateCoins();

        UpdateUI();
    }

    private void HandlePlayerMovement()
    {
        // معالجة الإدخال
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerX -= playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            playerX += playerSpeed * Time.deltaTime;
        }

        // تحديد الحدود
        playerX = Mathf.Clamp(playerX, -playerBoundaryX, playerBoundaryX);

        playerCar.anchoredPosition = new Vector2(playerX, playerCar.anchoredPosition.y);
    }

    private void SpawnEnemyCars()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            GameObject enemy = new GameObject("EnemyCar");
            enemy.transform.SetParent(enemyCarsContainer);
            
            Image enemyImage = enemy.AddComponent<Image>();
            enemyImage.color = Color.cyan;

            RectTransform rectTransform = enemy.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(60, 100);
            rectTransform.anchoredPosition = new Vector2(Random.Range(-150, 150), 500);

            activeEnemies.Add(enemy);
            enemySpawnTimer = 0f;
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
            rectTransform.sizeDelta = new Vector2(40, 40);
            rectTransform.anchoredPosition = new Vector2(Random.Range(-150, 150), 500);

            activeCoins.Add(coin);
            coinSpawnTimer = 0f;
        }
    }

    private void UpdateEnemyCars()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            RectTransform rectTransform = enemy.GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, gameSpeed * 100 * Time.deltaTime);

            // التحقق من الاصطدام
            if (CheckCollision(rectTransform, playerCar))
            {
                EndGame();
                return;
            }

            // حذف السيارة عند الخروج من الشاشة
            if (rectTransform.anchoredPosition.y < -600)
            {
                Destroy(enemy);
                activeEnemies.RemoveAt(i);
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
            rectTransform.anchoredPosition -= new Vector2(0, gameSpeed * 100 * Time.deltaTime);

            // التحقق من التقاط العملة
            if (CheckCollision(rectTransform, playerCar))
            {
                collectedCoins++;
                gameManager.AddCoins(coinsPerCoin);
                Destroy(coin);
                activeCoins.RemoveAt(i);
            }
            else if (rectTransform.anchoredPosition.y < -600)
            {
                Destroy(coin);
                activeCoins.RemoveAt(i);
            }
        }
    }

    private bool CheckCollision(RectTransform obj1, RectTransform obj2)
    {
        Rect rect1 = new Rect(obj1.anchoredPosition - obj1.sizeDelta / 2, obj1.sizeDelta);
        Rect rect2 = new Rect(obj2.anchoredPosition - obj2.sizeDelta / 2, obj2.sizeDelta);
        return rect1.Overlaps(rect2);
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
        int totalCoins = collectedCoins + (int)(distance / 10) * coinsPerCoin;
        gameManager.AddCoins(totalCoins);
        Debug.Log($"Race Game Over! Distance: {distance}m, Coins: {totalCoins}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
