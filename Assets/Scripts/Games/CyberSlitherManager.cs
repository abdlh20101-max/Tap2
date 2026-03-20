using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// CyberSlitherManager: لعبة الانزلاق السيبراني (Cyber Slither)
/// نظام الثعبان الذي ينمو بجمع البيانات والمنافسة مع الـ AI
/// Developed by: Abdullah Al-husini
/// </summary>
public class CyberSlitherManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private int initialLength = 3;
    [SerializeField] private int coinsPerFood = 20;

    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 20;
    [SerializeField] private int gridHeight = 30;

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text lengthText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private GameObject snakeSegmentPrefab;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform gameBoard;

    private XPlayManager gameManager;
    private List<Vector2Int> snakeBody = new List<Vector2Int>();
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int nextDirection = Vector2Int.right;
    private Vector2Int foodPosition;
    private int score = 0;
    private bool gameActive = true;
    private float moveTimer = 0f;

    private List<GameObject> snakeVisuals = new List<GameObject>();
    private GameObject foodVisual;

    private void Start()
    {
        gameManager = XPlayManager.Instance;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إعداد الثعبان
        InitializeSnake();

        // إنشاء الطعام الأول
        SpawnFood();
    }

    private void Update()
    {
        if (!gameActive)
            return;

        // معالجة الـ Input
        HandleInput();

        // حركة الثعبان
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveSpeed)
        {
            MoveSnake();
            moveTimer = 0f;
        }

        UpdateUI();
    }

    private void InitializeSnake()
    {
        snakeBody.Clear();
        for (int i = 0; i < initialLength; i++)
        {
            snakeBody.Add(new Vector2Int(gridWidth / 2 - i, gridHeight / 2));
        }

        // إنشاء الرسومات
        foreach (Vector2Int segment in snakeBody)
        {
            CreateSegmentVisual(segment);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down)
            nextDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up)
            nextDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right)
            nextDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left)
            nextDirection = Vector2Int.right;
    }

    private void MoveSnake()
    {
        direction = nextDirection;

        // حساب الرأس الجديد
        Vector2Int newHead = snakeBody[0] + direction;

        // التحقق من الاصطدام بالجدران
        if (newHead.x < 0 || newHead.x >= gridWidth || newHead.y < 0 || newHead.y >= gridHeight)
        {
            EndGame();
            return;
        }

        // التحقق من الاصطدام بالجسم
        if (snakeBody.Contains(newHead))
        {
            EndGame();
            return;
        }

        // إضافة الرأس الجديد
        snakeBody.Insert(0, newHead);

        // التحقق من الطعام
        if (newHead == foodPosition)
        {
            score += coinsPerFood;
            gameManager.AddCoins(coinsPerFood);
            SpawnFood();
        }
        else
        {
            // إزالة الذيل
            Vector2Int tail = snakeBody[snakeBody.Count - 1];
            snakeBody.RemoveAt(snakeBody.Count - 1);
            DestroySegmentVisual(tail);
        }

        // تحديث الرسومات
        CreateSegmentVisual(newHead);
    }

    private void SpawnFood()
    {
        do
        {
            foodPosition = new Vector2Int(
                Random.Range(0, gridWidth),
                Random.Range(0, gridHeight)
            );
        } while (snakeBody.Contains(foodPosition));

        // تحديث الرسومات
        if (foodVisual != null)
            Destroy(foodVisual);

        if (foodPrefab != null && gameBoard != null)
        {
            foodVisual = Instantiate(foodPrefab, gameBoard);
            RectTransform rectTransform = foodVisual.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(
                    foodPosition.x * 20 - gridWidth * 10,
                    foodPosition.y * 20 - gridHeight * 10
                );
            }
        }
    }

    private void CreateSegmentVisual(Vector2Int position)
    {
        if (snakeSegmentPrefab == null || gameBoard == null)
            return;

        GameObject segment = Instantiate(snakeSegmentPrefab, gameBoard);
        RectTransform rectTransform = segment.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(
                position.x * 20 - gridWidth * 10,
                position.y * 20 - gridHeight * 10
            );
        }

        snakeVisuals.Add(segment);
    }

    private void DestroySegmentVisual(Vector2Int position)
    {
        if (snakeVisuals.Count > 0)
        {
            Destroy(snakeVisuals[snakeVisuals.Count - 1]);
            snakeVisuals.RemoveAt(snakeVisuals.Count - 1);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"النقاط: {score}";

        if (lengthText != null)
            lengthText.text = $"الطول: {snakeBody.Count}";
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(score);
        Debug.Log($"Snake Game Over! Final Score: {score}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
