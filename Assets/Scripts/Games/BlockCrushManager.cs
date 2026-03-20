using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// BlockCrushManager: لعبة سحق المكعبات (Block Crush)
/// نظام ترتيب المكعبات وتفجير الصفوف والأعمدة
/// Developed by: Abdullah Al-husini
/// </summary>
public class BlockCrushManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float gameDuration = 120f;

    [Header("Scoring")]
    [SerializeField] private int coinsPerLine = 50;
    [SerializeField] private int comboMultiplier = 2;

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text comboText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform gridContainer;

    private XPlayManager gameManager;
    private int[,] grid;
    private int score = 0;
    private int combo = 0;
    private float timeRemaining;
    private bool gameActive = true;

    private List<GameObject> blockVisuals = new List<GameObject>();

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        timeRemaining = gameDuration;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إعداد الشبكة
        InitializeGrid();
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

        UpdateUI();
    }

    private void InitializeGrid()
    {
        grid = new int[gridWidth, gridHeight];

        // ملء الشبكة بقيم عشوائية
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = Random.Range(0, 3); // 0 = فارغ، 1-2 = مكعبات
            }
        }

        // رسم الشبكة
        DrawGrid();
    }

    private void DrawGrid()
    {
        if (gridContainer == null)
            return;

        // حذف الرسومات القديمة
        foreach (GameObject visual in blockVisuals)
            Destroy(visual);
        blockVisuals.Clear();

        // رسم الشبكة الجديدة
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] > 0 && blockPrefab != null)
                {
                    GameObject block = Instantiate(blockPrefab, gridContainer);
                    RectTransform rectTransform = block.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = new Vector2(x * 40, y * 40);
                    }

                    blockVisuals.Add(block);
                }
            }
        }
    }

    public void PlaceBlock(int x, int y, int blockType)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return;

        if (grid[x, y] == 0)
        {
            grid[x, y] = blockType;
            CheckForMatches();
            DrawGrid();
        }
    }

    private void CheckForMatches()
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();

        // التحقق من الصفوف الكاملة
        for (int y = 0; y < gridHeight; y++)
        {
            bool rowFull = true;
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] == 0)
                {
                    rowFull = false;
                    break;
                }
            }

            if (rowFull)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    toRemove.Add(new Vector2Int(x, y));
                }
            }
        }

        // التحقق من الأعمدة الكاملة
        for (int x = 0; x < gridWidth; x++)
        {
            bool colFull = true;
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == 0)
                {
                    colFull = false;
                    break;
                }
            }

            if (colFull)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (!toRemove.Contains(new Vector2Int(x, y)))
                        toRemove.Add(new Vector2Int(x, y));
                }
            }
        }

        // إزالة المكعبات المتطابقة
        if (toRemove.Count > 0)
        {
            combo++;
            int earnedCoins = coinsPerLine * toRemove.Count * combo;
            score += earnedCoins;
            gameManager.AddCoins(earnedCoins);

            foreach (Vector2Int pos in toRemove)
            {
                grid[pos.x, pos.y] = 0;
            }
        }
        else
        {
            combo = 0;
        }
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

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(score);
        Debug.Log($"Block Game Over! Final Score: {score}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
